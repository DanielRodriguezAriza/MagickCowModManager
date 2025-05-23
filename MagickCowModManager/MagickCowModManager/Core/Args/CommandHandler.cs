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

        public ModManager ModManager { get; set; } // Reference to a mod manager instance. Could really just be instantiated inside of this class directly, but we do it like this just in case that the ModManager class adds some statefullness in the future, which would make requiring an instance as a sort of context make sense.
        public Command[] Commands { get; set; } // List of known commands. Could use a list in the future so that it can be modified with custom commands from the outside in case any users want to add custom commands.

        #endregion

        #region Command Variables

        private bool cmdvar_Help;

        private bool cmdvar_SetPathToInstalls;
        private bool cmdvar_SetPathToMods;
        private bool cmdvar_SetPathToProfiles;

        private string cmdvar_PathToInstalls;
        private string cmdvar_PathToMods;
        private string cmdvar_PathToProfiles;

        private bool cmdvar_ListInstalls;
        private bool cmdvar_ListMods;
        private bool cmdvar_ListProfiles;

        private bool cmdvar_ApplyProfile;
        private string cmdvar_Profile;

        #endregion

        #region Constructor

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

        #endregion

        #region CmdHelp

        public void CmdHelp(string[] args, int index)
        {
            cmdvar_Help = true;
            Print($"Usage : mcow-mm [OPTIONS] <ARGS>");
            foreach (var cmd in Commands)
            {
                Print($"    -{cmd.ShortCommand}, --{cmd.LongCommand} {cmd.ArgumentsString}{cmd.Description}");
            }
        }

        #endregion

        #region CmdSetPath

        void CmdSetPathToInstalls(string[] args, int index)
        {
            if (cmdvar_SetPathToInstalls)
                throw new Exception("Path to installs cannot be set more than once in a single call!");
            cmdvar_PathToInstalls = args[index + 1];
        }

        void CmdSetPathToMods(string[] args, int index)
        {
            if (cmdvar_SetPathToMods)
                throw new Exception("Path to mods cannot be set more than once in a single call!");
            cmdvar_PathToMods = args[index + 1];
        }

        void CmdSetPathToProfiles(string[] args, int index)
        {
            if (cmdvar_SetPathToProfiles)
                throw new Exception("Path to profiles cannot be set more than once in a single call!");
            cmdvar_PathToProfiles = args[index + 1];
        }

        #endregion

        #region CmdList

        void CmdListInstalls(string[] args, int index)
        {
            if (cmdvar_ListInstalls)
                throw new Exception("List installs has already been invoked!");
            cmdvar_ListInstalls = true;
        }

        void CmdListMods(string[] args, int index)
        {
            if (cmdvar_ListMods)
                throw new Exception("List mods has already been invoked!");
            cmdvar_ListMods = true;
        }

        void CmdListProfiles(string[] args, int index)
        {
            if (cmdvar_ListProfiles)
                throw new Exception("List profiles has already been invoked!");
            cmdvar_ListProfiles = true;
        }

        #endregion

        #region CmdApply

        void CmdApplyProfile(string[] args, int index)
        {
            // TODO : Implement
        }

        #endregion

        private void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        // Command to execute the logic once all of the commands have been registered.
        public void Execute()
        {
            // NEVER run any commands if the help command was used.
            if (cmdvar_Help)
                return;

            // Set the path variables if required
            if(cmdvar_SetPathToInstalls)
                ModManager.PathInstalls = cmdvar_PathToInstalls;
            if(cmdvar_SetPathToMods)
                ModManager.PathMods = cmdvar_PathToMods;
            if(cmdvar_SetPathToProfiles)
                ModManager.PathProfiles = cmdvar_PathToProfiles;

            // Execute list commands if required
            if (cmdvar_ListInstalls)
                ModManager.ListInstalls();
            if (cmdvar_ListMods)
                ModManager.ListMods();
            if (cmdvar_ListProfiles)
                ModManager.ListProfiles();

            // Apply profile if required
            if (cmdvar_ApplyProfile)
                ModManager.ApplyProfile(cmdvar_Profile);
        }
    }
}
