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

        private static void CopyFileInternal_HardLink(string origin, string destination)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                bool success = CreateHardLink(destination, origin, IntPtr.Zero);
                if (!success)
                {
                    throw new Exception("Failed to create hard link on Windows");
                }
            }
            else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                bool success = link(origin, destination) == 0;
                if (!success)
                {
                    throw new Exception("Failed to create hard link on Unix-like system");
                }
            }
            else
            {
                throw new NotImplementedException("Hard Link usage is not implemented yet!");
            }
        }

        // Windows API for creating hard links.
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        // Linux API for creating hard links.
        // Also works for macOS, but I think I need to use hunlink() to remove directories and files that are hard linked... not sure about that tho...
        [DllImport("libc", SetLastError = true)]
        private static extern int link(string oldpath, string newpath);
    }
}
