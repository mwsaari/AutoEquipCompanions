using AutoEquipCompanions.Model.Templates.Armor;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Armor
{
   public class BaseArmorTemplateTests
   {
      // ── IsValidFor ──────────────────────────────────────────────────────────

      [Theory]
      [InlineData(EquipmentIndex.Head, ItemObject.ItemTypeEnum.HeadArmor)]
      [InlineData(EquipmentIndex.Cape, ItemObject.ItemTypeEnum.Cape)]
      [InlineData(EquipmentIndex.Body, ItemObject.ItemTypeEnum.BodyArmor)]
      [InlineData(EquipmentIndex.Gloves, ItemObject.ItemTypeEnum.HandArmor)]
      [InlineData(EquipmentIndex.Leg, ItemObject.ItemTypeEnum.LegArmor)]
      public void CorrectSlotAndType_ReturnsTrue(EquipmentIndex slot, ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeArmor(type);
         var hero = Helpers.MakeHero();
         Assert.True(DefaultArmorTemplate.Instance.IsValidFor(el, slot, hero));
      }

      [Theory]
      [InlineData(EquipmentIndex.Head, ItemObject.ItemTypeEnum.BodyArmor)]
      [InlineData(EquipmentIndex.Body, ItemObject.ItemTypeEnum.HeadArmor)]
      [InlineData(EquipmentIndex.Cape, ItemObject.ItemTypeEnum.LegArmor)]
      [InlineData(EquipmentIndex.Gloves, ItemObject.ItemTypeEnum.Cape)]
      [InlineData(EquipmentIndex.Leg, ItemObject.ItemTypeEnum.HandArmor)]
      public void WrongTypeForSlot_ReturnsFalse(EquipmentIndex slot, ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeArmor(type);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, slot, hero));
      }

      [Fact]
      public void WeaponSlot_ReturnsFalse()
      {
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void HorseSlot_ReturnsFalse()
      {
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(default, EquipmentIndex.Head, hero));
      }

      [Fact]
      public void NoArmorComponent_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.HeadArmor;
         var el = new EquipmentElement(item);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      // ── MaxTier ─────────────────────────────────────────────────────────────

      [Fact]
      public void LightArmor_AtTierCap_IsValid()
      {
         var hero = Helpers.MakeHero();
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor, tier: ItemObject.ItemTiers.Tier2);
         Assert.True(LightArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      [Fact]
      public void LightArmor_AboveTierCap_IsInvalid()
      {
         var hero = Helpers.MakeHero();
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor, tier: ItemObject.ItemTiers.Tier3);
         Assert.False(LightArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      [Fact]
      public void MediumArmor_AtTierCap_IsValid()
      {
         var hero = Helpers.MakeHero();
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor, tier: ItemObject.ItemTiers.Tier3);
         Assert.True(MediumArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      [Fact]
      public void MediumArmor_AboveTierCap_IsInvalid()
      {
         var hero = Helpers.MakeHero();
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor, tier: ItemObject.ItemTiers.Tier4);
         Assert.False(MediumArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      [Fact]
      public void HeavyArmor_HasNoTierCap_HighestTierIsValid()
      {
         var hero = Helpers.MakeHero();
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor, tier: ItemObject.ItemTiers.Tier6);
         Assert.True(HeavyArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      // ── GetScore ────────────────────────────────────────────────────────────
      // All concrete armor templates use ArmorField.ArmorTotal, so this is the only
      // ComparisonField branch reachable in production; it's tested via a real template
      // rather than a test-only subclass.

      [Fact]
      public void GetScore_EmptyCandidate_ReturnsZero()
      {
         Assert.Equal(0, DefaultArmorTemplate.Instance.GetScore(default));
      }

      [Fact]
      public void GetScore_SumsAllFourArmorPieces()
      {
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.BodyArmor,
            headArmor: 1, bodyArmor: 2, armArmor: 3, legArmor: 4);
         Assert.Equal(10, DefaultArmorTemplate.Instance.GetScore(el));
      }
   }
}
