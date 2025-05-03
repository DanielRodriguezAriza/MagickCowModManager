using MagickCowModManager.Core;

namespace MagickCowModManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            ModManager modManager = new ModManager();

            Console.WriteLine("Installs:");
            foreach (var x in FileSystemUtil.GetChildDirectories(modManager.PathInstalls)) Console.WriteLine($"    - {x}");

            Console.WriteLine("Mods:");
            foreach (var x in FileSystemUtil.GetChildDirectories(modManager.PathMods)) Console.WriteLine($"    - {x}");

            Console.WriteLine("Profiles:");
            foreach (var x in FileSystemUtil.GetChildDirectories(modManager.PathProfiles)) Console.WriteLine($"    - {x}");
        }
    }
}
