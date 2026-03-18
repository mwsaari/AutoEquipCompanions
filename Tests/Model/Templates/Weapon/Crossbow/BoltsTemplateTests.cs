using AutoEquipCompanions.Model.Templates.Weapon.Crossbow;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon.Crossbow
{
   public class BoltsTemplateTests
   {
      [Fact]
      public void BoltsType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Bolts);
         var hero = Helpers.MakeHero();
         Assert.True(BoltsTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Arrows)]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow)]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(BoltsTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(BoltsTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

   }
}
