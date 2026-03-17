using AutoEquipCompanions.Model.Templates.Armor;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Armor
{
   public class BaseArmorTemplateTests
   {
      [Theory]
      [InlineData(EquipmentIndex.Head, ItemObject.ItemTypeEnum.HeadArmor)]
      [InlineData(EquipmentIndex.Cape, ItemObject.ItemTypeEnum.Cape)]
      [InlineData(EquipmentIndex.Body, ItemObject.ItemTypeEnum.BodyArmor)]
      [InlineData(EquipmentIndex.Gloves, ItemObject.ItemTypeEnum.HandArmor)]
      [InlineData(EquipmentIndex.Leg, ItemObject.ItemTypeEnum.LegArmor)]
      public void CorrectSlotAndType_ReturnsTrue(EquipmentIndex slot, ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeArmor(type);
         var hero = Helpers.MakeHero();
         Assert.True(DefaultArmorTemplate.Instance.IsValidFor(el, slot, hero));
      }

      [Theory]
      [InlineData(EquipmentIndex.Head, ItemObject.ItemTypeEnum.BodyArmor)]
      [InlineData(EquipmentIndex.Body, ItemObject.ItemTypeEnum.HeadArmor)]
      [InlineData(EquipmentIndex.Cape, ItemObject.ItemTypeEnum.LegArmor)]
      [InlineData(EquipmentIndex.Gloves, ItemObject.ItemTypeEnum.Cape)]
      [InlineData(EquipmentIndex.Leg, ItemObject.ItemTypeEnum.HandArmor)]
      public void WrongTypeForSlot_ReturnsFalse(EquipmentIndex slot, ItemObject.ItemTypeEnum type)
      {
         var el = Helpers.MakeArmor(type);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, slot, hero));
      }

      [Fact]
      public void WeaponSlot_ReturnsFalse()
      {
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void HorseSlot_ReturnsFalse()
      {
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(default, EquipmentIndex.Head, hero));
      }

      [Fact]
      public void NoArmorComponent_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.HeadArmor;
         var el = new EquipmentElement(item);
         var hero = Helpers.MakeHero();
         Assert.False(DefaultArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

   }
}
