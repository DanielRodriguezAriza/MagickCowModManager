using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Exceptions
{
    public class LoadException : Exception
    {
        public LoadException() : base()
        { }

        public LoadException(string? message) : base($"Load Error : {message}")
        { }

        public LoadException(string? message, Exception innerException) : base($"Load Error : {message}", innerException)
        { }
    }
}
