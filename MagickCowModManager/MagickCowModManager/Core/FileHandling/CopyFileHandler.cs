using MagickCowModManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.FileHandling
{
    // File Handler that uses Copy operations to handle file creation
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
            foreach (var modName in this.profile.EnabledMods)
            {
                InstallMod(modName);
            }
        }

        private void InstallMod(string modName)
        {
            string originPath = Path.Combine(this.modsContentPath, modName, "Content");
            string destinationPath = this.gameContentPath;

            DirectoryInfo originInfo = new DirectoryInfo(originPath);
            DirectoryInfo destinationInfo = new DirectoryInfo(destinationPath);

            RegisterData(originInfo);

            /*
            foreach (var file in this.filesToInstall)
                Console.WriteLine(file);
            Console.ReadLine();
            */

            GenerateData(originInfo, destinationInfo);
            CleanUpData(destinationInfo);
        }

        private void RegisterData(DirectoryInfo origin)
        {
            Console.WriteLine("Registering Data...");
            FileSystemHelper.GetChildPaths(origin, this.directoriesToInstall, this.filesToInstall);
        }

        private void GenerateData(DirectoryInfo origin, DirectoryInfo destination)
        {
            Console.WriteLine("Generating Data...");

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

                if (!File.Exists(fileToCreate) || !FileSystemHelper.FileContentsAreEqual(fileToCopy, fileToCreate))
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
            Console.WriteLine("Cleaning Up Data...");

            List<string> dirsToRemove = new List<string>();
            List<string> filesToRemove = new List<string>();
            FileSystemHelper.GetChildPathsDiff(destination, this.directoriesToInstall, this.filesToInstall, dirsToRemove, filesToRemove);

            foreach (var file in filesToRemove)
            {
                Console.WriteLine($"Deleting File : {file}");
                File.Delete(file);
            }

            foreach (var dir in dirsToRemove)
            {
                // Console.WriteLine(dir);
                Directory.Delete(dir);
            }
        }
    }
}
