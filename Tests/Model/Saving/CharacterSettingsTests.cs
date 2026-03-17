using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates.Character;
using Newtonsoft.Json.Linq;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Saving
{
   public class CharacterSettingsTests
   {
      // ── Save ────────────────────────────────────────────────────────────────

      [Fact]
      public void ToJson_DefaultTemplate_WritesTemplateName()
      {
         var settings = new CharacterSettings().Initialize();
         var json = settings.ToJson();
         Assert.Equal("default", json["Template"]?.Value<string>());
      }

      [Fact]
      public void ToJson_DefaultTemplate_AllArmorSlotsEnabled()
      {
         var settings = new CharacterSettings().Initialize();
         var json = settings.ToJson();

         Assert.True(json[EquipmentIndex.Head.ToString()]?.Value<bool>());
         Assert.True(json[EquipmentIndex.Cape.ToString()]?.Value<bool>());
         Assert.True(json[EquipmentIndex.Body.ToString()]?.Value<bool>());
         Assert.True(json[EquipmentIndex.Gloves.ToString()]?.Value<bool>());
         Assert.True(json[EquipmentIndex.Leg.ToString()]?.Value<bool>());
      }

      [Fact]
      public void ToJson_DefaultTemplate_HorseAndHarnesDisabledByDefault()
      {
         var settings = new CharacterSettings().Initialize();
         var json = settings.ToJson();

         Assert.False(json[EquipmentIndex.Horse.ToString()]?.Value<bool>());
         Assert.False(json[EquipmentIndex.HorseHarness.ToString()]?.Value<bool>());
      }

      [Fact]
      public void ToJson_DefaultTemplate_AllWeaponSlotsEnabled()
      {
         var settings = new CharacterSettings().Initialize();
         var json = settings.ToJson();

         Assert.True(json[EquipmentIndex.Weapon0.ToString()]?.Value<bool>());
         Assert.True(json[EquipmentIndex.Weapon1.ToString()]?.Value<bool>());
         Assert.True(json[EquipmentIndex.Weapon2.ToString()]?.Value<bool>());
         Assert.True(json[EquipmentIndex.Weapon3.ToString()]?.Value<bool>());
      }

      [Fact]
      public void ToJson_WithDisabledHead_WritesFalseForHead()
      {
         var settings = new CharacterSettings().Initialize();
         settings[EquipmentIndex.Head] = false;
         var json = settings.ToJson();

         Assert.False(json[EquipmentIndex.Head.ToString()]?.Value<bool>());
      }

      [Fact]
      public void ToJson_WithDisabledWeapon0_WritesFalseForWeapon0()
      {
         var settings = new CharacterSettings().Initialize();
         settings[EquipmentIndex.Weapon0] = false;
         var json = settings.ToJson();

         Assert.False(json[EquipmentIndex.Weapon0.ToString()]?.Value<bool>());
      }

      // ── Load ────────────────────────────────────────────────────────────────

      [Fact]
      public void FromJson_DefaultTemplate_LoadsDefaultTemplate()
      {
         var json = BuildJson("default");
         var settings = CharacterSettings.FromJson(json);
         Assert.Same(CharacterTemplate.Instance, settings.Template);
      }

      [Fact]
      public void FromJson_InfantryCaptainTemplate_LoadsInfantryCaptainTemplate()
      {
         var json = BuildJson("infantry_captain");
         var settings = CharacterSettings.FromJson(json);
         Assert.Same(InfantryCaptainTemplate.Instance, settings.Template);
      }

      [Fact]
      public void FromJson_WithDisabledHead_HeadToggleIsFalse()
      {
         var json = BuildJson("default", (EquipmentIndex.Head.ToString(), false));
         var settings = CharacterSettings.FromJson(json);
         Assert.False(settings[EquipmentIndex.Head]);
      }

      [Fact]
      public void FromJson_WithDisabledWeapon2_Weapon2ToggleIsFalse()
      {
         var json = BuildJson("default", (EquipmentIndex.Weapon2.ToString(), false));
         var settings = CharacterSettings.FromJson(json);
         Assert.False(settings[EquipmentIndex.Weapon2]);
      }

      [Fact]
      public void FromJson_WithEnabledHorse_HorseToggleIsTrue()
      {
         var json = BuildJson("default", (EquipmentIndex.Horse.ToString(), true));
         var settings = CharacterSettings.FromJson(json);
         Assert.True(settings[EquipmentIndex.Horse]);
      }

      [Fact]
      public void FromJson_MissingToggle_DefaultsToTrue()
      {
         var json = new JObject { ["Template"] = "default" };
         var settings = CharacterSettings.FromJson(json);
         Assert.True(settings[EquipmentIndex.Head]);
      }

      // ── Roundtrip ───────────────────────────────────────────────────────────

      [Fact]
      public void RoundTrip_DefaultTemplate_PreservesTemplate()
      {
         var original = new CharacterSettings().Initialize();
         var loaded = CharacterSettings.FromJson(original.ToJson());
         Assert.Equal(original.Template.Name, loaded.Template.Name);
      }

      [Fact]
      public void RoundTrip_WithVariousDisabledSlots_PreservesAllToggles()
      {
         var original = new CharacterSettings().Initialize();
         original[EquipmentIndex.Head] = false;
         original[EquipmentIndex.Weapon1] = false;
         original[EquipmentIndex.Horse] = true;

         var loaded = CharacterSettings.FromJson(original.ToJson());

         Assert.False(loaded[EquipmentIndex.Head]);
         Assert.True(loaded[EquipmentIndex.Body]);
         Assert.False(loaded[EquipmentIndex.Weapon1]);
         Assert.True(loaded[EquipmentIndex.Weapon0]);
         Assert.True(loaded[EquipmentIndex.Horse]);
      }

      // ── Invalid templates ───────────────────────────────────────────────────

      [Fact]
      public void FromJson_UnknownTemplate_FallsBackToNull()
      {
         var json = BuildJson("not_a_real_template");
         var settings = CharacterSettings.FromJson(json);
         Assert.Same(TemplateRegistry.Null, settings.Template);
      }

      [Fact]
      public void FromJson_NullTemplate_FallsBackToNull()
      {
         var json = new JObject { ["Template"] = null };
         var settings = CharacterSettings.FromJson(json);
         Assert.Same(TemplateRegistry.Null, settings.Template);
      }

      [Fact]
      public void FromJson_MissingTemplateField_FallsBackToNull()
      {
         var settings = CharacterSettings.FromJson(new JObject());
         Assert.Same(TemplateRegistry.Null, settings.Template);
      }

      [Fact]
      public void CampaignSettings_Load_MalformedJson_DoesNotThrow()
      {
         CampaignSettings.Initialize();
         CampaignSettings.Load("{ this is not valid json }");
         Assert.NotNull(CampaignSettings.CharacterSettings);
      }

      [Fact]
      public void CampaignSettings_Load_EmptyString_DoesNotThrow()
      {
         CampaignSettings.Initialize();
         CampaignSettings.Load("");
         Assert.NotNull(CampaignSettings.CharacterSettings);
      }

      [Fact]
      public void CampaignSettings_Load_UnknownTemplateInCharacterEntry_DoesNotThrow()
      {
         CampaignSettings.Initialize();
         var json = new JObject
         {
            ["SettingsVisible"] = true,
            ["CharacterSettings"] = new JObject
            {
               ["hero_1"] = BuildJson("unknown_template")
            }
         }.ToString();

         CampaignSettings.Load(json);

         Assert.True(CampaignSettings.CharacterSettings.ContainsKey("hero_1"));
         Assert.Same(TemplateRegistry.Null, CampaignSettings.CharacterSettings["hero_1"].Template);
      }

      // ── Helpers ─────────────────────────────────────────────────────────────

      private static JObject BuildJson(string templateName, params (string slot, bool enabled)[] toggles)
      {
         var json = new JObject { ["Template"] = templateName };
         foreach (var (slot, enabled) in toggles)
            json[slot] = enabled;
         return json;
      }
   }
}
