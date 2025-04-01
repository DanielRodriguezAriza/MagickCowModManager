using MagickCowModManager.Core.Args;
using MagickCowModManager.Core.Data;
using MagickCowModManager.Core.Exceptions;
using MagickCowModManager.Core.FileHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public class ModManager
    {
        #region Private Variables

        private FileHandler handler;

        #endregion

        #region Public Properties

        public string ModsContentPath { get; set; }
        public string GameContentPath { get; set; }

        #endregion

        #region Constructors

        public ModManager()
        : this("./Mods", "./Content")
        { }

        public ModManager(string gamePath)
        : this(Path.Combine(gamePath, "Mods"), Path.Combine(gamePath, "Content"))
        { }

        public ModManager(string modsPath, string contentPath)
        {
            this.ModsContentPath = modsPath;
            this.GameContentPath = contentPath;
            this.handler = new FileHandler();

            // Console.WriteLine($"Mods Content Path : {this.ModsContentPath}");
            // Console.WriteLine($"Game Content Path : {this.GameContentPath}");
        }

        #endregion

        #region PublicMethods

        // NOTE : Maybe rename to LoadProfile ? The point is that this function basically loads a profile and sets the active mods to be the ones within this profile.
        public void ApplyProfile(string profileName)
        {
            var profiles = GetProfiles();
            foreach (var profile in profiles)
            {
                if (profile.Name == profileName)
                {
                    Console.WriteLine($"Installing mods from profile \"{profileName}\"!");
                    InstallProfile(profile);
                    return;
                }
            }
            throw new LoadException($"The profile \"{profileName}\" could not be found!");
        }

        public void ListProfiles()
        {
            var profiles = GetProfiles();

            Console.WriteLine("Profiles:");
            Console.WriteLine($"  - Found: {profiles.Length}");
            
            foreach (var profile in profiles)
            {
                Console.WriteLine($"    * {profile.Name}");
            }
        }

        public void ListMods()
        {
            // TODO : Maybe improve this implementation by actually making a real Mod or ModDirectory or whatever kind of struct with info about the mods? Like name, path, etc...
            DirectoryInfo info = new DirectoryInfo(ModsContentPath);
            DirectoryInfo[] childDirectories = info.GetDirectories();

            Console.WriteLine("Mods:");
            Console.WriteLine($"  - Found: {childDirectories.Length}");

            foreach (var mod in childDirectories)
            {
                Console.WriteLine($"    * {mod.Name}");
            }
        }

        public Profile[] GetProfiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(this.ModsContentPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            List<Profile> foundProfiles = new List<Profile>();

            foreach (var fileInfo in fileInfos)
            {
                try
                {
                    Profile profile = GetProfile(fileInfo);
                    foundProfiles.Add(profile);
                }
                catch
                {
                    // Console.WriteLine($"The file \"{fileInfo.FullName}\" is not a valid profile file!");
                    // Do nothing, just skip the file if it's not a valid JSON file that matches the ModProfile "schema"...
                }
            }

            return foundProfiles.ToArray();
        }

        public Profile GetProfile(FileInfo fileInfo)
        {
            string json = File.ReadAllText(fileInfo.FullName);

            Profile profile = JsonSerializer.Deserialize<Profile>(json);
            profile.Name = fileInfo.Name;
            
            return profile;
        }

        public void ParseArguments(string[] args)
        {
            ArgParser argParser = new ArgParser(this);
            argParser.Parse(args);
        }

        #endregion

        #region PrivateMethods

        private void InstallProfile(Profile profile)
        {
            CopyFileHandler handler = new CopyFileHandler(profile, this.ModsContentPath, this.GameContentPath);
            handler.InstallProfile();
        }
        
        #endregion
    }
}
