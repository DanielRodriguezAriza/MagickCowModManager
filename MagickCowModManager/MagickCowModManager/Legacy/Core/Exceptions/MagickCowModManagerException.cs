using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Legacy.Core.Exceptions
{
    // Base class for all other types of exceptions
    public class MagickCowModManagerException : Exception
    {
        public MagickCowModManagerException() : base()
        { }

        public MagickCowModManagerException(string? message) : base(message)
        { }

        public MagickCowModManagerException(string? message, Exception innerException) : base(message, innerException)
        { }
    }
}
