using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Shield;
using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using AutoEquipCompanions.Model.Templates.Weapon.Polearm;
using AutoEquipCompanions.Model.Templates.Weapon.Thrown;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class InfantryCaptainTemplate : BaseCharacterTemplate
   {
      public static readonly InfantryCaptainTemplate Instance = new InfantryCaptainTemplate();

      private InfantryCaptainTemplate() : base(CreateSlots()) { }

      public override string Name => "infantry_captain";
      public override string DisplayName => "Infantry Captain";

      private static List<(EquipmentIndex, ISlotTemplate)> CreateSlots()
      {
         return new List<(EquipmentIndex, ISlotTemplate)>
         {
            (EquipmentIndex.Head,         DefaultArmorTemplate.Instance),
            (EquipmentIndex.Cape,         DefaultArmorTemplate.Instance),
            (EquipmentIndex.Body,         DefaultArmorTemplate.Instance),
            (EquipmentIndex.Gloves,       DefaultArmorTemplate.Instance),
            (EquipmentIndex.Leg,          DefaultArmorTemplate.Instance),
            (EquipmentIndex.Horse,        EmptySlotTemplate.Instance),
            (EquipmentIndex.HorseHarness, EmptySlotTemplate.Instance),
            (EquipmentIndex.Weapon0,      OneHandedWeaponTemplate.Instance),
            (EquipmentIndex.Weapon1,      DefaultShieldTemplate.Instance),
            (EquipmentIndex.Weapon2,      PolearmTemplate.Instance),
            (EquipmentIndex.Weapon3,      ThrownTemplate.Instance),
         };
      }
   }
}
