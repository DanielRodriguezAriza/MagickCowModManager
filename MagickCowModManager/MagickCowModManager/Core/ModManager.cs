using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
    // TODO : In the future, add some mod profile handling kind of stuff rather than a single mod load order file. For example, have a profile0, profile1, etc... files, each being just a different mod load order file.
    public class ModManager
    {
        #region Private Variables

        private FileHandler handler;

        #endregion

        #region Public Properties

        public string ModsContentPath { get; set; }
        public string GameContentPath { get; set; }

        #endregion

        #region Constructors

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

        #endregion

        #region PublicMethods

        public void LoadAllMods()
        {
            // TODO : Implement
        }

        #endregion

        #region PrivateMethods

        private void ReadModLoadOrder(string path)
        {
            File.Open(path, FileMode.Open, FileAccess.Read);
        }

        #endregion

    }
}
