using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public static class FileSystemUtil
    {
        #region DirInfoOps

        private static class DirInfoOps
        {
            public static List<DirectoryInfo> GetChildDirectories(DirectoryInfo directoryInfo)
            {
                var ans = new List<DirectoryInfo>(directoryInfo.GetDirectories());
                return ans;
            }
        }

        public static List<DirectoryInfo> GetChildDirectoriesInfo(string path)
        {
            return GetChildDirectoriesInfo(new DirectoryInfo(path));
        }

        public static List<DirectoryInfo> GetChildDirectoriesInfo(DirectoryInfo directoryInfo)
        {
            return DirInfoOps.GetChildDirectories(directoryInfo);
        }

        public static List<string> GetChildDirectoriesName(string path)
        {
            return GetChildDirectoriesName(new DirectoryInfo(path));
        }

        public static List<string> GetChildDirectoriesName(DirectoryInfo directoryInfo)
        {
            var directories = GetChildDirectoriesInfo(directoryInfo);
            List<string> result = new List<string>(directories.Count);
            foreach (var dir in directories)
            {
                result.Add(dir.Name);
            }
            return result;
        }

        #endregion

        #region FileInfoOps

        public static class FileInfoOps
        {
            public static List<FileInfo> GetChildFiles(DirectoryInfo directoryInfo)
            {
                var ans = new List<FileInfo>(directoryInfo.GetFiles());
                return ans;
            }
        }

        public static List<FileInfo> GetChildFilesInfo(string path)
        {
            return GetChildFilesInfo(new DirectoryInfo(path));
        }

        public static List<FileInfo> GetChildFilesInfo(DirectoryInfo directoryInfo)
        {
            return FileInfoOps.GetChildFiles(directoryInfo);
        }

        public static List<string> GetChildFilesName(string path)
        {
            return GetChildFilesName(new DirectoryInfo(path));
        }

        public static List<string> GetChildFilesName(DirectoryInfo directoryInfo)
        {
            var files = GetChildFilesInfo(directoryInfo);
            List<string> result = new List<string>(files.Count);
            foreach (var file in files)
            {
                result.Add(file.Name);
            }
            return result;
        }

        #endregion

        #region FileOps

        public static void CopyFile(string source, string destination, FileHandlingMode fileHandlingMode = FileHandlingMode.HardLink)
        {
            CopyFile(new FileInfo(source), new FileInfo(destination), fileHandlingMode);
        }

        public static void CopyFile(FileInfo source, FileInfo destination, FileHandlingMode fileHnadlingMode = FileHandlingMode.HardLink)
        {
            FileOps.CopyFile(source.FullName, destination.FullName, fileHnadlingMode);
        }

        public static void CopyDirectory(string source, string destination, FileHandlingMode fileHandlingMode = FileHandlingMode.HardLink)
        {
            DirectoryInfo directoryInfoSrc = new DirectoryInfo(source);
            DirectoryInfo directoryInfoDst = new DirectoryInfo(destination);
            CopyDirectory(directoryInfoSrc, directoryInfoDst, fileHandlingMode);
        }

        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination, FileHandlingMode fileHandlingMode = FileHandlingMode.HardLink)
        {
            var childDirs = source.GetDirectories();
            foreach (var child in childDirs)
            {
                string srcPath = child.FullName;
                string dstPath = Path.Combine(destination.FullName, child.Name);

                if (!child.Exists)
                {
                    Directory.CreateDirectory(dstPath);
                }

                CopyDirectory(srcPath, dstPath, fileHandlingMode); // Recursive call
            }

            var childFiles = source.GetFiles();
            foreach (var child in childFiles)
            {
                string srcPath = child.FullName;
                string dstPath = Path.Combine(destination.FullName, child.Name);

                if(!child.Exists)
                {
                    // If the file already exists, then we do not copy over it.
                    // This is important because of the way that the overriding is implemented, with the whole reverse iteration stuff, which takes advantage of this
                    // behaviour, reducing the number of calls to file system operations.
                    CopyFile(srcPath, dstPath, fileHandlingMode);
                }
            }
        }

        public static void DeleteDirectory(string path)
        {
            DeleteDirectory(new DirectoryInfo(path));
        }

        public static void DeleteDirectory(DirectoryInfo path)
        {
            if (!path.Exists)
            {
                return; // Can't delete a dir that does not exist, so we just bail out.
            }

            var childDirs = path.GetDirectories();
            foreach (var child in childDirs)
            {
                DeleteDirectory(child);
            }

            // The ".preserve" file indicates that the current directory is to be preserved
            bool shouldPreserve = File.Exists(Path.Combine(path.FullName, ".preserve"));
            if (shouldPreserve)
            {
                return;
            }

            // Directory.Delete() calls in C# only delete if the directory is empty, but just in case this
            // behaviour is not preserved in future updates, we add our own check first, just to be sure
            var childDirsLatest = path.GetDirectories();
            var childFilesLatest = path.GetFiles();
            bool anyChildrenLeft = childDirsLatest.Length > 0 || childFilesLatest.Length > 0;
            if (anyChildrenLeft)
            {
                // If any children are left on the directory, we can't delete it, because that means
                // that at least one of the children on this directory structure has a ".preserve" file present with it.
                return;
            }

            Directory.Delete(path.FullName);
        }

        private static class FileOps
        {
            public static void CopyFile(string source, string destination, FileHandlingMode fileHandlingMode = FileHandlingMode.HardLink)
            {
                CopyFileInternal(source, destination, fileHandlingMode);
            }

            private static void CopyFileInternal(string source, string destination, FileHandlingMode fileHandlingMode)
            {
                switch (fileHandlingMode)
                {
                    default:
                    case FileHandlingMode.Copy:
                        CopyFileInternal_Copy(source, destination);
                        break;
                    case FileHandlingMode.SymbolicLink:
                        CopyFileInternal_SymbolicLink(source, destination);
                        break;
                    case FileHandlingMode.HardLink:
                        CopyFileInternal_HardLink(source, destination);
                        break;
                }
            }

            private static void CopyFileInternal_Copy(string source, string destination)
            {
                File.Copy(source, destination);
            }

            private static void CopyFileInternal_SymbolicLink(string source, string destination)
            {
                File.CreateSymbolicLink(destination, source);
            }

            private static void CopyFileInternal_HardLink(string source, string destination)
            {
                // NOTE : This makes me cry blood... runtime checks to see what OS we're on... why couldn't this shit be done at compiletime on C#??
                // Oh how I miss macros and conditional compilation from C...
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Win32Helper.CreateFileAsHardLink(source, destination);
                }
                else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                {
                    UnixHelper.CreateFileAsHardLink(source, destination);
                }
                else
                {
                    throw new NotImplementedException("Hard Link usage is not implemented yet on the current platform!)");
                }
            }

            private static class Win32Helper
            {
                // Gotta love Windows and their undocumented magic numbers! took quite a while to find the definition of this macro...
                // And ofc, here on C# it has to be a fucking const int, like wtf...
                const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

                // Windows API for creating hard links.
                [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
                private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, nint lpSecurityAttributes);

                [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
                private static extern int FormatMessage(int flags, nint source, int messageId, int languageId, StringBuilder buffer, int size, nint arguments);

                private static string GetErrorMessage(int errorCode)
                {
                    StringBuilder buffer = new StringBuilder(256);
                    FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, nint.Zero, errorCode, 0, buffer, buffer.Capacity, nint.Zero);
                    return buffer.ToString().Trim();
                }

                public static void CreateFileAsHardLink(string origin, string destination)
                {
                    bool success = CreateHardLink(destination, origin, nint.Zero);
                    if (!success)
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        string message = GetErrorMessage(errorCode);
                        throw new Exception(message);
                    }
                }
            }

            private static class UnixHelper
            {
                // Linux API for creating hard links.
                // Also works for macOS, but I think I need to use hunlink() to remove directories and files that are hard linked... not sure about that tho...
                [DllImport("libc", SetLastError = true)]
                private static extern int link(string oldpath, string newpath);

                [DllImport("libc")]
                private static extern nint strerror(int errnum);

                private static string GetErrorMessage(int errorCode)
                {
                    nint messagePointer = strerror(errorCode);
                    return Marshal.PtrToStringAnsi(messagePointer) ?? "Unknown error";
                }

                public static void CreateFileAsHardLink(string origin, string destination)
                {
                    int errorCode = link(origin, destination);
                    if (errorCode != 0)
                    {
                        string message = GetErrorMessage(errorCode);
                        throw new Exception(message);
                    }
                }
            }
        }

        #endregion
    }
}
