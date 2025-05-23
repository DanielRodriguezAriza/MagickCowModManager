using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Args
{
    public class CommandHandler
    {
        public ModManager ModManager { get; set; } // Reference to a mod manager instance. Could really just be instantiated inside of this class directly, but we do it like this just in case that the ModManager class adds some statefullness in the future, which would make requiring an instance as a sort of context make sense.
        public Command[] Commands { get; set; } // List of known commands. Could use a list in the future so that it can be modified with custom commands from the outside in case any users want to add custom commands.

        public CommandHandler(ModManager modManager)
        {
            ModManager = modManager;
            Commands =
            [
                new Command
                {
                    ShortCommand = "h",
                    LongCommand = "help",
                    Arguments = [],
                    Description = "Display this help message",
                    Function = CmdHelp
                },
            ];
        }

        public void CmdHelp(string[] args, int index)
        {
            // TODO : Implement
        }
    }
}
