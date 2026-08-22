using AutoEquipCompanions.Model.Saving;
using Newtonsoft.Json.Linq;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Saving
{
   public class CampaignSettingsTests
   {
      // ── Load ────────────────────────────────────────────────────────────────

      [Fact]
      public void Load_ValidJson_SetsSettingsVisibleAndCharacterSettings()
      {
         CampaignSettings.Initialize();
         var json = new JObject
         {
            ["SettingsVisible"] = false,
            ["CharacterSettings"] = new JObject { ["hero_1"] = BuildCharacterJson("default") }
         }.ToString();

         CampaignSettings.Load(json);

         Assert.False(CampaignSettings.SettingsVisible);
         Assert.True(CampaignSettings.CharacterSettings.ContainsKey("hero_1"));
      }

      [Fact]
      public void Load_MissingSettingsVisible_DefaultsToTrue()
      {
         CampaignSettings.Initialize();
         CampaignSettings.Load(new JObject { ["CharacterSettings"] = new JObject() }.ToString());
         Assert.True(CampaignSettings.SettingsVisible);
      }

      [Fact]
      public void Load_MalformedJson_DoesNotThrow()
      {
         CampaignSettings.Initialize();
         CampaignSettings.Load("{ this is not valid json }");
         Assert.NotNull(CampaignSettings.CharacterSettings);
      }

      [Fact]
      public void Load_EmptyString_DoesNotThrow()
      {
         CampaignSettings.Initialize();
         CampaignSettings.Load("");
         Assert.NotNull(CampaignSettings.CharacterSettings);
      }

      [Fact]
      public void Load_UnknownTemplateInCharacterEntry_FallsBackToNullTemplate()
      {
         CampaignSettings.Initialize();
         var json = new JObject
         {
            ["SettingsVisible"] = true,
            ["CharacterSettings"] = new JObject { ["hero_1"] = BuildCharacterJson("unknown_template") }
         }.ToString();

         CampaignSettings.Load(json);

         Assert.True(CampaignSettings.CharacterSettings.ContainsKey("hero_1"));
         Assert.Same(TemplateRegistry.Null, CampaignSettings.CharacterSettings["hero_1"].Template);
      }

      // ── Save ────────────────────────────────────────────────────────────────

      [Fact]
      public void Save_Empty_WritesSettingsVisibleAndEmptyCharacterSettings()
      {
         CampaignSettings.Initialize();
         var json = JObject.Parse(CampaignSettings.Save());

         Assert.True(json["SettingsVisible"]?.Value<bool>());
         Assert.Empty(((JObject)json["CharacterSettings"]).Properties());
      }

      [Fact]
      public void Save_WithCharacterSettings_IncludesSerializedEntries()
      {
         CampaignSettings.Initialize();
         CampaignSettings.CharacterSettings["hero_1"] = new CharacterSettings().Initialize();

         var json = JObject.Parse(CampaignSettings.Save());

         Assert.Equal("default", json["CharacterSettings"]["hero_1"]["Template"]?.Value<string>());
      }

      [Fact]
      public void SaveThenLoad_RoundTrips()
      {
         CampaignSettings.Initialize();
         CampaignSettings.SettingsVisible = false;
         var heroSettings = new CharacterSettings().Initialize();
         heroSettings[EquipmentIndex.Head] = false;
         CampaignSettings.CharacterSettings["hero_1"] = heroSettings;

         var saved = CampaignSettings.Save();
         CampaignSettings.Load(saved);

         Assert.False(CampaignSettings.SettingsVisible);
         Assert.False(CampaignSettings.CharacterSettings["hero_1"][EquipmentIndex.Head]);
      }

      // ── Helpers ─────────────────────────────────────────────────────────────

      private static JObject BuildCharacterJson(string templateName)
      {
         return new JObject { ["Template"] = templateName };
      }
   }
}
