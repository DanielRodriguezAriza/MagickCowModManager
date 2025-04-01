using MagickCowModManager.Core;
using MagickCowModManager.Core.Exceptions;

namespace MagickCowModManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ModManager modManager = new ModManager();
                modManager.ParseArguments(args);
            }
            catch (LoadException exception)
            {
                Console.WriteLine($"Load Error : {exception.Message}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
                Console.WriteLine($"Stack Trace : {exception.StackTrace}");
            }
        }
    }
}
