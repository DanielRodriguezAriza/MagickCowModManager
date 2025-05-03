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

        // Function to "copy" a file from source to destination.
        // Assume usage of hardlinks by default.
        // TODO : Improve these shitty comments and clean up the code...
        public static void CopyFile(string source, string destination, FileHandlingMode fileHandlingMode = FileHandlingMode.HardLink)
        {
            CopyFileInternal(source, destination, fileHandlingMode);
        }

        public static void CopyFileInternal(string source, string destination, FileHandlingMode fileHandlingMode)
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

        // NOTE : This makes me cry blood... runtime checks to see what OS we're on... why couldn't this shit be done at compiletime on C#??
        // Oh how I miss macros and conditional compilation from C...
        private static void CopyFileInternal_HardLink(string source, string destination)
        {
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
}
