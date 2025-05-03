using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public struct ProfileInfo
    {
        public string Name { get; set; }
        public string Install { get; set; }
        public string[] Mods { get; set; }

        public ProfileInfo()
        {
            this.Name = "New Profile";
            this.Install = "Magicka";
            this.Mods = Array.Empty<string>();
        }
    }
}
