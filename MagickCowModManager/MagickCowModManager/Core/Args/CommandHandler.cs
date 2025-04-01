using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Args
{
    public class CommandHandler
    {
        public ModManager ModManager { get; set; } // Reference to a mod manager instance.
        public Command[] Commands { get; set; } // List of known commands.

        public CommandHandler(ModManager modManager)
        {
            this.ModManager = modManager;
            this.Commands = new Command[]
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
    }
}
