using Newtonsoft.Json;
using System.Collections.Generic;

namespace AutoEquipCompanions.Model.Saving
{
    public static class Config
    {
        public static void Initialize()
        {
            SettingsVisible = true;
            CharacterSettings = new Dictionary<string, CharacterSettings>();
            GeneralSettings = new GeneralSettings();
        }

        internal static void ReadSaveData(string jsonString)
        {
            CharacterSettings = JsonConvert.DeserializeObject<Dictionary<string, CharacterSettings>>(jsonString);
        }

        internal static string CreateSaveData()
        {
            if (SettingsVisible != CharacterSettings.ContainsKey("CharacterSettings"))
            {
                if (SettingsVisible)
                {
                    CharacterSettings.Add("CharacterSettings", null);
                }
                else
                {
                    CharacterSettings.Remove("CharacterSettings");
                }
            }
            return JsonConvert.SerializeObject(Config.CharacterSettings);
        }

        internal static bool SettingsVisible;

        internal static Dictionary<string, CharacterSettings> CharacterSettings;

        internal static GeneralSettings GeneralSettings { get; private set; }
    }
}
