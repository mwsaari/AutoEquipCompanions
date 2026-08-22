using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Character;
using AutoEquipCompanions.Model.Templates.Mount;
using AutoEquipCompanions.Model.Templates.Shield;
using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.Bow;
using AutoEquipCompanions.Model.Templates.Weapon.Crossbow;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using AutoEquipCompanions.Model.Templates.Weapon.Polearm;
using AutoEquipCompanions.Model.Templates.Weapon.Thrown;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Character
{
   // Each captain template wires a specific ISlotTemplate to each EquipmentIndex; a copy-paste
   // slip here (e.g. reusing another captain's armor weight) would otherwise go unnoticed.
   public class CaptainTemplateSlotsTests
   {
      [Fact]
      public void InfantryCaptain_AssignsExpectedSlotTemplates()
      {
         AssertSlots(InfantryCaptainTemplate.Instance, new Dictionary<EquipmentIndex, ISlotTemplate>
         {
            [EquipmentIndex.Head] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Cape] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Body] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Gloves] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Leg] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Horse] = EmptySlotTemplate.Instance,
            [EquipmentIndex.HorseHarness] = EmptySlotTemplate.Instance,
            [EquipmentIndex.Weapon0] = OneHandedWeaponTemplate.Instance,
            [EquipmentIndex.Weapon1] = DefaultShieldTemplate.Instance,
            [EquipmentIndex.Weapon2] = PolearmTemplate.Instance,
            [EquipmentIndex.Weapon3] = ThrownTemplate.Instance,
         });
      }

      [Fact]
      public void CavalryCaptain_AssignsExpectedSlotTemplates()
      {
         AssertSlots(CavalryCaptainTemplate.Instance, new Dictionary<EquipmentIndex, ISlotTemplate>
         {
            [EquipmentIndex.Head] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Cape] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Body] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Gloves] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Leg] = HeavyArmorTemplate.Instance,
            [EquipmentIndex.Horse] = DefaultMountTemplate.Instance,
            [EquipmentIndex.HorseHarness] = DefaultMountTemplate.Instance,
            [EquipmentIndex.Weapon0] = MountWeaponTemplate.Instance,
            [EquipmentIndex.Weapon1] = OneHandedWeaponTemplate.Instance,
            [EquipmentIndex.Weapon2] = DefaultShieldTemplate.Instance,
            [EquipmentIndex.Weapon3] = ThrownTemplate.Instance,
         });
      }

      [Fact]
      public void BowCaptain_AssignsExpectedSlotTemplates()
      {
         AssertSlots(BowCaptainTemplate.Instance, new Dictionary<EquipmentIndex, ISlotTemplate>
         {
            [EquipmentIndex.Head] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Cape] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Body] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Gloves] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Leg] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Horse] = EmptySlotTemplate.Instance,
            [EquipmentIndex.HorseHarness] = EmptySlotTemplate.Instance,
            [EquipmentIndex.Weapon0] = BowTemplate.Instance,
            [EquipmentIndex.Weapon1] = ArrowsTemplate.Instance,
            [EquipmentIndex.Weapon2] = DefaultShieldTemplate.Instance,
            [EquipmentIndex.Weapon3] = OneHandedWeaponTemplate.Instance,
         });
      }

      [Fact]
      public void CrossbowCaptain_AssignsExpectedSlotTemplates()
      {
         AssertSlots(CrossbowCaptainTemplate.Instance, new Dictionary<EquipmentIndex, ISlotTemplate>
         {
            [EquipmentIndex.Head] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Cape] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Body] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Gloves] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Leg] = MediumArmorTemplate.Instance,
            [EquipmentIndex.Horse] = EmptySlotTemplate.Instance,
            [EquipmentIndex.HorseHarness] = EmptySlotTemplate.Instance,
            [EquipmentIndex.Weapon0] = CrossbowTemplate.Instance,
            [EquipmentIndex.Weapon1] = BoltsTemplate.Instance,
            [EquipmentIndex.Weapon2] = DefaultShieldTemplate.Instance,
            [EquipmentIndex.Weapon3] = OneHandedWeaponTemplate.Instance,
         });
      }

      [Fact]
      public void HorseArcher_AssignsExpectedSlotTemplates()
      {
         AssertSlots(HorseArcherTemplate.Instance, new Dictionary<EquipmentIndex, ISlotTemplate>
         {
            [EquipmentIndex.Head] = LightArmorTemplate.Instance,
            [EquipmentIndex.Cape] = LightArmorTemplate.Instance,
            [EquipmentIndex.Body] = LightArmorTemplate.Instance,
            [EquipmentIndex.Gloves] = LightArmorTemplate.Instance,
            [EquipmentIndex.Leg] = LightArmorTemplate.Instance,
            [EquipmentIndex.Horse] = LightMountTemplate.Instance,
            [EquipmentIndex.HorseHarness] = LightMountTemplate.Instance,
            [EquipmentIndex.Weapon0] = BowTemplate.Instance,
            [EquipmentIndex.Weapon1] = ArrowsTemplate.Instance,
            [EquipmentIndex.Weapon2] = OneHandedWeaponTemplate.Instance,
            [EquipmentIndex.Weapon3] = ArrowsTemplate.Instance,
         });
      }

      private static void AssertSlots(ICharacterTemplate template, Dictionary<EquipmentIndex, ISlotTemplate> expected)
      {
         var actual = template.Slots.ToDictionary(x => x.Slot, x => x.Template);
         Assert.Equal(expected.Keys.OrderBy(k => k), actual.Keys.OrderBy(k => k));
         foreach (var slot in expected.Keys)
            Assert.Same(expected[slot], actual[slot]);
      }
   }
}
