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
         var characterSettingsJson = new JObject();
         foreach (var kv in CharacterSettings)
            characterSettingsJson[kv.Key] = kv.Value.ToJson();

         return new JObject
         {
            ["SettingsVisible"] = SettingsVisible,
            ["CharacterSettings"] = characterSettingsJson
         }.ToString();
      }

      public static void Load(string json)
      {
         if (string.IsNullOrEmpty(json))
            return;

         try
         {
            var obj = JObject.Parse(json);
            SettingsVisible = obj["SettingsVisible"]?.Value<bool>() ?? true;

            CharacterSettings = new Dictionary<string, CharacterSettings>();
            var characterSettingsObj = (JObject)obj["CharacterSettings"];
            if (characterSettingsObj != null)
            {
               foreach (var prop in characterSettingsObj.Properties())
                  CharacterSettings[prop.Name] = Saving.CharacterSettings.FromJson((JObject)prop.Value);
            }
         }
         catch
         {
            Initialize();
         }
      }
   }
}
