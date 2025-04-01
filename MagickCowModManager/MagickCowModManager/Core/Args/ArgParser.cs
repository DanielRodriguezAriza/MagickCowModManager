using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Args
{
    public class ArgParser
    {
        private ModManager modManager; // Reference to the mod manager instance itself.
        private Command[] commands; // List of known commands.

        public ArgParser(ModManager modManager)
        {
            this.modManager = modManager;
            this.commands = new Command[]
            {
                new Command
                {
                    ShortCommand = "h",
                    LongCommand = "help",
                    Arguments = Array.Empty<string>(),
                    Description = "Display this help message.",
                    Function = null
                }
            };
        }

        public void Parse(string[] args)
        {
            // TODO : Implement
        }
    }
}
