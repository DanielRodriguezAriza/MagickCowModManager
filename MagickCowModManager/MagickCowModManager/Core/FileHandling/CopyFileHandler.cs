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

        public CopyFileHandler(Profile profile, string modsContentPath, string gameContentPath)
        {
            this.profile = profile;
            this.modsContentPath = modsContentPath;
            this.gameContentPath = gameContentPath;
        }

        public void InstallProfile()
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

        private void ProcessDirectory(string origin, string destination)
        {
            DirectoryInfo originInfo = new DirectoryInfo(origin);
            DirectoryInfo destinationInfo = new DirectoryInfo(destination);
            ProcessDirectory(originInfo, destinationInfo);
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

        private void ProcessFile(FileInfo fileInfo, DirectoryInfo destination)
        {
            // TODO : Add permission handling in the future or what?
            // File.CreateSymbolicLink(Path.Combine(destination.FullName, fileInfo.Name), fileInfo.FullName);

            bool shouldDeleteFile = false;
            bool shouldCreateFile = false;

            string destinationFileName = Path.Combine(destination.FullName, fileInfo.Name);

            if (File.Exists(destinationFileName))
            {
                if (!FileContentsAreEqual(fileInfo.FullName, destinationFileName))
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
                File.Delete(destinationFileName);

            if (shouldCreateFile)
            {
                Console.WriteLine($"Installing mod file : {fileInfo.FullName}");
                File.Copy(fileInfo.FullName, destinationFileName); // Shitty, what about heavy files? don't want to copy those... fucking windows I swear...
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
