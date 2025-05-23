using MagickCowModManager.Core;
using MagickCowModManager.Core.IO;

namespace MagickCowModManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ModManager modManager = new ModManager();

                modManager.ListInstalls();
                modManager.ListMods();
                modManager.ListProfiles();

                modManager.ApplyProfile(args[0]);
                Logger.Log(0, "Successfully completed the operation!");
            }
            catch(Exception e)
            {
                Logger.Log(1, $"An error orcurred while running the program : {e.Message}", null);
            }
        }
    }
}
