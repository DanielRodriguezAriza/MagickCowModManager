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

        // NOTE : Maybe rename to LoadProfile ? The point is that this function basically loads a profile and sets the active mods to be the ones within this profile.
        public void ApplyProfile(string filePath)
        {
            // TODO : Implement
        }

        public Profile[] GetProfiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(this.ModsContentPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            List<Profile> foundProfiles = new List<Profile>();

            foreach (var fileInfo in fileInfos)
            {
                try
                {
                    Profile profile = GetProfile(fileInfo.FullName);
                    foundProfiles.Add(profile);
                }
                catch
                {
                    // Do nothing, just skip the file if it's not a valid JSON file that matches the ModProfile "schema"...
                }
            }

            return foundProfiles.ToArray();
        }

        public Profile GetProfile(string filePath)
        {
            Profile profile = new Profile(filePath);
            return profile;
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
