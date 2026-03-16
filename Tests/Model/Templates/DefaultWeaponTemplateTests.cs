using System.Linq;
using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Base;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates
{
   public class DefaultWeaponTemplateTests
   {
      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.TwoHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.Polearm)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown)]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow)]
      public void AllowedTypes_ReturnsTrue(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.True(DefaultWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Arrows)]
      [InlineData(ItemObject.ItemTypeEnum.Bolts)]
      [InlineData(ItemObject.ItemTypeEnum.Shield)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var item = new ItemObject();
         item.Type = type;
         var hero = Helpers.MakeHero();
         Assert.False(DefaultWeaponTemplate.Instance.IsValidFor(
            new EquipmentElement(item), EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(DefaultWeaponTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyNotMet_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 80);
         var hero = Helpers.MakeHero(oneHandedSkill: 20);
         Assert.False(DefaultWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void ComparisonField_IsValue()
      {
         Assert.Equal(BaseWeaponTemplate.WeaponField.Value, DefaultWeaponTemplate.Instance.ComparisonField);
      }

      [Fact]
      public void AllowedItemTypes_ContainsSixTypes()
      {
         Assert.Equal(6, DefaultWeaponTemplate.Instance.AllowedItemTypes.Count());
      }
   }
}
