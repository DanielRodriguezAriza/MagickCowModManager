using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Data
{
    public struct ProfileInfo
    {
        public string Name { get; set; }
        public string Install { get; set; }
        public string[] Mods { get; set; }

        public ProfileInfo()
        {
            Name = "New Profile";
            Install = "Magicka";
            Mods = Array.Empty<string>();
        }
    }
}
