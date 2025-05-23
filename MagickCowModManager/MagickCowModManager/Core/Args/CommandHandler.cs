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
                new Command
                {
                    ShortCommand = "pi",
                    LongCommand = "set-path-to-installs",
                    Arguments = ["path-to-installs"],
                    Description = "Set the path where the base game installs are located",
                    Function = CmdSetPathToInstalls
                },
                new Command
                {
                    ShortCommand = "pm",
                    LongCommand = "set-path-to-mods",
                    Arguments = ["path-to-mods"],
                    Description = "Set the path where the mods are located",
                    Function = CmdSetPathToMods
                },
                new Command
                {
                    ShortCommand = "pp",
                    LongCommand = "set-path-to-profiles",
                    Arguments = ["path-to-profiles"],
                    Description = "Set the path where the profiles are located",
                    Function = CmdSetPathToProfiles
                },
                new Command
                {
                    ShortCommand = "li",
                    LongCommand = "list-installs",
                    Arguments = [],
                    Description = "Display the list of all the installs found within the installs directory",
                    Function = CmdListInstalls
                },
                new Command
                {
                    ShortCommand = "lm",
                    LongCommand = "list-mods",
                    Arguments = [],
                    Description = "Display the list of all mods found within the mods directory",
                    Function = CmdListMods
                },
                new Command
                {
                    ShortCommand = "lp",
                    LongCommand = "list-profiles",
                    Arguments = [],
                    Description = "Display the list of all profiles found within the profiles directory",
                    Function = CmdListProfiles
                },
                new Command
                {
                    ShortCommand = "a",
                    LongCommand = "apply-profile",
                    Arguments = ["profile-name"],
                    Description = "Apply a profile, generates the final game files with the selected base install and mods",
                    Function = CmdApplyProfile
                },
            ];
        }

        #region Cmd - Utility

        public void CmdHelp(string[] args, int index)
        {
            // TODO : Implement
        }

        #endregion

        #region CmdSetPath

        void CmdSetPathToInstalls(string[] args, int index)
        {
            // TODO : Implement
        }

        void CmdSetPathToMods(string[] args, int index)
        {
            // TODO : Implement
        }

        void CmdSetPathToProfiles(string[] args, int index)
        {
            // TODO : Implement
        }

        #endregion

        #region CmdList

        void CmdListInstalls(string[] args, int index)
        {
            // TODO : Implement
        }

        void CmdListMods(string[] args, int index)
        {
            // TODO : Implement
        }

        void CmdListProfiles(string[] args, int index)
        {
            // TODO : Implement
        }

        #endregion

        #region CmdApply

        void CmdApplyProfile(string[] args, int index)
        {
            // TODO : Implement
        }

        #endregion
    }
}
