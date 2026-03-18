using AutoEquipCompanions.Model.Templates.Mount;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Mount
{
   public class BaseMountTemplateTests
   {
      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(default, EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void HarnessCandidate_InHorseSlot_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.HorseHarness;
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(
            new EquipmentElement(item), EquipmentIndex.Horse, hero));
      }

      [Fact]
      public void HorseCandidate_InHarnessSlot_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.Horse;
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(
            new EquipmentElement(item), EquipmentIndex.HorseHarness, hero));
      }

      [Fact]
      public void InvalidSlot_ReturnsFalse()
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.Horse;
         var hero = Helpers.MakeHero();
         Assert.False(DefaultMountTemplate.Instance.IsValidFor(
            new EquipmentElement(item), EquipmentIndex.Weapon0, hero));
      }

   }
}
