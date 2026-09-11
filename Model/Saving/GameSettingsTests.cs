using System;
using System.IO;
using AutoEquipCompanions.Model.Saving;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Saving
{
   public class GameSettingsTests : IDisposable
   {
      private readonly string _path = Path.GetTempFileName();

      public void Dispose()
      {
         if (File.Exists(_path))
            File.Delete(_path);
      }

      [Fact]
      public void Load_ValidJson_PopulatesFields()
      {
         File.WriteAllText(_path,
            "{\"CanAutoEquipLockedItems\":true,\"DebugEnabled\":true,\"DumpItemsEnabled\":true,\"BastardSwordsAreOneHanded\":false,\"UseTemplates\":true}");

         var settings = new GameSettings();
         settings.Load(_path);

         Assert.True(settings.CanAutoEquipLockedItems);
         Assert.True(settings.DebugEnabled);
         Assert.True(settings.DumpItemsEnabled);
         Assert.False(settings.BastardSwordsAreOneHanded);
         Assert.True(settings.UseTemplates);
      }

      [Fact]
      public void Load_MissingFile_LeavesDefaults()
      {
         File.Delete(_path);

         var settings = new GameSettings();
         settings.Load(_path);

         Assert.False(settings.CanAutoEquipLockedItems);
         Assert.False(settings.DebugEnabled);
         Assert.False(settings.DumpItemsEnabled);
         Assert.True(settings.BastardSwordsAreOneHanded);
         Assert.False(settings.UseTemplates);
      }

      [Fact]
      public void Load_CorruptJson_LeavesDefaults()
      {
         File.WriteAllText(_path, "{ not valid json");

         var settings = new GameSettings();
         settings.Load(_path);

         Assert.False(settings.CanAutoEquipLockedItems);
         Assert.True(settings.BastardSwordsAreOneHanded);
      }

      [Fact]
      public void Save_WritesReadableJson()
      {
         var settings = new GameSettings { CanAutoEquipLockedItems = true, DebugEnabled = true };
         settings.Save(_path);

         var json = File.ReadAllText(_path);
         Assert.Contains("\"CanAutoEquipLockedItems\": true", json);
         Assert.Contains("\"DebugEnabled\": true", json);
      }

      [Fact]
      public void SaveThenLoad_RoundTrips()
      {
         var original = new GameSettings
         {
            CanAutoEquipLockedItems = true,
            DebugEnabled = true,
            DumpItemsEnabled = true,
            BastardSwordsAreOneHanded = false,
            UseTemplates = true
         };
         original.Save(_path);

         var loaded = new GameSettings();
         loaded.Load(_path);

         Assert.Equal(original.CanAutoEquipLockedItems, loaded.CanAutoEquipLockedItems);
         Assert.Equal(original.DebugEnabled, loaded.DebugEnabled);
         Assert.Equal(original.DumpItemsEnabled, loaded.DumpItemsEnabled);
         Assert.Equal(original.BastardSwordsAreOneHanded, loaded.BastardSwordsAreOneHanded);
         Assert.Equal(original.UseTemplates, loaded.UseTemplates);
      }
   }
}
