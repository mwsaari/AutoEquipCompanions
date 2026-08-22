using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon
{
   public class OneHandedWeaponTemplateTests
   {
      [Fact]
      public void OneHandedType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword);
         var hero = Helpers.MakeHero();
         Assert.True(OneHandedWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Polearm)]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(OneHandedWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(OneHandedWeaponTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyNotMet_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 80);
         var hero = Helpers.MakeHero(oneHandedSkill: 20);
         Assert.False(OneHandedWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      // ── Bastard sword exception ─────────────────────────────────────────────

      [Theory]
      [InlineData("onehanded_block_rshield_swing_thrust")]
      [InlineData("onehanded_block_rshield_swing")]
      [InlineData("onehanded_rshield_axe")]
      [InlineData("onehanded_polearm_block_long_rshield_thrust")]
      [InlineData("onehanded_polearm_block_rshield_thrust")]
      public void TwoHandedBastardSword_SettingOn_ReturnsTrue(string usage)
      {
         Main.GameSettings.BastardSwordsAreOneHanded = true;
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: usage);
         var hero = Helpers.MakeHero();
         Assert.True(OneHandedWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void TwoHandedNonBastardSword_SettingOn_ReturnsFalse()
      {
         Main.GameSettings.BastardSwordsAreOneHanded = true;
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "twohanded_block_swing_thrust");
         var hero = Helpers.MakeHero();
         Assert.False(OneHandedWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void TwoHandedBastardSword_SettingOff_ReturnsFalse()
      {
         Main.GameSettings.BastardSwordsAreOneHanded = false;
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, itemUsage: "onehanded_block_rshield_swing_thrust");
         var hero = Helpers.MakeHero();
         Assert.False(OneHandedWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
         Main.GameSettings.BastardSwordsAreOneHanded = true;
      }
   }
}
