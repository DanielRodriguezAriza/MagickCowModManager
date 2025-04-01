using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagickCowModManager.Core.Data
{
    public class ModProfile
    {
        private ModProfileData profileData;

        public string Name { get; private set; }
        public List<string> Mods { get{ return this.profileData.EnabledMods; } }

        public ModProfile(string filePath)
        {
            string json = File.ReadAllText(filePath);

            this.Name = filePath;
            this.profileData = JsonSerializer.Deserialize<ModProfileData>(json);
        }
    }
}
