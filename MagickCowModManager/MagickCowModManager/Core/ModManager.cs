using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    public class ModManager
    {
        private FileHandler handler;

        public string ModsContentPath { get; set; }
        public string GameContentPath { get; set; }

        public ModManager()
        {
            this.ModsContentPath = "./Mods";
            this.GameContentPath = "./Content";
            this.handler = new FileHandler();
        }

        public ModManager(string gamePath)
        {
            this.ModsContentPath = Path.Combine(gamePath, "/Mods");
            this.GameContentPath = Path.Combine(gamePath, "/Content");
            this.handler = new FileHandler();
        }

        public ModManager(string modsPath, string contentPath)
        {
            this.ModsContentPath = modsPath;
            this.GameContentPath = contentPath;
            this.handler = new FileHandler();
        }

    }
}
