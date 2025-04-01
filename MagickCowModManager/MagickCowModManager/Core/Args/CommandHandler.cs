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
                    Function = CmdHelp
                }
            };
        }

        private void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        public void CmdHelp(string[] args, int index)
        {
            Print($"Usage : mcow-mm [OPTIONS] <ARGS>");
            foreach (var cmd in this.Commands)
            {
                Print($"    -{cmd.ShortCommand}, --{cmd.LongCommand} {cmd.ArgumentsString}{cmd.Description}");
            }
        }
    }
}
