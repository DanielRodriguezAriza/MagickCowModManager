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
                ModManagerProgram modManager = new ModManagerProgram();
                modManager.Run(args);

                // Logger.Log(0, "Successfully completed the operation!");

                return 0;
            }
            catch(Exception e)
            {
                // Logger.Log(1, $"An error orcurred while running the program : {e.Message}", null);

                Console.WriteLine($"An error ocurred while running the program : \"{e.Message}\"");
                Console.WriteLine($"Stack trace : {e.StackTrace}");

                return 1;
            }
        }
    }
}
