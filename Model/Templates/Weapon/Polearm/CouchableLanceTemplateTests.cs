using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.Polearm;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon
{
   public class CouchableLanceTemplateTests
   {
      [Fact]
      public void CouchInDescriptionId_ReturnsTrue()
      {
         // itemUsage left empty — avoids native MBItem.GetItemUsageSetFlags call
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm,
            descriptionId: "mp_couch_lance");
         var hero = Helpers.MakeHero();
         Assert.True(CouchableLanceTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void NoCouchInDescriptionId_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm,
            descriptionId: "mp_lance");
         var hero = Helpers.MakeHero();
         Assert.False(CouchableLanceTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void CouchIsCaseInsensitive_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm,
            descriptionId: "COUCH_LANCE");
         var hero = Helpers.MakeHero();
         Assert.True(CouchableLanceTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyDescriptionId_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm,
            descriptionId: "");
         var hero = Helpers.MakeHero();
         Assert.False(CouchableLanceTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(CouchableLanceTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DisallowedItemType_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.Shield;
         var el = new EquipmentElement(item);
         var hero = Helpers.MakeHero();
         Assert.False(CouchableLanceTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyNotMet_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm,
            descriptionId: "couch_lance", difficulty: 80);
         var hero = Helpers.MakeHero(polearmSkill: 20);
         Assert.False(CouchableLanceTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void DifficultyMet_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm,
            descriptionId: "couch_lance", difficulty: 80);
         var hero = Helpers.MakeHero(polearmSkill: 100);
         Assert.True(CouchableLanceTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }
   }
}
