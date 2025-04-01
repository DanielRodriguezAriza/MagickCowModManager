using MagickCowModManager.Core.Exceptions;
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

        private bool cmdvar_HelpWasExecuted;

        private bool cmdvar_ModsContentPathSet;
        private bool cmdvar_GameContentPathSet;
        private string cmdvar_ModsContentPath;
        private string cmdvar_GameContentPath;

        private bool cmdvar_ListMods;
        private bool cmdvar_ListProfiles;
        
        private bool cmdvar_ApplyProfile;
        private string cmdvar_Profile;

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
                    Arguments = [],
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
                    ShortCommand = "lm",
                    LongCommand = "list-mods",
                    Arguments = [],
                    Description = "Display the list of all mods stored within the \"Mods\" folder",
                    Function = CmdListMods
                },
                new Command
                {
                    ShortCommand = "lp",
                    LongCommand = "list-profiles",
                    Arguments = [],
                    Description = "Display the list of all profiles stored within the \"Mods\" folder",
                    Function = CmdListProfiles
                },
                new Command
                {
                    ShortCommand = "a",
                    LongCommand = "apply-profile",
                    Arguments = ["profile-name"],
                    Description = "Apply a mod profile",
                    Function = CmdApplyProfile
                },
            ];

            cmdvar_HelpWasExecuted = false;

            cmdvar_ModsContentPathSet = false;
            cmdvar_GameContentPathSet = false;
            cmdvar_ModsContentPath = "./Mods";
            cmdvar_GameContentPath = "./Content";

            cmdvar_ListMods = false;
            cmdvar_ListProfiles = false;

            cmdvar_ApplyProfile = false;
            cmdvar_Profile = "Default";
        }

        #endregion

        #region Public Methods - Execution

        public void Run()
        {
            // NEVER run any commands if the help command was used.
            if (this.cmdvar_HelpWasExecuted)
                return;

            this.ModManager.ModsContentPath = this.cmdvar_ModsContentPath;
            this.ModManager.GameContentPath = this.cmdvar_GameContentPath;

            if (this.cmdvar_ListMods)
                this.ModManager.ListMods();

            if (this.cmdvar_ListProfiles)
                this.ModManager.ListProfiles();

            if(this.cmdvar_ApplyProfile)
                this.ModManager.ApplyProfile(this.cmdvar_Profile);
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
            this.cmdvar_HelpWasExecuted = true;
            Print($"Usage : mcow-mm [OPTIONS] <ARGS>");
            foreach (var cmd in this.Commands)
            {
                Print($"    -{cmd.ShortCommand}, --{cmd.LongCommand} {cmd.ArgumentsString}{cmd.Description}");
            }
        }

        public void CmdSetPath_Game(string[] args, int index)
        {
            if (this.cmdvar_ModsContentPathSet || this.cmdvar_GameContentPathSet)
            {
                throw new CommandException("Content paths cannot be set more than once in a single call!");
            }

            string gamePath = args[index + 1];
            
            this.cmdvar_ModsContentPath = Path.Combine(gamePath, "Mods");
            this.cmdvar_GameContentPath = Path.Combine(gamePath, "Content");

            this.cmdvar_ModsContentPathSet = true;
            this.cmdvar_GameContentPathSet = true;
        }

        public void CmdSetPath_ModsContent(string[] args, int index)
        {
            if (this.cmdvar_ModsContentPathSet)
            {
                throw new CommandException("Mods Content path cannot be set more than once in a single call!");
            }

            this.cmdvar_ModsContentPath = args[index + 1];
        }

        public void CmdSetPath_GameContent(string[] args, int index)
        {
            if (this.cmdvar_GameContentPathSet)
            {
                throw new CommandException("Game Content path cannot be set more than once in a single call!");
            }

            this.cmdvar_GameContentPath = args[index + 1];
        }

        public void CmdListMods(string[] args, int index)
        {
            this.cmdvar_ListMods = true;
        }

        public void CmdListProfiles(string[] args, int index)
        {
            this.cmdvar_ListProfiles = true;
        }

        public void CmdApplyProfile(string[] args, int index)
        {
            if (this.cmdvar_ApplyProfile)
            {
                throw new CommandException("Cannot apply multiple profiles in a single call!");
            }

            this.cmdvar_ApplyProfile = true;
            this.cmdvar_Profile = args[index + 1];
        }

        #endregion
    }
}
