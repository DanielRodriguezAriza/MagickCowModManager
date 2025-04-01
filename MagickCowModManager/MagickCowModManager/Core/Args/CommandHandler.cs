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

        private string modsContentPath;
        private string gameContentPath;

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
                    Description = "Display this help message",
                    Function = CmdHelp
                },
                new Command
                {
                    ShortCommand = "pg",
                    LongCommand = "set-game-path",
                    Arguments = ["game-path"],
                    Description = "Set the path where Magicka is located",
                    Function = CmdSetPath_Game
                },
                new Command
                {
                    ShortCommand = "pm",
                    LongCommand = "set-mods-content-path",
                    Arguments = ["mods-content-path"],
                    Description = "Set the path where the \"Mods\" folder is located", // NOTE : With this command, the path could be named anything the user wants other than Mods.
                    Function = CmdSetPath_ModsContent
                },
                new Command
                {
                    ShortCommand = "pc",
                    LongCommand = "set-game-content-path",
                    Arguments = ["game-content-path"],
                    Description = "Set the path where the \"Content\" folder is located",
                    Function = CmdSetPath_GameContent
                },
                new Command
                {

                }
            ];

            this.cmdHelpWasExecuted = false;
            this.modsContentPath = "./Mods";
            this.gameContentPath = "./Content";
        }

        #endregion

        #region Public Methods - Execution

        public void Run()
        {
            // NEVER run any commands if the help command was used.
            if (this.cmdHelpWasExecuted)
                return;

            this.ModManager.ModsContentPath = this.modsContentPath;
            this.ModManager.GameContentPath = this.gameContentPath;
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

        public void CmdSetPath_Game(string[] args, int index)
        {
            string gamePath = args[index + 1];
            this.modsContentPath = Path.Combine(gamePath, "Mods");
            this.gameContentPath = Path.Combine(gamePath, "Content");
        }

        public void CmdSetPath_ModsContent(string[] args, int index)
        {
            this.modsContentPath = args[index + 1];
        }

        public void CmdSetPath_GameContent(string[] args, int index)
        {
            this.gameContentPath = args[index + 1];
        }

        #endregion
    }
}
