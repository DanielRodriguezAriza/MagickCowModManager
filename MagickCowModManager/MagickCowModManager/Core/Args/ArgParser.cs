using MagickCowModManager.Legacy.Core.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Args
{
    public class ArgParser
    {
        public CommandHandler CommandHandler { get; set; } // Refernece to command handler instance

        public ArgParser(CommandHandler commandHandler)
        {
            this.CommandHandler = commandHandler;
        }

        public void Parse(string[] args)
        {
            if (args.Length == 0)
            {
                CommandHandler.CmdHelp(args, 0);
                return;
            }

            for (int i = 0; i < args.Length; ++i)
            {
                int argsRemaining = args.Length - i - 1;
                var arg = args[i];
                bool commandFound = false;

                foreach (var cmd in CommandHandler.Commands)
                {
                    if ($"-{cmd.ShortCommand}" == arg || $"--{cmd.LongCommand}" == arg)
                    {
                        commandFound = true;
                        if (argsRemaining < cmd.Arguments.Length)
                        {
                            throw new Exception($"Not enough arguments found : {cmd.Arguments.Length} were expected, but {argsRemaining} were found!");
                        }
                        cmd.Function(args, i);
                        i += cmd.Arguments.Length; // Consume the arguments as well.
                        break;
                    }
                }

                if (!commandFound)
                {
                    throw new Exception($"Unknown argument found : \"{arg}\"");
                }
            }
        }

        public void Execute()
        {
            CommandHandler.Execute(); // Execute the actions after all of the commands have been registered
        }

        public void ParseAndExecute(string[] args)
        {
            Parse(args);
            Execute();
        }
    }
}
