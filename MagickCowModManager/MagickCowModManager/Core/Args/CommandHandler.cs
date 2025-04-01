using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Args
{
    public class CommandHandler
    {
        #region Variables

        public ModManager ModManager { get; set; } // Reference to a mod manager instance.
        public Command[] Commands { get; set; } // List of known commands.

        #endregion

        #region Command Variables

        private bool cmdHelpWasExecuted;

        #endregion

        #region Constructor

        public CommandHandler(ModManager modManager)
        {
            this.ModManager = modManager;
            this.Commands =
            [
                new Command
                {
                    ShortCommand = "h",
                    LongCommand = "help",
                    Arguments = Array.Empty<string>(),
                    Description = "Display this help message.",
                    Function = CmdHelp
                }
            ];

            this.cmdHelpWasExecuted = false;
        }

        #endregion

        #region Public Methods - Execution

        public void Run()
        {
            // NEVER run any commands if the help command was used.
            if (this.cmdHelpWasExecuted)
                return;

            // TODO : Implement mod manager execution...
        }

        #endregion

        #region Private Methods - Logging

        private void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        #endregion

        #region Private Methods - Cmd

        public void CmdHelp(string[] args, int index)
        {
            this.cmdHelpWasExecuted = true;
            Print($"Usage : mcow-mm [OPTIONS] <ARGS>");
            foreach (var cmd in this.Commands)
            {
                Print($"    -{cmd.ShortCommand}, --{cmd.LongCommand} {cmd.ArgumentsString}{cmd.Description}");
            }
        }

        #endregion
    }
}
