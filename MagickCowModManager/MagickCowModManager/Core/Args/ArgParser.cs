using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Args
{
    public class ArgParser
    {
        private CommandHandler commandHandler; // Reference to a command handler instance.

        public ArgParser(ModManager modManager)
        {
            this.commandHandler = new CommandHandler(modManager);
        }

        public void Parse(string[] args)
        {
            // TODO : Implement
        }
    }
}
