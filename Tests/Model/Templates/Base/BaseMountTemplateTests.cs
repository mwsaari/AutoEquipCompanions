using System.Linq;
using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Base;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Base
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

      [Fact]
      public void LegalSlots_ContainsHorseAndHarness()
      {
         var slots = DefaultMountTemplate.Instance.LegalSlots.ToList();
         Assert.Contains(EquipmentIndex.Horse, slots);
         Assert.Contains(EquipmentIndex.HorseHarness, slots);
         Assert.Equal(2, slots.Count);
      }

      [Fact]
      public void DefaultMountTemplate_UseCamel_IsFalse()
      {
         Assert.False(DefaultMountTemplate.Instance.UseCamel);
      }

      [Fact]
      public void CamelMountTemplate_UseCamel_IsTrue()
      {
         Assert.True(CamelMountTemplate.Instance.UseCamel);
      }

      [Fact]
      public void DefaultMountTemplate_HorseComparisonField_IsValue()
      {
         Assert.Equal(BaseMountTemplate.HorseField.Value, DefaultMountTemplate.Instance.HorseComparisonField);
      }

      [Fact]
      public void DefaultMountTemplate_HarnessComparisonField_IsValue()
      {
         Assert.Equal(BaseMountTemplate.HarnessField.Value, DefaultMountTemplate.Instance.HarnessComparisonField);
      }
   }
}
