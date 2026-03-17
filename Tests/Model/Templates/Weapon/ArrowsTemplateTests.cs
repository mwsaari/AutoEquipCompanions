using AutoEquipCompanions.Model.Templates.Weapon.Bow;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon
{
   public class ArrowsTemplateTests
   {
      [Fact]
      public void ArrowsType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Arrows);
         var hero = Helpers.MakeHero();
         Assert.True(ArrowsTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Bolts)]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow)]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(ArrowsTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(ArrowsTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

   }
}
