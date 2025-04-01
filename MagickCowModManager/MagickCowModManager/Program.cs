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
            catch (ParseException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (CommandException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Unhandled Exception Found!");
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
