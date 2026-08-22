using System.Collections.Generic;
using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model
{
   public class HeroToggleControllerTests
   {
      private const string HeroId = "hero_1";

      private static HeroToggleController MakeController(
         string currentHero, Dictionary<string, CharacterSettings> heroToggles = null)
      {
         return new HeroToggleController(
            () => currentHero,
            heroToggles ?? new Dictionary<string, CharacterSettings>(),
            new CharacterSettings().Initialize());
      }

      [Fact]
      public void CharacterToggle_NoCurrentHero_DefaultsTrue()
      {
         var controller = MakeController(null);
         Assert.True(controller.CharacterToggle);
      }

      [Fact]
      public void CharacterToggle_UnknownHero_DefaultsTrue()
      {
         var controller = MakeController(HeroId);
         Assert.True(controller.CharacterToggle);
      }

      [Fact]
      public void CharacterToggle_SetThenGet_RoundTrips()
      {
         var toggles = new Dictionary<string, CharacterSettings>();
         var controller = MakeController(HeroId, toggles);

         controller.CharacterToggle = false;

         Assert.False(controller.CharacterToggle);
         Assert.True(toggles.ContainsKey(HeroId));
      }

      [Fact]
      public void CharacterToggle_SetForNoCurrentHero_IsNoOp()
      {
         var toggles = new Dictionary<string, CharacterSettings>();
         var controller = MakeController(null, toggles);

         controller.CharacterToggle = false;

         Assert.Empty(toggles);
      }

      [Fact]
      public void GetSlotToggle_UnknownHero_FallsBackToDefaultSettings()
      {
         var controller = MakeController(HeroId);
         Assert.Equal(new CharacterSettings().Initialize()[EquipmentIndex.Head], controller.GetSlotToggle(EquipmentIndex.Head));
      }

      [Fact]
      public void GetSlotToggle_KnownHero_ReflectsHeroSettings()
      {
         var settings = new CharacterSettings().Initialize();
         settings[EquipmentIndex.Head] = false;
         var toggles = new Dictionary<string, CharacterSettings> { [HeroId] = settings };
         var controller = MakeController(HeroId, toggles);

         Assert.False(controller.GetSlotToggle(EquipmentIndex.Head));
      }

      [Fact]
      public void ToggleEquipment_KnownHero_Flips()
      {
         var settings = new CharacterSettings().Initialize();
         var toggles = new Dictionary<string, CharacterSettings> { [HeroId] = settings };
         var controller = MakeController(HeroId, toggles);

         controller.ToggleEquipment(EquipmentIndex.Head);

         Assert.False(settings[EquipmentIndex.Head]);
      }

      [Fact]
      public void ToggleEquipment_UnseenHero_CreatesEntryAndFlips()
      {
         var toggles = new Dictionary<string, CharacterSettings>();
         var controller = MakeController(HeroId, toggles);

         controller.ToggleEquipment(EquipmentIndex.Head);

         Assert.True(toggles.ContainsKey(HeroId));
         Assert.False(toggles[HeroId][EquipmentIndex.Head]);
      }

      [Fact]
      public void ToggleEquipment_NoCurrentHero_IsNoOp()
      {
         var toggles = new Dictionary<string, CharacterSettings>();
         var controller = MakeController(null, toggles);

         controller.ToggleEquipment(EquipmentIndex.Head);

         Assert.Empty(toggles);
      }
   }
}
