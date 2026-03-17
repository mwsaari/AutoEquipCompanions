using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Shield;
using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.Crossbow;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class CrossbowCaptainTemplate : BaseCharacterTemplate
   {
      public static readonly CrossbowCaptainTemplate Instance = new CrossbowCaptainTemplate();

      private CrossbowCaptainTemplate() : base(CreateSlots()) { }

      public override string Name => "crossbow_captain";
      public override string DisplayName => "Crossbow Captain";

      private static List<(EquipmentIndex, ISlotTemplate)> CreateSlots()
      {
         return new List<(EquipmentIndex, ISlotTemplate)>
         {
            (EquipmentIndex.Head,         MediumArmorTemplate.Instance),
            (EquipmentIndex.Cape,         MediumArmorTemplate.Instance),
            (EquipmentIndex.Body,         MediumArmorTemplate.Instance),
            (EquipmentIndex.Gloves,       MediumArmorTemplate.Instance),
            (EquipmentIndex.Leg,          MediumArmorTemplate.Instance),
            (EquipmentIndex.Horse,        EmptySlotTemplate.Instance),
            (EquipmentIndex.HorseHarness, EmptySlotTemplate.Instance),
            (EquipmentIndex.Weapon0,      CrossbowTemplate.Instance),
            (EquipmentIndex.Weapon1,      BoltsTemplate.Instance),
            (EquipmentIndex.Weapon2,      DefaultShieldTemplate.Instance),
            (EquipmentIndex.Weapon3,      OneHandedWeaponTemplate.Instance),
         };
      }
   }
}
