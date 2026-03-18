using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test
{
   public class EquipmentIntegrationTests
   {
      [Fact]
      public void Equipment_SetAndGet_ByEquipmentIndex_PreservesItem()
      {
         var eq = new Equipment();
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon);
         eq[EquipmentIndex.Weapon0] = el;
         Assert.Same(el.Item, eq[EquipmentIndex.Weapon0].Item);
      }

      [Fact]
      public void Equipment_SetAndGet_ByIntIndex_PreservesItem()
      {
         var eq = new Equipment();
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm);
         eq[(int)EquipmentIndex.Weapon2] = el;
         Assert.Same(el.Item, eq[(int)EquipmentIndex.Weapon2].Item);
      }

      [Fact]
      public void Equipment_SetByEnum_GetByInt_SameResult()
      {
         var eq = new Equipment();
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Bow);
         eq[EquipmentIndex.Weapon1] = el;
         Assert.Same(el.Item, eq[(int)EquipmentIndex.Weapon1].Item);
      }

      [Fact]
      public void Equipment_DefaultSlot_IsEmpty()
      {
         var eq = new Equipment();
         Assert.True(eq[EquipmentIndex.Weapon0].IsEmpty);
      }

      [Fact]
      public void Equipment_AllFourWeaponSlots_IndependentlyAddressable()
      {
         var eq = new Equipment();
         var el0 = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon);
         var el1 = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Bow);
         var el2 = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm);
         var el3 = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Thrown);
         eq[EquipmentIndex.Weapon0] = el0;
         eq[EquipmentIndex.Weapon1] = el1;
         eq[EquipmentIndex.Weapon2] = el2;
         eq[EquipmentIndex.Weapon3] = el3;
         Assert.Same(el0.Item, eq[EquipmentIndex.Weapon0].Item);
         Assert.Same(el1.Item, eq[EquipmentIndex.Weapon1].Item);
         Assert.Same(el2.Item, eq[EquipmentIndex.Weapon2].Item);
         Assert.Same(el3.Item, eq[EquipmentIndex.Weapon3].Item);
      }

      [Fact]
      public void Hero_BattleEquipment_ReflectionSet_ReturnsCorrectSlot()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon);
         var hero = Helpers.MakeHero(slot0: el);
         Assert.Same(el.Item, hero.BattleEquipment[EquipmentIndex.Weapon0].Item);
      }

      [Fact]
      public void Hero_SetSkillValue_GetSkillValue_Roundtrip()
      {
         var hero = new Hero();
         hero.SetSkillValue(DefaultSkills.OneHanded, 150);
         Assert.Equal(150, hero.GetSkillValue(DefaultSkills.OneHanded));
      }

      [Fact]
      public void Hero_MultipleSkills_AreIndependent()
      {
         var hero = new Hero();
         hero.SetSkillValue(DefaultSkills.OneHanded, 100);
         hero.SetSkillValue(DefaultSkills.TwoHanded, 200);
         hero.SetSkillValue(DefaultSkills.Polearm, 50);
         Assert.Equal(100, hero.GetSkillValue(DefaultSkills.OneHanded));
         Assert.Equal(200, hero.GetSkillValue(DefaultSkills.TwoHanded));
         Assert.Equal(50, hero.GetSkillValue(DefaultSkills.Polearm));
      }

      [Fact]
      public void Hero_DefaultSkillValue_IsZero()
      {
         var hero = new Hero();
         Assert.Equal(0, hero.GetSkillValue(DefaultSkills.OneHanded));
      }
   }
}
