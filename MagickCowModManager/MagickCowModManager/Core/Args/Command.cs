using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Args
{
    public struct Command
    {
        public string ShortCommand { get; set; }
        public string LongCommand { get; set; }
        public string[] Arguments { get; set; }
        public string Description { get; set; }
        public Action<string[], int> Function { get; set; }
    }
}
