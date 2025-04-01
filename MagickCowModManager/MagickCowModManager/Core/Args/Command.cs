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

        // Helper getter to get the Arguments as a single string
        public string ArgumentsString
        {
            get
            {
                string argsString = "";
                foreach (var arg in this.Arguments)
                {
                    argsString += $"<{arg}> ";
                }
                return argsString; // NOTE : We could trim this to remove the last space, but we don't for now. This is because the help command exploits the extra space for pretty printing.
            }
        }
    }
}
