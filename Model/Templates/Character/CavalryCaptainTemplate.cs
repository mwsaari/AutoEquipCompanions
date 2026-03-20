using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Mount;
using AutoEquipCompanions.Model.Templates.Shield;
using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using AutoEquipCompanions.Model.Templates.Weapon.Polearm;
using AutoEquipCompanions.Model.Templates.Weapon.Thrown;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class CavalryCaptainTemplate : BaseCharacterTemplate
   {
      public static readonly CavalryCaptainTemplate Instance = new CavalryCaptainTemplate();

      private CavalryCaptainTemplate() : base(CreateSlots()) { }

      public override string Name => "cavalry_captain";
      public override string DisplayName => "Cavalry Captain";

      private static List<(EquipmentIndex, ISlotTemplate)> CreateSlots()
      {
         return new List<(EquipmentIndex, ISlotTemplate)>
         {
            (EquipmentIndex.Head,         DefaultArmorTemplate.Instance),
            (EquipmentIndex.Cape,         DefaultArmorTemplate.Instance),
            (EquipmentIndex.Body,         DefaultArmorTemplate.Instance),
            (EquipmentIndex.Gloves,       DefaultArmorTemplate.Instance),
            (EquipmentIndex.Leg,          DefaultArmorTemplate.Instance),
            (EquipmentIndex.Horse,        DefaultMountTemplate.Instance),
            (EquipmentIndex.HorseHarness, DefaultMountTemplate.Instance),
            (EquipmentIndex.Weapon0,      MountWeaponTemplate.Instance),
            (EquipmentIndex.Weapon1,      OneHandedWeaponTemplate.Instance),
            (EquipmentIndex.Weapon2,      DefaultShieldTemplate.Instance),
            (EquipmentIndex.Weapon3,      ThrownTemplate.Instance),
         };
      }
   }
}
