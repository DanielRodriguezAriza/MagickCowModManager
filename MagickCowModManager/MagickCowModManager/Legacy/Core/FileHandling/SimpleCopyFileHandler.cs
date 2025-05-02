using MagickCowModManager.Legacy.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Legacy.Core.FileHandling
{
    // NOTE : Reverse load order implementation:
    // 
    // File / Asset overriding is now implemented through reverse copy.
    // The copy is performed in reverse order, by copying / symlinking first the data of the last mod in the load order.
    // Then, when the previous mods are loaded, if the file is already present, it is simply ignored.

    public class SimpleCopyFileHandler
    {
        public SimpleCopyFileHandler()
        { }

        public void InstallMods(Profile profile, string modsContentPath, string gameContentPath, FileHandlingMode handlingMode)
        {
            DirectoryInfo origin = new DirectoryInfo(modsContentPath);
            DirectoryInfo destination = new DirectoryInfo(gameContentPath);
            Console.WriteLine($"origin path : {origin.FullName}");
            Console.WriteLine($"destination path : {destination.FullName}");

            string destinationName = destination.FullName;
            destination.Delete(true);

            Directory.CreateDirectory(destinationName);

            for (int i = profile.EnabledMods.Count - 1; i >= 0; --i)
            {
                var modName = profile.EnabledMods[i];
                Console.WriteLine($"Installing Mod \"{modName}\"...");

                string modPath = Path.Combine(modsContentPath, modName, "Content");
                DirectoryInfo originInfo = new DirectoryInfo(modPath);

                ProcessDirectoryRec(originInfo, destination);
            }
        }

        private void ProcessDirectoryRec(DirectoryInfo origin, DirectoryInfo destination)
        {
            foreach (var file in origin.GetFiles())
            {
                string targetPath = Path.Combine(destination.FullName, file.Name);
                if (!File.Exists(targetPath))
                {
                    // Console.WriteLine("Creating file : " + targetPath);
                    // File.Copy(file.FullName, targetPath);
                    File.CreateSymbolicLink(targetPath, file.FullName);
                    // FileHandler.CopyFile(file.FullName, targetPath); // TODO : Change the code to use this line instead. Still need to check for correctness when using hard links and stuff like that, so yeah. And ofc, think of the performance impact of switching every single time we call this function... we may need to rework that so that we pick the function BEFORE starting the process, maybe with an Action or something, idk... for now, just use symlink always and that's it.
                }
                else
                {
                    // File.Delete(targetPath);
                    // File.CreateSymbolicLink(targetPath, file.FullName);
                }
            }

            foreach (var dir in origin.GetDirectories())
            {
                string targetPath = Path.Combine(destination.FullName, dir.Name);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                ProcessDirectoryRec(dir, new DirectoryInfo(targetPath));
            }
        }
    }
}
