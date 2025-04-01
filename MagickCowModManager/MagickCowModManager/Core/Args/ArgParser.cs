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
            if (args.Length == 0)
            {
                this.commandHandler.CmdHelp(args, 0);
                return;
            }

            for (int i = 0; i < args.Length; ++i)
            {
                int argsRemaining = args.Length - i - 1;
                var arg = args[i];

                foreach (var cmd in this.commandHandler.Commands)
                {
                    if ($"-{cmd.ShortCommand}" == arg || $"--{cmd.LongCommand}" == arg)
                    {
                        if (argsRemaining < cmd.Arguments.Length)
                        {
                            throw new Exception($"Not enough arguments found : {cmd.Arguments.Length} were expected, but {argsRemaining} were found!");
                        }
                        cmd.Function(args, i);
                        i += cmd.Arguments.Length; // Consume the arguments as well.
                        return;
                    }
                }

                throw new Exception($"Unknown argument found : \"{arg}\"");
            }
        }
    }
}
