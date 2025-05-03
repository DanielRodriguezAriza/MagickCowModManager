using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public struct AuthorInfo
    {
        public string Name { get; set; }
        public string[] Contacts { get; set; }

        public AuthorInfo()
        {
            this.Name = string.Empty;
            this.Contacts = Array.Empty<string>();
        }

        public AuthorInfo(string name, string[] contacts)
        {
            this.Name = name;
            this.Contacts = contacts;
        }

        public AuthorInfo(string name, string contact)
        {
            this.Name = name;
            this.Contacts = new[] { contact };
        }

        public AuthorInfo(string name)
        {
            this.Name = name;
            this.Contacts = Array.Empty<string>();
        }
    }
}
