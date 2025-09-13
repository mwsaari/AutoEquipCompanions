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
            Presets = new List<Preset>();
        }

        internal static void ReadSaveData(string characterJson, string presetJson)
        {
            if (!string.IsNullOrEmpty(presetJson))
            {
                Presets = JsonConvert.DeserializeObject<List<Preset>>(presetJson);
            }

            // Character Settings needs to be deserialized after presets. So the character setting object can lookup the preset object.
            if (!string.IsNullOrEmpty(characterJson))
            {
                CharacterSettings = JsonConvert.DeserializeObject<Dictionary<string, CharacterSettings>>(characterJson);
            }
        }

        internal static string CreateCharacterSaveData()
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
            return JsonConvert.SerializeObject(CharacterSettings);
        }

        internal static string CreatePresetSaveData()
        {
            return JsonConvert.SerializeObject(Presets);
        }

        internal static bool SettingsVisible;

        internal static Dictionary<string, CharacterSettings> CharacterSettings;

        internal static GeneralSettings GeneralSettings { get; private set; }

        internal static List<Preset> Presets;
    }
}
