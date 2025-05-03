using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public struct ModInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUnsafe { get; set; }
        public AuthorInfo[] Authors { get; set; }
        public string Version { get; set; }

        public ModInfo()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.IsUnsafe = false;
            this.Authors = Array.Empty<AuthorInfo>();
            this.Version = "1.0.0";
        }
    }
}
