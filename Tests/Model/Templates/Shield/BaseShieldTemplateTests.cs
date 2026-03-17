using AutoEquipCompanions.Model.Templates.Shield;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Shield
{
   public class BaseShieldTemplateTests
   {
      [Fact]
      public void ValidShield_ZeroDifficulty_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Shield, WeaponClass.SmallShield);
         var hero = Helpers.MakeHero();
         Assert.True(DefaultShieldTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void NotShieldType_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, WeaponClass.OneHandedSword);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultShieldTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(DefaultShieldTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyMet_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Shield, WeaponClass.SmallShield, difficulty: 50);
         var hero = Helpers.MakeHero(oneHandedSkill: 100);
         Assert.True(DefaultShieldTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyNotMet_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Shield, WeaponClass.SmallShield, difficulty: 50);
         var hero = Helpers.MakeHero(oneHandedSkill: 20);
         Assert.False(DefaultShieldTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

   }
}
