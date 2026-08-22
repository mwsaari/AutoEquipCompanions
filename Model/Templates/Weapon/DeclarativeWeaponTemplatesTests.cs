using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.Bow;
using AutoEquipCompanions.Model.Templates.Weapon.Crossbow;
using AutoEquipCompanions.Model.Templates.Weapon.Polearm;
using AutoEquipCompanions.Model.Templates.Weapon.Thrown;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon
{
   // Covers the weapon templates that are pure AllowedItemTypes/ComparisonField declarations
   // with no overridden logic of their own (PolearmTemplate, BowTemplate, CrossbowTemplate,
   // RangedTemplate, AmmoTemplate, ThrownTemplate) -- one class per template, grouped in a
   // single file since each body is just the AllowedItemTypes wiring check.

   public class PolearmTemplateTests
   {
      [Fact]
      public void PolearmType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm);
         var hero = Helpers.MakeHero();
         Assert.True(PolearmTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(PolearmTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }
   }

   public class BowTemplateTests
   {
      [Fact]
      public void BowType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Bow);
         var hero = Helpers.MakeHero();
         Assert.True(BowTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow)]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(BowTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }
   }

   public class CrossbowTemplateTests
   {
      [Fact]
      public void CrossbowType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Crossbow);
         var hero = Helpers.MakeHero();
         Assert.True(CrossbowTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(CrossbowTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }
   }

   public class RangedTemplateTests
   {
      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.Crossbow)]
      public void AllowedTypes_ReturnsTrue(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.True(RangedTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      [InlineData(ItemObject.ItemTypeEnum.Thrown)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(RangedTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }
   }

   public class AmmoTemplateTests
   {
      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Arrows)]
      [InlineData(ItemObject.ItemTypeEnum.Bolts)]
      public void AllowedTypes_ReturnsTrue(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.True(AmmoTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DisallowedType_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon);
         var hero = Helpers.MakeHero();
         Assert.False(AmmoTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void GetScore_ScoresByMissileDamage()
      {
         // A weapon's missile damage is driven by its thrust-damage field, not a separate stat
         var strong = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Arrows, thrustDamage: 90);
         var weak = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Arrows, thrustDamage: 10);
         Assert.True(AmmoTemplate.Instance.GetScore(strong) > AmmoTemplate.Instance.GetScore(weak));
      }
   }

   public class ThrownTemplateTests
   {
      [Fact]
      public void ThrownType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Thrown);
         var hero = Helpers.MakeHero();
         Assert.True(ThrownTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTypeEnum.Bow)]
      [InlineData(ItemObject.ItemTypeEnum.OneHandedWeapon)]
      public void DisallowedTypes_ReturnsFalse(ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeWeapon(type);
         var hero = Helpers.MakeHero();
         Assert.False(ThrownTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void GetScore_ScoresByMissileDamage()
      {
         var strong = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Thrown, thrustDamage: 90);
         var weak = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Thrown, thrustDamage: 10);
         Assert.True(ThrownTemplate.Instance.GetScore(strong) > ThrownTemplate.Instance.GetScore(weak));
      }
   }
}
