using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Data
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
            this.Name = "Default";
            this.EnabledMods = new List<string>();
        }

        public Profile(string name)
        {
            this.Name = name;
            this.EnabledMods = new List<string>();
        }

        public Profile(string name, List<string> enabledMods)
        {
            this.Name = name;
            this.EnabledMods = new List<string>(enabledMods);
        }

        public Profile(string name, string[] enabledMods)
        {
            this.Name = name;
            this.EnabledMods = new List<string>(enabledMods);
        }
    }
}
