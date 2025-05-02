using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Legacy.Core.Exceptions
{
    public class ParseException : MagickCowModManagerException
    {
        public ParseException() : base()
        { }

        public ParseException(string? message) : base(message)
        { }

        public ParseException(string? message, Exception innerException) : base(message, innerException)
        { }
    }
}
