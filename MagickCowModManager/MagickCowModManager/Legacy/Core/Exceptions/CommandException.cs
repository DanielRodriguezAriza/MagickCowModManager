using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Legacy.Core.Exceptions
{
    public class CommandException : MagickCowModManagerException
    {
        public CommandException() : base()
        { }

        public CommandException(string? message) : base(message)
        { }

        public CommandException(string? message, Exception innerException) : base(message, innerException)
        { }
    }
}
