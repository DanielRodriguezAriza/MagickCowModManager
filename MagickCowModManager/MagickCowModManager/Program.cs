namespace MagickCowModManager
{
    public class FileHandler
    {
        public enum OperationMode
        {
            Copy,
            SymbolicLink,
            Hardlink,
        }

        public OperationMode Mode { get; set; }

        public FileHandler()
        {
            this.Mode = OperationMode.Copy;
        }

        public void EnableModFile(string origin, string destination)
        {
            switch (this.Mode)
            {
                default:
                case OperationMode.Copy:
                    EnableModFile_Copy(origin, destination);
                    break;
                case OperationMode.SymbolicLink:
                    EnableModFile_SymbolicLink(origin, destination);
                    break;
                case OperationMode.Hardlink:
                    EnableModFile_HardLink(origin, destination);
                    break;
            }
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

    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("No se ha hecho nada...");
                return;
            }
            File.CreateSymbolicLink(args[0], args[1]);
            Console.WriteLine("Se ha creado el symlink.");
        }
    }
}
