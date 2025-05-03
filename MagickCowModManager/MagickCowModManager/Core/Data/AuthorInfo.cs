using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Data
{
    public struct AuthorInfo
    {
        public string Name { get; set; }
        public string[] Contacts { get; set; }

        public AuthorInfo()
        {
            Name = string.Empty;
            Contacts = Array.Empty<string>();
        }

        public AuthorInfo(string name, string[] contacts)
        {
            Name = name;
            Contacts = contacts;
        }

        public AuthorInfo(string name, string contact)
        {
            Name = name;
            Contacts = new[] { contact };
        }

        public AuthorInfo(string name)
        {
            Name = name;
            Contacts = Array.Empty<string>();
        }
    }
}
