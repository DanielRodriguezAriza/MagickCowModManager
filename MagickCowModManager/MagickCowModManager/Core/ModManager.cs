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

        public void ApplyProfile(string profileDir)
        {
            ApplyProfile(PathInstalls, PathMods, PathProfiles, profileDir, true);
        }

        public void ApplyProfile(string pathToInstalls, string pathToMods, string pathToProfiles, string profileDir, bool handleManifest)
        {
            // Debug log
            Console.WriteLine($"Installing Profile from directory \"{profileDir}\"");

            // Create path strings
            string pathToProfileDir = Path.Combine(pathToProfiles, profileDir);
            string pathToProfileFile = Path.Combine(pathToProfiles, profileDir, "profile.json");
            string pathToProfileManifest = Path.Combine(pathToProfiles, profileDir, "manifest");
            string pathToProfileGameDir = Path.Combine(pathToProfiles, profileDir, "game");

            // Handle the case where the profile directory does not exist
            if (!Directory.Exists(pathToProfileDir))
            {
                throw new Exception("The specified profile does not exist!");
            }

            // Handle the case where the profile file does not exist
            if (!File.Exists(pathToProfileFile))
            {
                throw new Exception("The specified profile is corrupted and is missing files!");
            }

            // Load profile data from the profile.json file
            Console.WriteLine("    - Loading Profile Data from \"profile.json\"");
            ProfileInfo profileInfo = JsonSerializer.Deserialize<ProfileInfo>(File.ReadAllText(pathToProfileFile));
            Console.WriteLine($"    - Profile Data loaded from \"profile.json\"");

            // Delete the last generated manifest file and game directory
            Console.WriteLine("    - Cleaning up old files...");
            if(handleManifest)
                DeleteManifest(pathToProfileManifest);
            DeleteGameFiles(pathToProfileGameDir);

            // Generate the new game files and manifest
            Console.WriteLine($"    - Generating files for profile named \"{profileInfo.Name}\" from directory \"{profileDir}\"");
            CreateGameFiles(pathToInstalls, pathToMods, pathToProfileGameDir, profileInfo);
            if(handleManifest)
                CreateManifest(pathToProfileManifest);
        }

        private void CreateManifest(string path)
        {
            File.WriteAllText(path, $"Installed with mcow-mm at {DateTime.Now.ToString()}");
        }

        private void DeleteManifest(string path)
        {
            File.Delete(path);
        }

        private void CreateGameFiles(string pathToInstalls, string pathToMods, string pathToProfileGameDir, ProfileInfo profileInfo)
        {
            // Create the directory if it does not exist already
            if (!Directory.Exists(pathToProfileGameDir))
                Directory.CreateDirectory(pathToProfileGameDir);

            // Copy the base install
            FileSystemUtil.CopyDirectory(Path.Combine(pathToInstalls, profileInfo.Install), pathToProfileGameDir);

            // Copy all of the mod files
            // NOTE : Reverse order iteration for easy mod overriding implementation
            foreach (var mod in profileInfo.Mods.Reverse())
            {
                FileSystemUtil.CopyDirectory(Path.Combine(pathToMods, mod), pathToProfileGameDir);
            }
        }

        private void DeleteGameFiles(string dir)
        {
            FileSystemUtil.DeleteDirectory(dir);
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
