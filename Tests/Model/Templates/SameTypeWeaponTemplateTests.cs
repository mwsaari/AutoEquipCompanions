using AutoEquipCompanions.Model.Templates;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates
{
   public class SameTypeWeaponTemplateTests
   {
      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero(
            slot0: Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon));
         Assert.False(SameTypeWeaponTemplate.Instance.IsValidFor(
            default, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCurrentSlot_ReturnsFalse()
      {
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon);
         var hero = Helpers.MakeHero();
         Assert.False(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword)]
      [InlineData(ItemObject.ItemTypeEnum.TwoHandedWeapon, WeaponClass.TwoHandedSword)]
      [InlineData(ItemObject.ItemTypeEnum.Polearm, WeaponClass.TwoHandedPolearm)]
      [InlineData(ItemObject.ItemTypeEnum.Bow, WeaponClass.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow, WeaponClass.Crossbow)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown, WeaponClass.ThrowingAxe)]
      public void SameType_ReturnsTrue(ItemObject.ItemTypeEnum type, WeaponClass weaponClass)
      {
         var current = Helpers.MakeWeapon(type, weaponClass);
         var candidate = Helpers.MakeWeapon(type, weaponClass);
         var hero = Helpers.MakeHero(slot0: current);
         Assert.True(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon, ItemObject.ItemTypeEnum.TwoHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.TwoHandedWeapon, ItemObject.ItemTypeEnum.Polearm)]
      [InlineData(ItemObject.ItemTypeEnum.Polearm, ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Bow, ItemObject.ItemTypeEnum.Crossbow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow, ItemObject.ItemTypeEnum.Thrown)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown, ItemObject.ItemTypeEnum.OneHandedWeapon)]
      public void DifferentType_ReturnsFalse(
         ItemObject.ItemTypeEnum candidateType, ItemObject.ItemTypeEnum currentType)
      {
         var current = Helpers.MakeWeapon(currentType);
         var candidate = Helpers.MakeWeapon(candidateType);
         var hero = Helpers.MakeHero(slot0: current);
         Assert.False(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void BastardSword_VsOneHandedSlot_SettingOn_ReturnsTrue()
      {
         Main.GameSettings.BastardSwordsAreOneHanded = true;
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword);
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon, WeaponClass.TwoHandedSword,
            itemUsage: "onehanded_block_rshield_swing_thrust");
         var hero = Helpers.MakeHero(slot0: current);
         Assert.True(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void BastardSword_VsOneHandedSlot_SettingOff_ReturnsFalse()
      {
         Main.GameSettings.BastardSwordsAreOneHanded = false;
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword);
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon, WeaponClass.TwoHandedSword,
            itemUsage: "onehanded_block_rshield_swing_thrust");
         var hero = Helpers.MakeHero(slot0: current);
         Assert.False(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
         Main.GameSettings.BastardSwordsAreOneHanded = true;
      }

      [Fact]
      public void BastardSword_VsTrueTwoHandedSlot_SettingOn_ReturnsFalse()
      {
         // Bastard sword remaps to OneHandedWeapon — won't match a true TwoHanded slot
         Main.GameSettings.BastardSwordsAreOneHanded = true;
         var trueTwoHander = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "twohanded_block_swing_thrust");
         var bastard = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "onehanded_block_rshield_swing_thrust");
         var hero = Helpers.MakeHero(slot0: trueTwoHander);
         Assert.False(SameTypeWeaponTemplate.Instance.IsValidFor(
            bastard, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void BastardSword_VsTrueTwoHandedSlot_SettingOff_ReturnsTrue()
      {
         Main.GameSettings.BastardSwordsAreOneHanded = false;
         var trueTwoHander = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "twohanded_block_swing_thrust");
         var bastard = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "onehanded_block_rshield_swing_thrust");
         var hero = Helpers.MakeHero(slot0: trueTwoHander);
         Assert.True(SameTypeWeaponTemplate.Instance.IsValidFor(
            bastard, EquipmentIndex.Weapon0, hero));
         Main.GameSettings.BastardSwordsAreOneHanded = true;
      }

      [Fact]
      public void OneHanded_VsBastardSwordSlot_SettingOn_ReturnsTrue()
      {
         Main.GameSettings.BastardSwordsAreOneHanded = true;
         var bastard = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "onehanded_block_rshield_swing_thrust");
         var oneHanded = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword);
         var hero = Helpers.MakeHero(slot0: bastard);
         Assert.True(SameTypeWeaponTemplate.Instance.IsValidFor(
            oneHanded, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void BastardSword_VsBastardSwordSlot_SettingOn_ReturnsTrue()
      {
         Main.GameSettings.BastardSwordsAreOneHanded = true;
         var bastard1 = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "onehanded_block_rshield_swing_thrust");
         var bastard2 = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "onehanded_block_rshield_swing");
         var hero = Helpers.MakeHero(slot0: bastard1);
         Assert.True(SameTypeWeaponTemplate.Instance.IsValidFor(
            bastard2, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyNotMet_ReturnsFalse()
      {
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword);
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 80);
         var hero = Helpers.MakeHero(oneHandedSkill: 50, slot0: current);
         Assert.False(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyMet_ReturnsTrue()
      {
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword);
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 80);
         var hero = Helpers.MakeHero(oneHandedSkill: 100, slot0: current);
         Assert.True(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void ChecksCorrectSlotIndex()
      {
         var current = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Bow, WeaponClass.Bow);
         var candidate = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Bow, WeaponClass.Bow);
         var hero = Helpers.MakeHero(slot1: current);
         Assert.False(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon0, hero));
         Assert.True(SameTypeWeaponTemplate.Instance.IsValidFor(
            candidate, EquipmentIndex.Weapon1, hero));
      }
   }
}
