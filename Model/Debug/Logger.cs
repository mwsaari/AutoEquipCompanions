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

      public static void WriteToItemDebug(string content)
      {
         WriteToFile("debug_items.txt", content);
      }

      public static void WriteToTrace(string message)
      {
         AppendToFile("debug_trace.txt", $"[{DateTime.Now:HH:mm:ss.fff}] {message}");
      }

      private static void WriteToFile(string filename, string content)
      {
         try
         {
            File.WriteAllText(Path.Combine(ModuleRoot, filename), content);
         }
         catch { }
      }

      private static void AppendToFile(string filename, string line)
      {
         try
         {
            File.AppendAllText(Path.Combine(ModuleRoot, filename), line + Environment.NewLine);
         }
         catch { }
      }
   }
}
