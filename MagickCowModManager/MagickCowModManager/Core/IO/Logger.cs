using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.IO
{
    public struct LogMessage
    {
        public int status { get; set; }
        public string message { get; set; }
        public object? data { get; set; }
    }

    public static class Logger
    {
        public static void Log(int status, string message, object? data = null)
        {
            LogMessage dataWrapper = new()
            {
                status = status,
                message = message,
                data = data
            };
            string json = JsonSerializer.Serialize(dataWrapper);
            Console.WriteLine(json);
        }
    }
}
