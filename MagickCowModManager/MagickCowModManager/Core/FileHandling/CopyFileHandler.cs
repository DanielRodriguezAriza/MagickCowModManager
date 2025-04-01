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

            foreach (var file in this.filesToInstall)
                Console.WriteLine(file);
            Console.ReadLine();

            GenerateData(originInfo, destinationInfo);
            CleanUpData(destinationInfo);
        }

        private void RegisterData(DirectoryInfo origin)
        {
            FileSystemHelper.GetChildPaths(origin, this.directoriesToInstall, this.filesToInstall);
        }

        private void GenerateData(DirectoryInfo origin, DirectoryInfo destination)
        {
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

                if (!File.Exists(fileToCreate) || FileSystemHelper.FileContentsAreEqual(fileToCopy, fileToCreate))
                {
                    File.Copy(fileToCopy, fileToCreate, true);
                }
            }
        }

        private void CleanUpData(DirectoryInfo destination)
        {
            CleanUpDataRec("", destination);
        }

        private void CleanUpDataRec(string parent, DirectoryInfo destination)
        {
            FileInfo[] files = destination.GetFiles();
            foreach (var file in files)
            {
                string path = Path.Combine(parent, file.Name);
                if (!this.filesToInstall.Contains(path))
                {
                    File.Delete(file.FullName);
                }
            }

            DirectoryInfo[] dirs = destination.GetDirectories();
            foreach (var dir in dirs)
            {
                string path = Path.Combine(parent, dir.Name);
                if (!this.directoriesToInstall.Contains(path))
                {
                    Directory.Delete(dir.FullName);
                }
                CleanUpData(dir);
            }
        }
    }
}
