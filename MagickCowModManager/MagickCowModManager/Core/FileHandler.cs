using MagickCowModManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public class FileHandler
    {
        public FileHandlingMode Mode { get; set; }

        public FileHandler()
        {
            this.Mode = FileHandlingMode.Copy;
        }

        public void EnableModFile(string origin, string destination)
        {
            switch (this.Mode)
            {
                default:
                case FileHandlingMode.Copy:
                    EnableModFile_Copy(origin, destination);
                    break;
                case FileHandlingMode.SymbolicLink:
                    EnableModFile_SymbolicLink(origin, destination);
                    break;
                case FileHandlingMode.Hardlink:
                    EnableModFile_HardLink(origin, destination);
                    break;
            }
        }

        public void DisableModFile()
        {
            // TODO : Implement
        }

        private void EnableModFile_Copy(string org, string dst)
        {
            File.Copy(org, dst);
        }

        private void EnableModFile_SymbolicLink(string org, string dst)
        {
            File.CreateSymbolicLink(dst, org);
        }

        private void EnableModFile_HardLink(string org, string dst)
        {
            // TODO : Find a way to implement this in both windows and linux.
            // For windows I already know what DLL and function to load, just need to find the same for linux.
            // Also add a "not supported" message / exception for any other system.
            throw new NotImplementedException("Hard Link usage is not implemented yet!");
        }
    }
}
