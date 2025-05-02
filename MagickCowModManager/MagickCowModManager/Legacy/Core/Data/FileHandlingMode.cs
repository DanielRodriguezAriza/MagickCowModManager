using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Legacy.Core.Data
{
    public enum FileHandlingMode
    {
        Copy,
        SymbolicLink,
        Hardlink,
    }
}
