using MagickCowModManager.Core.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    // This class is in charge of controlling the top-level logic of the mod manager program for the command-line interface.
    // Basically, this is the top-level class of the mcow-mm cli program

    public class ModManagerProgram
    {
        private ModManager modManager;
        private CommandHandler commandHandler;
        private ArgParser argParser;

        public ModManagerProgram()
        {
            // Initialize the members of this mod manager program.
            modManager = new ModManager();
            commandHandler = new CommandHandler(modManager);
            argParser = new ArgParser(commandHandler);
        }

        public void Run(string[] args)
        {
            // Execute the program and parse through all of the args to change the instructions and functionality that will be executed.
            argParser.ParseAndExecute(args);
        }
    }
}
