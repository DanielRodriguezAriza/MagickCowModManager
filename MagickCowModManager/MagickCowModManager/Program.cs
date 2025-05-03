using MagickCowModManager.Core;

namespace MagickCowModManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
            ModManager modManager = new ModManager();

            Console.WriteLine("Installs:");
            foreach (var x in FileSystemUtil.GetChildDirectoriesName(modManager.PathInstalls)) Console.WriteLine($"    - {x}");

            Console.WriteLine("Mods:");
            foreach (var x in FileSystemUtil.GetChildDirectoriesName(modManager.PathMods)) Console.WriteLine($"    - {x}");

            Console.WriteLine("Profiles:");
            foreach (var x in FileSystemUtil.GetChildDirectoriesName(modManager.PathProfiles)) Console.WriteLine($"    - {x}");
            */

            try
            {
                ModManager modManager = new ModManager();
                modManager.ApplyProfile(args[0]);
                Logger.Log(0, "Successfully completed the operation!");
            }
            catch
            {
                Logger.Log(1, "An error orcurred while running the program!");
            }

            /*if (args.Length == 2)
            {
                FileSystemUtil.CopyFile(args[0], args[1]);
                Console.WriteLine("Created hard link");
            }
            else
            {
                Console.WriteLine("Wrong number of args");
            }*/
        }
    }
}
