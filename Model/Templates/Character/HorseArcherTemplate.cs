using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Mount;
using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.Bow;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class HorseArcherTemplate : BaseCharacterTemplate
   {
      public static readonly HorseArcherTemplate Instance = new HorseArcherTemplate();

      private HorseArcherTemplate() : base(CreateSlots()) { }

      public override string Name => "horse_archer";
      public override string DisplayName => "Horse Archer";

      private static List<(EquipmentIndex, ISlotTemplate)> CreateSlots()
      {
         return new List<(EquipmentIndex, ISlotTemplate)>
         {
            (EquipmentIndex.Head,         LightArmorTemplate.Instance),
            (EquipmentIndex.Cape,         LightArmorTemplate.Instance),
            (EquipmentIndex.Body,         LightArmorTemplate.Instance),
            (EquipmentIndex.Gloves,       LightArmorTemplate.Instance),
            (EquipmentIndex.Leg,          LightArmorTemplate.Instance),
            (EquipmentIndex.Horse,        LightMountTemplate.Instance),
            (EquipmentIndex.HorseHarness, LightMountTemplate.Instance),
            (EquipmentIndex.Weapon0,      BowTemplate.Instance),
            (EquipmentIndex.Weapon1,      ArrowsTemplate.Instance),
            (EquipmentIndex.Weapon2,      OneHandedWeaponTemplate.Instance),
            (EquipmentIndex.Weapon3,      ArrowsTemplate.Instance),
         };
      }
   }
}
