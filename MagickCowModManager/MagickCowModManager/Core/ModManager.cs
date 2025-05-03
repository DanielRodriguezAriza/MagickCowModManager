using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public class ModManager
    {
        // NOTE : We could enforce having all of the mod manager data located within the same directory and path and use a single string for this, but I think some
        // users will appreciate the flexibility in the long run, maybe, idk...

        public string PathInstalls { get; set; } // Path where the base game installs are located
        public string PathMods { get; set; } // Path where the mod files are located
        public string PathProfiles { get; set; } // Path where the profiles are located

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
    }
}
