using MagickCowModManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.FileHandling
{
    // TODO : Use this class as the file copying and handling backend. Also, make sure to rename the other "file handler" classes to something that makes more sense... they are part of the mod manager after all, maybe mod loader or something could be a good name?
    // NOTE : Maybe, in linux, automatically use symlink. On windows, default to hard link if the hard drives are the same, and then use symlink if they are not the same... could use some automatic detection shit or whatever... or just make it a manual setting...
    public static class FileHandler
    {
        public static void CopyFile(string origin, string destination, FileHandlingMode mode = FileHandlingMode.SymbolicLink)
        {
            CopyFileInternal(origin, destination, mode);
        }

        public static void CopyFile(FileInfo origin, FileInfo destination, FileHandlingMode mode = FileHandlingMode.SymbolicLink)
        {
            CopyFileInternal(origin.FullName, destination.FullName, mode);
        }

        private static void CopyFileInternal(string origin, string destination, FileHandlingMode mode)
        {
            switch (mode)
            {
                default:
                case FileHandlingMode.Copy:
                    CopyFileInternal_Copy(origin, destination);
                    break;
                case FileHandlingMode.SymbolicLink:
                    CopyFileInternal_SymbolicLink(origin, destination);
                    break;
                case FileHandlingMode.Hardlink:
                    CopyFileInternal_HardLink(origin, destination);
                    break;
            }
        }

        private static void CopyFileInternal_Copy(string origin, string destination)
        {
            File.Copy(origin, destination);
        }

        private static void CopyFileInternal_SymbolicLink(string origin, string destination)
        {
            File.CreateSymbolicLink(destination, origin);
        }

        // NOTE : This makes me cry blood... runtime checks to see what OS we're on... why couldn't this shit be done at compiletime on C#??
        // Oh how I miss macros and conditional compilation from C...
        private static void CopyFileInternal_HardLink(string origin, string destination)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Win32Helper.CreateFileAsHardLink(origin, destination);
            }
            else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                UnixHelper.CreateFileAsHardLink(origin, destination);
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
            private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

            [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern int FormatMessage(int flags, IntPtr source, int messageId, int languageId, StringBuilder buffer, int size, IntPtr arguments);

            private static string GetErrorMessage(int errorCode)
            {
                StringBuilder buffer = new StringBuilder(256);
                FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, errorCode, 0, buffer, buffer.Capacity, IntPtr.Zero);
                return buffer.ToString().Trim();
            }

            public static void CreateFileAsHardLink(string origin, string destination)
            {
                bool success = CreateHardLink(destination, origin, IntPtr.Zero);
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
            private static extern IntPtr strerror(int errnum);

            private static string GetErrorMessage(int errorCode)
            {
                IntPtr messagePointer = strerror(errorCode);
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
