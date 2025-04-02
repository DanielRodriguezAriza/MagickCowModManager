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
        private string modsContentPath;
        private string gameContentPath;
        private Profile profile;

        public SimpleCopyFileHandler(Profile profile, string modsContentPath, string gameContentPath)
        {
            this.profile = profile;
            this.modsContentPath = modsContentPath;
            this.gameContentPath = gameContentPath;
        }

        public void InstallMods()
        {
            DirectoryInfo origin = new DirectoryInfo(this.modsContentPath);
            DirectoryInfo destination = new DirectoryInfo(this.gameContentPath);
            Console.WriteLine($"origin path : {origin.FullName}");
            Console.WriteLine($"destination path : {destination.FullName}");

            string destinationName = destination.FullName;
            destination.Delete(true);

            Directory.CreateDirectory(destinationName);

            foreach (var modName in this.profile.EnabledMods)
            {
                // Install Mod
                Console.WriteLine($"Installing Mod \"{modName}\"...");

                string modPath = Path.Combine(this.modsContentPath, modName, "Content");
                DirectoryInfo originInfo = new DirectoryInfo(modPath);

                ProcessDirectoryRec(originInfo, destination);
            }
        }

        public void ProcessDirectoryRec(DirectoryInfo origin, DirectoryInfo destination)
        {
            foreach (var file in origin.GetFiles())
            {
                string targetPath = Path.Combine(destination.FullName, file.Name);
                if (!File.Exists(targetPath))
                {
                    // Console.WriteLine("Creating file : " + targetPath);
                    File.Copy(file.FullName, targetPath);
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
