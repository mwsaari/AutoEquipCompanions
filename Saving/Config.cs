﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace AutoEquipCompanions.Saving
{
    public static class Config
    {
        public static void Initialize()
        {
            SettingsVisible = true;
            CharacterSettings = new Dictionary<string, CharacterSettings>();
        }

        internal static void ReadSaveData(string jsonString)
        {
            CharacterSettings = JsonConvert.DeserializeObject<Dictionary<string, CharacterSettings>>(jsonString);
        }

        internal static string CreateSaveData()
        {
            var setCharacterSettings = CharacterSettings.ContainsKey("CharacterSettings");
            if (SettingsVisible != setCharacterSettings)
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
    }
}
