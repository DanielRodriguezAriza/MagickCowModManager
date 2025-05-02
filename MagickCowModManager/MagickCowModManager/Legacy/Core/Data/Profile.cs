using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MagickCowModManager.Legacy.Core.Data
{
    // Describes a mod profile, with a list of the mods enabled for this given profile.
    // NOTE : The profile name is given by the string of the file, so no need to store it internally in here within this class / struct.
    public struct Profile
    {
        [JsonIgnore]
        public string Name { get; set; }
        public List<string> EnabledMods { get; set; }

        public Profile()
        {
            Name = "Default";
            EnabledMods = new List<string>();
        }

        public Profile(string name)
        {
            Name = name;
            EnabledMods = new List<string>();
        }

        public Profile(string name, List<string> enabledMods)
        {
            Name = name;
            EnabledMods = new List<string>(enabledMods);
        }

        public Profile(string name, string[] enabledMods)
        {
            Name = name;
            EnabledMods = new List<string>(enabledMods);
        }
    }
}
