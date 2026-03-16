using System.Linq;
using AutoEquipCompanions.Model.Templates;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates
{
   public class CharacterTemplateTests
   {
      [Fact]
      public void Name_IsDefault()
      {
         Assert.Equal("Default", CharacterTemplate.Instance.Name);
      }

      [Fact]
      public void Slots_ContainsElevenEntries()
      {
         Assert.Equal(11, CharacterTemplate.Instance.Slots.Count());
      }

      [Theory]
      [InlineData(EquipmentIndex.Head)]
      [InlineData(EquipmentIndex.Cape)]
      [InlineData(EquipmentIndex.Body)]
      [InlineData(EquipmentIndex.Gloves)]
      [InlineData(EquipmentIndex.Leg)]
      public void ArmorSlots_UseDefaultArmorTemplate(EquipmentIndex slot)
      {
         var template = CharacterTemplate.Instance.Slots.First(x => x.Slot == slot).Template;
         Assert.Same(DefaultArmorTemplate.Instance, template);
      }

      [Theory]
      [InlineData(EquipmentIndex.Horse)]
      [InlineData(EquipmentIndex.HorseHarness)]
      public void MountSlots_UseDefaultMountTemplate(EquipmentIndex slot)
      {
         var template = CharacterTemplate.Instance.Slots.First(x => x.Slot == slot).Template;
         Assert.Same(DefaultMountTemplate.Instance, template);
      }

      [Theory]
      [InlineData(EquipmentIndex.Weapon0)]
      [InlineData(EquipmentIndex.Weapon1)]
      [InlineData(EquipmentIndex.Weapon2)]
      [InlineData(EquipmentIndex.Weapon3)]
      public void WeaponSlots_UseSameTypeWeaponTemplate(EquipmentIndex slot)
      {
         var template = CharacterTemplate.Instance.Slots.First(x => x.Slot == slot).Template;
         Assert.Same(SameTypeWeaponTemplate.Instance, template);
      }

      [Fact]
      public void AllSlotIndices_AreUnique()
      {
         var slots = CharacterTemplate.Instance.Slots.Select(x => x.Slot).ToList();
         Assert.Equal(slots.Count, slots.Distinct().Count());
      }
   }
}
