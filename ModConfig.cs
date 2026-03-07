using System.IO;

namespace AutoEquipCompanions
{
   public class ModConfig
   {
      public bool UseV2 { get; private set; }

      public static ModConfig Load()
      {
         var config = new ModConfig();
         var path = GetConfigPath();
         if (!File.Exists(path))
            return config;

         foreach (var line in File.ReadAllLines(path))
         {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("#") || trimmed.Length == 0)
               continue;

            var separatorIndex = trimmed.IndexOf('=');
            if (separatorIndex < 0)
               continue;

            var key = trimmed.Substring(0, separatorIndex).Trim();
            var value = trimmed.Substring(separatorIndex + 1).Trim();

            switch (key)
            {
               case "UseV2":
                  config.UseV2 = value.ToLowerInvariant() == "true";
                  break;
            }
         }

         return config;
      }

      private static string GetConfigPath()
      {
         // Assembly is at: Modules/AutoEquipCompanions/bin/Win64_Shipping_Client/
         // Module root is two directories up.
         var assemblyDir = Path.GetDirectoryName(typeof(ModConfig).Assembly.Location);
         var moduleRoot = Path.GetFullPath(Path.Combine(assemblyDir, "..", ".."));
         return Path.Combine(moduleRoot, "config.ini");
      }
   }
}
