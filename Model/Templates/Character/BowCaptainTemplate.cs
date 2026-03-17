using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Shield;
using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.Bow;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class BowCaptainTemplate : BaseCharacterTemplate
   {
      public static readonly BowCaptainTemplate Instance = new BowCaptainTemplate();

      private BowCaptainTemplate() : base(CreateSlots()) { }

      public override string Name => "bow_captain";
      public override string DisplayName => "Bow Captain";

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
            (EquipmentIndex.Weapon0,      BowTemplate.Instance),
            (EquipmentIndex.Weapon1,      ArrowsTemplate.Instance),
            (EquipmentIndex.Weapon2,      DefaultShieldTemplate.Instance),
            (EquipmentIndex.Weapon3,      OneHandedWeaponTemplate.Instance),
         };
      }
   }
}
