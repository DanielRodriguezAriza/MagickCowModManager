using MagickCowModManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.FileHandling
{
    // TODO : Use this class as the file copying and handling backend. Also, make sure to rename the other "file handler" classes to something that makes more sense... they are part of the mod manager after all, maybe mod loader or something could be a good name?
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
            // TODO : Find a way to implement this in both windows and linux.
            // For windows I already know what DLL and function to load, just need to find the same for linux.
            // Also add a "not supported" message / exception for any other system.
            throw new NotImplementedException("Hard Link usage is not implemented yet!");
        }
    }
}
