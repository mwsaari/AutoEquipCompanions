using Newtonsoft.Json;
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
         return JsonConvert.SerializeObject(new { SettingsVisible, CharacterSettings });
      }

      public static void Load(string json)
      {
         if (string.IsNullOrEmpty(json))
            return;

         try
         {
            var data = JsonConvert.DeserializeAnonymousType(
               json,
               new
               {
                  SettingsVisible = true,
                  CharacterSettings = new Dictionary<string, CharacterSettings>()
               });
            SettingsVisible = data.SettingsVisible;
            CharacterSettings = data.CharacterSettings;
         }
         catch
         {
            Initialize();
         }
      }
   }
}
