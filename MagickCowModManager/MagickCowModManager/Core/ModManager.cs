using MagickCowModManager.Core.Data;
using MagickCowModManager.Core.IO;
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
        #region Comments

        // NOTE : We could enforce having all of the mod manager data located within the same directory and path and use a single string for this, but I think some
        // users will appreciate the flexibility in the long run, maybe, idk...

        #endregion

        #region Variables

        public string PathInstalls { get; set; } // Path where the base game installs are located
        public string PathMods { get; set; } // Path where the mod files are located
        public string PathProfiles { get; set; } // Path where the profiles are located

        #endregion

        #region Constructors

        // Constructor that allows the user to specify the paths for the installs, mods and profiles directories manually
        public ModManager(string pathInstalls, string pathMods, string pathProfiles)
        {
            this.PathInstalls = pathInstalls;
            this.PathMods = pathMods;
            this.PathProfiles = pathProfiles;
        }

        // Constructor that allows the user to specify the path where the mod manager data is located
        public ModManager(string path)
        : this(Path.Combine(path, "installs"), Path.Combine(path, "mods"), Path.Combine(path, "profiles"))
        { }

        // Constructor that allows the user to directly assume that the current working directory is where the mod manager data is located
        public ModManager()
        : this("./")
        { }

        #endregion

        #region Path Checking

        public bool PathIsValidInstalls()
        {
            return Directory.Exists(this.PathInstalls);
        }

        public bool PathIsValidMods()
        {
            return Directory.Exists(this.PathMods);
        }

        public bool PathIsValidProfiles()
        {
            return Directory.Exists(this.PathProfiles);
        }

        public bool PathsAreValid()
        {
            return PathIsValidInstalls() && PathIsValidMods() && PathIsValidProfiles();
        }

        #endregion

        #region ApllyProfile

        public void ApplyProfile(string profileFileName)
        {
            Console.WriteLine($"Installing Profile frome file \"{profileFileName}\"");

            string profilePathDir = Path.Combine(this.PathProfiles, profileFileName);
            string profilePathFile = Path.Combine(this.PathProfiles, profileFileName, "profile.json");
            string profilePathManifest = Path.Combine(this.PathProfiles, profileFileName, "manifest");

            // Get the path to our profile
            DirectoryInfo directoryInfo = new DirectoryInfo(profilePathDir);
            FileInfo fileInfo = new FileInfo(profilePathFile);

            // Handle the case where the profile directory does not exist
            if (!directoryInfo.Exists)
            {
                throw new Exception("The specified profile does not exist!");
            }

            // Handle the case where the profile file does not exist
            if (!fileInfo.Exists)
            {
                throw new Exception("The specified profile is corrupted and is missing files!");
            }

            // Apply the profile with the input data from the profile.json file
            var profileInfo = JsonSerializer.Deserialize<ProfileInfo>(File.ReadAllText(profilePathFile));
            ApplyProfile(profilePathDir, profileInfo);

            // Create manifest file with installation date
            CreateManifest(profilePathManifest);
        }

        public void ApplyProfile(string profileDirPath, ProfileInfo profileInfo)
        {
            Console.WriteLine($"Installing Profile \"{profileInfo.Name}\"");

            string profileGamePath = Path.Combine(profileDirPath, "game");

            // Delete old files
            FileSystemUtil.DeleteDirectory(profileGamePath);
            Directory.CreateDirectory(profileGamePath);

            // Copy all files from installs to our profile
            FileSystemUtil.CopyDirectory(Path.Combine(this.PathInstalls, profileInfo.Install), profileGamePath);

            // Copy all files from mods to our profile
            foreach (var mod in profileInfo.Mods.Reverse()) // NOTE : Reverse order iteration for easy mod overriding implementation
            {
                FileSystemUtil.CopyDirectory(Path.Combine(PathMods, mod), profileGamePath);
            }
        }

        private void CreateManifest(string path)
        {
            File.WriteAllText(path, $"Installed with mcow-mm at {DateTime.Now.ToString()}");
        }

        private void DeleteManifest(string path)
        {
            File.Delete(path);
        }

        #endregion

        #region ListData

        public void ListInstalls()
        {
            ListDirectories("installs", PathInstalls);
        }

        public void ListMods()
        {
            ListDirectories("mods", PathMods);
        }

        public void ListProfiles()
        {
            ListDirectories("profiles", PathProfiles);
        }

        private void ListDirectories(string name, string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            
            if(!directory.Exists)
            {
                Logger.Log(1, $"directories-not-found:{name}", Array.Empty<string>());
                return;
            }

            Logger.Log(0, $"directories-found:{name}", FileSystemUtil.GetChildDirectoriesName(directory));
        }

        #endregion
    }
}
