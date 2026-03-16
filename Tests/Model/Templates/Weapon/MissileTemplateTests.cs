using AutoEquipCompanions.Model.Templates.Weapon;
using System.Linq;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon
{
   public class MissileTemplateTests
   {
      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Arrows)]
      [InlineData(ItemObject.ItemTypeEnum.Bolts)]
      public void AllowedTypes_ReturnsTrue(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.True(MissileTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(MissileTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(MissileTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void ComparisonField_IsMissileDamage()
      {
         Assert.Equal(BaseWeaponTemplate.WeaponField.MissileDamage, MissileTemplate.Instance.ComparisonField);
      }

      [Fact]
      public void AllowedItemTypes_ContainsTwoTypes()
      {
         Assert.Equal(2, MissileTemplate.Instance.AllowedItemTypes.Count());
      }
   }
}
