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

            // Check if the selected paths point to a valid mod manager data install location or not:
            // 1) installs path
            // 2) mods path
            // 3) profiles path
            // NOTE : In the future, maybe modify this to assume a single source path for the mod manager data and just havea simple error message, something like
            // "The selected mod manager data path does not appear to be a valid install location" or whatever the fuck, idk.

            if (!Directory.Exists(this.PathInstalls))
            {
                throw new Exception("The selected Installs path could not be found");
            }

            if (!Directory.Exists(this.PathMods))
            {
                throw new Exception("The selected Mods path could not be found");
            }

            if (!Directory.Exists(this.PathProfiles))
            {
                throw new Exception("The selected Profiles path could not be found");
            }
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

        public void ApplyProfile(string profileFileName)
        {
            Console.WriteLine($"Installing Profile frome file \"{profileFileName}\"");

            string profilePathDir = Path.Combine(this.PathProfiles, profileFileName);
            string profilePathFile = Path.Combine(this.PathProfiles, profileFileName, "profile.json");

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

            var profileInfo = JsonSerializer.Deserialize<ProfileInfo>(File.ReadAllText(profilePathFile));
            ApplyProfile(profilePathDir, profileInfo);
        }

        public void ApplyProfile(string profileDirPath, ProfileInfo profileInfo)
        {
            Console.WriteLine($"Installing Profile \"{profileInfo.Name}\"");

            string profileGamePath = Path.Combine(profileDirPath, "game");

            // Delete old files
            FileSystemUtil.DeleteDirectory(profileGamePath);

            // Copy all files from installs to our profile
            FileSystemUtil.CopyDirectory(Path.Combine(this.PathInstalls, profileInfo.Install), profileGamePath);

            // Copy all files from mods to our profile
            foreach (var mod in profileInfo.Mods.Reverse()) // NOTE : Reverse order iteration for easy mod overriding implementation
            {
                FileSystemUtil.CopyDirectory(Path.Combine(PathMods, mod), profileGamePath);
            }
        }
    }
}
