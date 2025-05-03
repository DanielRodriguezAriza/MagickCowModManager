using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public static class FileSystemUtil
    {
        public static List<string> GetChildDirectories(string path)
        {
            return GetChildDirectories(new DirectoryInfo(path));
        }

        public static List<string> GetChildDirectories(DirectoryInfo directoryInfo)
        {
            var directories = directoryInfo.GetDirectories();
            List<string> result = new List<string>(directories.Length);
            foreach (var dir in directories)
            {
                result.Add(dir.Name);
            }
            return result;
        }

        public static List<string> GetChildFiles(string path)
        {
            return GetChildFiles(new DirectoryInfo(path));
        }

        public static List<string> GetChildFiles(DirectoryInfo directoryInfo)
        {
            var files = directoryInfo.GetFiles();
            List<string> result = new List<string>(files.Length);
            foreach (var file in files)
            {
                result.Add(file.Name);
            }
            return result;
        }
    }
}
