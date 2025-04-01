using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.FileHandling
{
    public static class FileSystemHelper
    {
        public static void GetChildPaths(string path, List<string> foundDirectories, List<string> foundFiles)
        {
            var pathInfo = new DirectoryInfo(path);
            GetChildPaths(pathInfo, foundDirectories, foundFiles);
        }

        public static void GetChildPaths(DirectoryInfo pathInfo, List<string> foundDirectories, List<string> foundFiles)
        {
            GetChildPathsRec("", pathInfo, foundDirectories, foundFiles);
        }

        private static void GetChildPathsRec(string parent, DirectoryInfo origin, List<string> foundDirs, List<string> foundFiles)
        {
            FileInfo[] files = origin.GetFiles();
            foreach (var file in files)
            {
                var path = Path.Combine(parent, file.Name);
                foundFiles.Add(path);
            }

            DirectoryInfo[] dirs = origin.GetDirectories();
            foreach (var dir in dirs)
            {
                var path = Path.Combine(parent, dir.Name);
                foundDirs.Add(path);
                GetChildPathsRec(path, dir, foundDirs, foundFiles);
            }
        }

        public static bool FileContentsAreEqual(string filePathA, string filePathB)
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
