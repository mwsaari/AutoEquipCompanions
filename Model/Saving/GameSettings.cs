using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AutoEquipCompanions.Model.Saving
{
   public class GameSettings
   {
      public bool CanAutoEquipLocked { get; set; } = true;

      // Placeholder for future template support
      public List<object> Templates { get; set; } = new List<object>();

      public static GameSettings Load()
      {
         var path = GetPath();
         if (!File.Exists(path))
            return new GameSettings();
         try
         {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<GameSettings>(json) ?? new GameSettings();
         }
         catch
         {
            return new GameSettings();
         }
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
         var assemblyDir = Path.GetDirectoryName(typeof(GameSettings).Assembly.Location);
         var moduleRoot = Path.GetFullPath(Path.Combine(assemblyDir, "..", ".."));
         return Path.Combine(moduleRoot, "game_settings.json");
      }
   }
}
