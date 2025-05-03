using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Data
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
            Name = string.Empty;
            Description = string.Empty;
            IsUnsafe = false;
            Authors = Array.Empty<AuthorInfo>();
            Version = "1.0.0";
        }
    }
}
