using MagickCowModManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.FileHandling
{
    // File Handler that uses Copy operations to handle file creation
    // NOTE : For future updates: it's probably a bit faster to perform the file verification on the same step as the copy step rather than doing it in 2 different loops. But for now, this is ok. This would mean that possibly, a speedup could exist if we merged the generate and register stages together. Obviously it would be super fast if we could merge cleanup as well, which would be trivial if we had a single source / origin, but we're merging multiple source directories into a single target, which requires a lot more diffing work... all in all, check this idea out in the future when you have time.
    // NOTE : When making the simple raw copy operator / handler, what you should do instead of all of this bullshit, is simply copying in reverse load order, simple as that. Delete everything from the Content folder, and then load the mods in reverse order from how they appear in the load order list. When a duplicate entry is found simply by File.Exists() with name, then don't copy it. That way, you already have the latest possible file, the last one in the load order. Kinda like the base JKA load trick which openJKA got rid of for some reason... (or maybe I'm misremembering how this worked in JKA, but whatever...)
    public class CopyFileHandler
    {
        private string modsContentPath;
        private string gameContentPath;
        private Profile profile;

        private List<string> directoriesToInstall;
        private List<string> filesToInstall;

        public CopyFileHandler(Profile profile, string modsContentPath, string gameContentPath)
        {
            this.profile = profile;
            this.modsContentPath = modsContentPath;
            this.gameContentPath = gameContentPath;
            
            this.directoriesToInstall = new List<string>();
            this.filesToInstall = new List<string>();
        }

        public void InstallMods()
        {
            DirectoryInfo destinationInfo = new DirectoryInfo(this.gameContentPath);

            foreach (var modName in this.profile.EnabledMods)
            {
                // Install Mod
                Console.WriteLine($"Installing Mod \"{modName}\"...");

                string modPath = Path.Combine(this.modsContentPath, modName, "Content");
                DirectoryInfo originInfo = new DirectoryInfo(modPath);

                RegisterData(originInfo);
                GenerateData(originInfo, destinationInfo);
            }

            // Clean Up Mods after installing all of them to remove old files that are now unused to prevent loading unwanted / unloaded mods
            Console.WriteLine("Cleaning Up Mod Data...");
            CleanUpData(destinationInfo);
        }

        private void RegisterData(DirectoryInfo origin)
        {
            // Console.WriteLine("Registering Data...");
            FileSystemHelper.GetChildPaths(origin, this.directoriesToInstall, this.filesToInstall);
        }

        private void GenerateData(DirectoryInfo origin, DirectoryInfo destination)
        {
            // Console.WriteLine("Generating Data...");

            foreach (var dir in this.directoriesToInstall)
            {
                string dirToCreate = Path.Combine(destination.FullName, dir);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dirToCreate);
                }
            }

            foreach (var file in this.filesToInstall)
            {
                string fileToCopy = Path.Combine(origin.FullName, file);
                string fileToCreate = Path.Combine(destination.FullName, file);

                // Console.WriteLine($"The file we are going to copy is   : {fileToCopy}");
                // Console.WriteLine($"The file we are going to create is : {fileToCreate}");
                // Console.ReadLine();

                if (File.Exists(fileToCopy) && (!File.Exists(fileToCreate) || !FileSystemHelper.FileContentsAreEqual(fileToCopy, fileToCreate)))
                {
                    Console.WriteLine($"Creating File : {fileToCreate}");
                    File.Copy(fileToCopy, fileToCreate, true);
                }
                else
                {
                    // Do nothing because the file is already there!
                    // Console.WriteLine("The file already exists!");
                }
            }
        }

        private void CleanUpData(DirectoryInfo destination)
        {
            // Console.WriteLine("Cleaning Up Data...");

            List<string> dirsToRemove = new List<string>();
            List<string> filesToRemove = new List<string>();
            FileSystemHelper.GetChildPathsDiff(destination, this.directoriesToInstall, this.filesToInstall, dirsToRemove, filesToRemove);

            foreach (var file in filesToRemove)
            {
                string path = Path.Combine(destination.FullName, file);
                Console.WriteLine($"Deleting File : {path}");
                File.Delete(path);
            }

            foreach (var dir in dirsToRemove)
            {
                string path = Path.Combine(destination.FullName, dir);
                Console.WriteLine($"Deleting Directory : {path}");
                Directory.Delete(path);
            }
        }
    }
}
