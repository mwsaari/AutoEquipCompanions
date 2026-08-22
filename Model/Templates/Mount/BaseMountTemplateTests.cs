using AutoEquipCompanions.Model.Templates.Mount;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Mount
{
   public class BaseMountTemplateTests
   {
      // ── IsValidFor ──────────────────────────────────────────────────────────

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(default, EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void HarnessCandidate_InHorseSlot_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.HorseHarness;
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(
            new EquipmentElement(item), EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void HorseCandidate_InHarnessSlot_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.Horse;
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(
            new EquipmentElement(item), EquipmentIndex.HorseHarness, hero));
      }

      [Fact]
      public void InvalidSlot_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.Horse;
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(
            new EquipmentElement(item), EquipmentIndex.Weapon0, hero));
      }

      // ── Camel matching ──────────────────────────────────────────────────────

      [Fact]
      public void DefaultMount_RegularHorse_IsValid()
      {
         var hero = Helpers.MakeHero();
         var horse = Helpers.MakeHorse(isCamel: false);
         Assert.True(DefaultMountTemplate.Instance.IsValidFor(horse, EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void DefaultMount_Camel_IsInvalid()
      {
         var hero = Helpers.MakeHero();
         var camel = Helpers.MakeHorse(isCamel: true);
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(camel, EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void CamelMount_Camel_IsValid()
      {
         var hero = Helpers.MakeHero();
         var camel = Helpers.MakeHorse(isCamel: true);
         Assert.True(CamelMountTemplate.Instance.IsValidFor(camel, EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void CamelMount_RegularHorse_IsInvalid()
      {
         var hero = Helpers.MakeHero();
         var horse = Helpers.MakeHorse(isCamel: false);
         Assert.False(CamelMountTemplate.Instance.IsValidFor(horse, EquipmentIndex.Horse, hero));
      }

      // ── Harness family matching ─────────────────────────────────────────────

      [Fact]
      public void NoHorseEquipped_AnyHarnessFamily_IsValid()
      {
         var hero = Helpers.MakeHero();
         var harness = Helpers.MakeHorseHarness(familyType: 3);
         Assert.True(DefaultMountTemplate.Instance.IsValidFor(harness, EquipmentIndex.HorseHarness, hero));
      }

      [Fact]
      public void HarnessFamilyMatchesEquippedHorse_IsValid()
      {
         var horse = Helpers.MakeHorse(familyType: 2);
         var hero = Helpers.MakeHero(horse: horse);
         var harness = Helpers.MakeHorseHarness(familyType: 2);
         Assert.True(DefaultMountTemplate.Instance.IsValidFor(harness, EquipmentIndex.HorseHarness, hero));
      }

      [Fact]
      public void HarnessFamilyMismatchesEquippedHorse_IsInvalid()
      {
         var horse = Helpers.MakeHorse(familyType: 2);
         var hero = Helpers.MakeHero(horse: horse);
         var harness = Helpers.MakeHorseHarness(familyType: 5);
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(harness, EquipmentIndex.HorseHarness, hero));
      }

      // ── MaxTier ─────────────────────────────────────────────────────────────

      [Fact]
      public void LightMount_HarnessAtTierCap_IsValid()
      {
         var hero = Helpers.MakeHero();
         var harness = Helpers.MakeHorseHarness(tier: ItemObject.ItemTiers.Tier2);
         Assert.True(LightMountTemplate.Instance.IsValidFor(harness, EquipmentIndex.HorseHarness, hero));
      }

      [Fact]
      public void LightMount_HarnessAboveTierCap_IsInvalid()
      {
         var hero = Helpers.MakeHero();
         var harness = Helpers.MakeHorseHarness(tier: ItemObject.ItemTiers.Tier3);
         Assert.False(LightMountTemplate.Instance.IsValidFor(harness, EquipmentIndex.HorseHarness, hero));
      }

      // ── GetScore ────────────────────────────────────────────────────────────

      [Fact]
      public void GetScore_EmptyCandidate_ReturnsZero()
      {
         Assert.Equal(0, DefaultMountTemplate.Instance.GetScore(default));
      }

      [Fact]
      public void GetScore_DefaultMount_ScoresHorseByValue()
      {
         var horse = Helpers.MakeHorse();
         Assert.Equal(horse.ItemValue, DefaultMountTemplate.Instance.GetScore(horse));
      }

      [Fact]
      public void GetScore_LightMount_ScoresHorseBySpeed()
      {
         var fast = Helpers.MakeHorse(speed: 90);
         var slow = Helpers.MakeHorse(speed: 10);
         Assert.True(LightMountTemplate.Instance.GetScore(fast) > LightMountTemplate.Instance.GetScore(slow));
      }
   }
}
