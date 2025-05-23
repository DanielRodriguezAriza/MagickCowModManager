using MagickCowModManager.Core;
using MagickCowModManager.Core.Args;
using MagickCowModManager.Core.IO;

namespace MagickCowModManager
{
    public class Program
    {
        static int Main(string[] args)
        {
            try
            {
                ModManager modManager = new ModManager();
                CommandHandler commandHandler = new CommandHandler(modManager);
                ArgParser argParser = new ArgParser(commandHandler);

                argParser.ParseAndExecute(args);

                Logger.Log(0, "Successfully completed the operation!");

                return 0;
            }
            catch(Exception e)
            {
                Logger.Log(1, $"An error orcurred while running the program : {e.Message}", null);

                return 1;
            }
        }
    }
}
