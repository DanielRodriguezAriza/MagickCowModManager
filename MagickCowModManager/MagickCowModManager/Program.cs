using MagickCowModManager.Core;

namespace MagickCowModManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
            if (args.Length != 2)
            {
                Console.WriteLine("No se ha hecho nada...");
                return;
            }
            File.CreateSymbolicLink(args[0], args[1]);
            Console.WriteLine("Se ha creado el symlink.");
            */

            if (args.Length != 2)
            {
                Console.WriteLine("Usage : TODO : Implement usage / help message");
                return;
            }

            ModManager modManager = new ModManager(args[0]);
            modManager.ApplyProfile(args[1]);
        }
    }
}
