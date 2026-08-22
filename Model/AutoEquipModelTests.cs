using System.Collections.Generic;
using System.Linq;
using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Character;
using AutoEquipCompanions.Model.Templates.Weapon;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model
{
   // The outer AutoEquipCompanions() orchestration (roster fetch from MobileParty.MainParty,
   // real InventoryLogic.AddTransferCommand calls, InformationManager messages) stays untested
   // integration surface, same category as TestSupport/EquipmentIntegrationTests.cs. These tests
   // cover the decision logic that was extracted to be independent of those statics/side effects.
   public class AutoEquipModelTests
   {
      private static AutoEquipModel MakeModel() => new AutoEquipModel(null, new HashSet<string>());

      // ── GetBestReplacement ───────────────────────────────────────────────────

      [Fact]
      public void GetBestReplacement_NoCandidates_ReturnsNull()
      {
         var model = MakeModel();
         var hero = Helpers.MakeHero();
         var result = model.GetBestReplacement(
            Enumerable.Empty<ItemRosterElement>(), hero, EquipmentIndex.Weapon0, DefaultWeaponTemplate.Instance, default);
         Assert.Null(result);
      }

      [Fact]
      public void GetBestReplacement_HigherValueCandidate_ReturnsIt()
      {
         var model = MakeModel();
         var hero = Helpers.MakeHero();
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 10);
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 50);

         var result = model.GetBestReplacement(
            new[] { new ItemRosterElement(candidate, 1) }, hero, EquipmentIndex.Weapon0, DefaultWeaponTemplate.Instance, current);

         Assert.Equal(candidate.Item, result.Value.EquipmentElement.Item);
      }

      [Fact]
      public void GetBestReplacement_NoCandidateBeatsCurrent_ReturnsNull()
      {
         var model = MakeModel();
         var hero = Helpers.MakeHero();
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 50);
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 10);

         var result = model.GetBestReplacement(
            new[] { new ItemRosterElement(candidate, 1) }, hero, EquipmentIndex.Weapon0, DefaultWeaponTemplate.Instance, current);

         Assert.Null(result);
      }

      [Fact]
      public void GetBestReplacement_MultipleCandidates_ReturnsHighestScoring()
      {
         var model = MakeModel();
         var hero = Helpers.MakeHero();
         var low = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 20);
         var high = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 90);

         var result = model.GetBestReplacement(
            new[] { new ItemRosterElement(low, 1), new ItemRosterElement(high, 1) },
            hero, EquipmentIndex.Weapon0, DefaultWeaponTemplate.Instance, default);

         Assert.Equal(high.Item, result.Value.EquipmentElement.Item);
      }

      // ── DetermineSlotChanges ─────────────────────────────────────────────────

      private static CharacterSettings MakeSettings(bool weapon0Enabled = true)
      {
         var settings = new CharacterSettings().Initialize();
         settings.Template = new CustomCharacterTemplate("test_template",
            new[] { (EquipmentIndex.Weapon0, (ISlotTemplate)DefaultWeaponTemplate.Instance) });
         settings[EquipmentIndex.Weapon0] = weapon0Enabled;
         return settings;
      }

      [Fact]
      public void DetermineSlotChanges_SlotDisabled_ReturnsNoDecision()
      {
         var model = MakeModel();
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 10);
         var better = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 90);
         var hero = Helpers.MakeHero(slot0: current);

         var decisions = model.DetermineSlotChanges(
            hero, MakeSettings(weapon0Enabled: false), new[] { new ItemRosterElement(better, 1) });

         Assert.Empty(decisions);
      }

      [Fact]
      public void DetermineSlotChanges_BetterItemAvailable_ReturnsEquipDecision()
      {
         var model = MakeModel();
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 10);
         var better = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 90);
         var hero = Helpers.MakeHero(slot0: current);

         var decisions = model.DetermineSlotChanges(
            hero, MakeSettings(), new[] { new ItemRosterElement(better, 1) }).ToList();

         var decision = Assert.Single(decisions);
         Assert.Equal(EquipmentIndex.Weapon0, decision.Slot);
         Assert.Equal(better.Item, decision.Replacement.Value.EquipmentElement.Item);
      }

      [Fact]
      public void DetermineSlotChanges_NoBetterItem_CurrentValid_ReturnsNoDecision()
      {
         var model = MakeModel();
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, value: 90);
         var hero = Helpers.MakeHero(slot0: current);

         var decisions = model.DetermineSlotChanges(hero, MakeSettings(), Enumerable.Empty<ItemRosterElement>());

         Assert.Empty(decisions);
      }

      [Fact]
      public void DetermineSlotChanges_NoBetterItem_CurrentInvalidForTemplate_ReturnsUnequipDecision()
      {
         var model = MakeModel();
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 80, value: 90);
         var hero = Helpers.MakeHero(oneHandedSkill: 20, slot0: current);

         var decisions = model.DetermineSlotChanges(hero, MakeSettings(), Enumerable.Empty<ItemRosterElement>()).ToList();

         var decision = Assert.Single(decisions);
         Assert.Equal(EquipmentIndex.Weapon0, decision.Slot);
         Assert.Null(decision.Replacement);
      }

      [Fact]
      public void DetermineSlotChanges_EmptyCurrent_NoCandidates_ReturnsNoDecision()
      {
         var model = MakeModel();
         var hero = Helpers.MakeHero();

         var decisions = model.DetermineSlotChanges(hero, MakeSettings(), Enumerable.Empty<ItemRosterElement>());

         Assert.Empty(decisions);
      }
   }
}
