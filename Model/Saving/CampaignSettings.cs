using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AutoEquipCompanions.Model.Saving
{
   public static class CampaignSettings
   {
      public static bool SettingsVisible { get; set; }
      public static Dictionary<string, CharacterSettings> CharacterSettings { get; private set; }

      public static void Initialize()
      {
         SettingsVisible = true;
         CharacterSettings = new Dictionary<string, CharacterSettings>();
      }

      public static string Save()
      {
         try
         {
            var characterSettingsJson = new JObject();
            foreach (var kv in CharacterSettings)
            {
               try
               {
                  characterSettingsJson[kv.Key] = kv.Value.ToJson();
               }
               catch
               {
                  // Skip this hero's entry rather than fail the whole save.
               }
            }

            return new JObject
            {
               ["SettingsVisible"] = SettingsVisible,
               ["CharacterSettings"] = characterSettingsJson
            }.ToString();
         }
         catch
         {
            // Never let a save-serialization failure escape into the game's save pipeline.
            return string.Empty;
         }
      }

      public static void Load(string json)
      {
         if (string.IsNullOrEmpty(json))
            return;

         try
         {
            var obj = JObject.Parse(json);
            SettingsVisible = obj["SettingsVisible"]?.Value<bool>() ?? true;

            var characterSettings = new Dictionary<string, CharacterSettings>();
            if (obj["CharacterSettings"] is JObject characterSettingsObj)
            {
               foreach (var prop in characterSettingsObj.Properties())
               {
                  try
                  {
                     if (prop.Value is JObject characterObj)
                        characterSettings[prop.Name] = Saving.CharacterSettings.FromJson(characterObj);
                  }
                  catch
                  {
                     // Skip this hero's corrupted entry; the rest of the save still loads.
                  }
               }
            }
            CharacterSettings = characterSettings;
         }
         catch
         {
            Initialize();
         }
      }
   }
}
