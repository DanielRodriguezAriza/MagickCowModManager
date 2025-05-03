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

            if (!File.Exists(this.PathInstalls))
            {
                throw new Exception("The selected Installs path does not exist");
            }

            if (!File.Exists(this.PathMods))
            {
                throw new Exception("The selected Mods path does not exist");
            }

            if (!File.Exists(this.PathProfiles))
            {
                throw new Exception("The selected Profiles path does not exist");
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
