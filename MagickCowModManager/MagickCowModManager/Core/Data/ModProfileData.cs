using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Data
{
    // Describes a mod profile, with a list of the mods enabled for this given profile.
    // NOTE : The profile name is given by the string of the file, so no need to store it internally in here within this class / struct.
    public struct ModProfileData
    {
        public List<string> EnabledMods { get; set; }

        public ModProfileData()
        {
            this.EnabledMods = new List<string>();
        }

        public ModProfileData(List<string> enabledMods)
        {
            this.EnabledMods = new List<string>(enabledMods);
        }

        public ModProfileData(string[] enabledMods)
        {
            this.EnabledMods = new List<string>(enabledMods);
        }
    }
}
