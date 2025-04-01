namespace MagickCowModManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("No se ha hecho nada...");
                return;
            }
            File.CreateSymbolicLink(args[0], args[1]);
            Console.WriteLine("Se ha creado el symlink.");
        }
    }
}
