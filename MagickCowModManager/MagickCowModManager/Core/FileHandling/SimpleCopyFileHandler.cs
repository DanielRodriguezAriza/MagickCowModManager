using MagickCowModManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.FileHandling
{
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

            for(int i = profile.EnabledMods.Count - 1; i >= 0; --i)
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
