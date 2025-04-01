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
            ProcessDirectory(originPath, destinationPath);
        }

        private void ProcessDirectory(string originPath, string destinationPath)
        {
            DirectoryInfo originInfo = new DirectoryInfo(originPath);
            DirectoryInfo destinationInfo = new DirectoryInfo(destinationPath);

            RegisterData(originInfo);
            GenerateData(originInfo, destinationInfo);
        }

        private void RegisterData(DirectoryInfo origin)
        {
            RegisterDirectoryRec("", origin);
        }

        private void RegisterDirectoryRec(string parent, DirectoryInfo origin)
        {
            FileInfo[] files = origin.GetFiles();
            foreach (var file in files)
            {
                var path = Path.Combine(parent, origin.Name);
                this.filesToInstall.Add(path);
            }

            DirectoryInfo[] dirs = origin.GetDirectories();
            foreach (var dir in dirs)
            {
                var path = Path.Combine(parent, dir.Name);
                this.directoriesToInstall.Add(path);
                RegisterDirectoryRec(path, dir);
            }
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

                if (!File.Exists(fileToCreate) || FileContentsAreEqual(fileToCopy, fileToCreate))
                {
                    File.Copy(fileToCopy, fileToCreate, true);
                }
            }
        }

        private void ProcessDirectory(DirectoryInfo origin, DirectoryInfo destination)
        {
            // Copy files from origin to destination
            FileInfo[] childFilesOrigin = origin.GetFiles();
            foreach (var file in childFilesOrigin)
            {
                ProcessFile(file, destination);
            }

            // Clean up files that are not in the origin directory
            // THIS IS ACTUALLY WRONG BECAUSE WE ONLY CHECK THE CURRENT MOD DIR, WHAT ABOUT ALL THE OTHER DIRS?? WE DELETE THE WORK WE JUST DID WTF!!!
            string[] childFileNamesOrigin = origin.GetFiles().Select(file => file.Name).ToArray();
            string[] childFileNamesDestination = destination.GetFiles().Select(file => file.Name).ToArray();
            foreach (var fileName in childFileNamesDestination)
            {
                if (!childFileNamesOrigin.Contains(fileName))
                {
                    File.Delete(Path.Combine(destination.FullName, fileName));
                }
            }

            // Copy directories from origin to destination
            DirectoryInfo[] childDirsOrigin = origin.GetDirectories();
            foreach (var dir in childDirsOrigin)
            {
                var newDir = destination.CreateSubdirectory(dir.Name);
                ProcessDirectory(dir, newDir);
            }
        }

        private void ProcessFile(FileInfo originFileInfo, DirectoryInfo destinationDirectoryInfo)
        {
            bool shouldDeleteFile = false;
            bool shouldCreateFile = false;

            string originFileName = originFileInfo.FullName;
            string destinationFileName = Path.Combine(destinationDirectoryInfo.FullName, originFileInfo.Name);

            if (File.Exists(destinationFileName))
            {
                if (!FileContentsAreEqual(originFileName, destinationFileName))
                {
                    shouldCreateFile = true;
                    shouldDeleteFile = true;
                }
            }
            else
            {
                shouldCreateFile = true;
            }

            if (shouldDeleteFile)
            {
                // If an outdated file exists on the destination, delete it.
                File.Delete(destinationFileName);
            }

            if (shouldCreateFile)
            {
                // If the destination doesn't already have installed the origin file, then copy it.
                Console.WriteLine($"Installing mod file : {originFileName}");
                File.Copy(originFileName, destinationFileName); // NOTE : Shitty solution, what about heavy files? don't want to copy those... fucking windows I swear...
            }
        }

        private bool FileContentsAreEqual(string filePathA, string filePathB)
        {
            bool ans = false;

            using (var fileA = File.Open(filePathA, FileMode.Open, FileAccess.Read))
            using (var fileB = File.Open(filePathB, FileMode.Open, FileAccess.Read))
            using (var readerA = new BinaryReader(fileA))
            using (var readerB = new BinaryReader(fileB))
            {
                long lengthA = readerA.BaseStream.Length;
                long lengthB = readerB.BaseStream.Length;

                if (lengthA == lengthB)
                {
                    var bytesA = readerA.ReadBytes((int)lengthA);
                    var bytesB = readerB.ReadBytes((int)lengthB);
                    if (bytesA.AsSpan().SequenceEqual(bytesB))
                        ans = true; // Supposedly in C# this is almost as fast as calling memcmp in C, so yeah, probably can't get faster than this without calling the CRT directly.

                    // Btw, yes, I know, i could just say ans = bytesA.AsSpan().SequenceEquals(bytesB).... But not really... because that fails in windows 11
                    // for some fucking stupid reason! So the "if(...) ans = true;" is a fucking hack that I need to use to work on that piece of trash OS.
                    // Why? who knows! But hopefully they'll fix that in a future update!
                    // "When?" you may ask! Who the fuck knows!
                }
            }

            return ans;
        }
    }
}
