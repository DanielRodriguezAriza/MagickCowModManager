using MagickCowModManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagickCowModManager.Core
{
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

        private ModProfile[] GetProfiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(this.ModsContentPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            List<ModProfile> foundProfiles = new List<ModProfile>();

            foreach (var fileInfo in fileInfos)
            {
                try
                {
                    ModProfile profile = GetProfile(fileInfo.FullName);
                    foundProfiles.Add(profile);
                }
                catch
                {
                    // Do nothing, just skip the file if it's not a valid JSON file that matches the ModProfile "schema"...
                }
            }

            return foundProfiles.ToArray();
        }

        private ModProfile GetProfile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var profileData = JsonSerializer.Deserialize<ModProfileData>(json);
        }

        #endregion

    }
}
