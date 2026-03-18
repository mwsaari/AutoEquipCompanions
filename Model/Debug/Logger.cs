using System;
using System.IO;

namespace AutoEquipCompanions.Model.Debug
{
   public static class Logger
   {
      private static string ModuleRoot
      {
         get
         {
            var assemblyDir = Path.GetDirectoryName(typeof(Logger).Assembly.Location);
            return Path.GetFullPath(Path.Combine(assemblyDir, "..", ".."));
         }
      }

      public static void WriteToLog(string message)
      {
         WriteToFile("aec_log.txt", $"[{DateTime.Now:HH:mm:ss}] {message}");
      }

      public static void WriteToItemDebug(string content)
      {
         WriteToFile("debug_items.txt", content);
      }

      private static void WriteToFile(string filename, string content)
      {
         try
         {
            File.WriteAllText(Path.Combine(ModuleRoot, filename), content);
         }
         catch { }
      }
   }
}
