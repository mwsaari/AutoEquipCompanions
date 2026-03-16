using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AutoEquipCompanions.Model.Saving
{
   public class GameSettings
   {
      public bool CanAutoEquipLocked { get; set; } = false;
      public bool DebugEnabled { get; set; } = false;
      public bool BastardSwordsAreOneHanded { get; set; } = true;

      // Placeholder for future template support
      public List<object> Templates { get; set; } = new List<object>();

      public void Load()
      {
         var path = GetPath();
         if (!File.Exists(path))
            return;

         try
         {
            var json = File.ReadAllText(path);
            JsonConvert.PopulateObject(json, this);
         }
         catch { }
      }

      public void Save()
      {
         var path = GetPath();
         try
         {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
         }
         catch { }
      }

      private static string GetPath()
      {
         var assemblyDir = Path.GetDirectoryName(typeof (GameSettings).Assembly.Location);
         var moduleRoot = Path.GetFullPath(Path.Combine(assemblyDir, "..", ".."));
         return Path.Combine(moduleRoot, "game_settings.json");
      }
   }
}
