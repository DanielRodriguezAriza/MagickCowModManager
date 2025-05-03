using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Data
{
    public enum FileHandlingMode
    {
        Copy = 0,
        SymbolicLink,
        HardLink
    }
}
