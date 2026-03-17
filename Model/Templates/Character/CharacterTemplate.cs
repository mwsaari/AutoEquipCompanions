using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Mount;
using AutoEquipCompanions.Model.Templates.Weapon;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class CharacterTemplate : BaseCharacterTemplate
   {
      public static readonly CharacterTemplate Instance = new CharacterTemplate();

      private CharacterTemplate() : base(CreateSlots()) { }

      public override string Name => "default";
      public override string DisplayName => "Adaptive";

      private static List<(EquipmentIndex, ISlotTemplate)> CreateSlots()
      {
         return new List<(EquipmentIndex, ISlotTemplate)>
         {
            (EquipmentIndex.Head, DefaultArmorTemplate.Instance),
            (EquipmentIndex.Cape, DefaultArmorTemplate.Instance),
            (EquipmentIndex.Body, DefaultArmorTemplate.Instance),
            (EquipmentIndex.Gloves, DefaultArmorTemplate.Instance),
            (EquipmentIndex.Leg, DefaultArmorTemplate.Instance),
            (EquipmentIndex.Horse, DefaultMountTemplate.Instance),
            (EquipmentIndex.HorseHarness, DefaultMountTemplate.Instance),
            (EquipmentIndex.Weapon0, SameTypeWeaponTemplate.Instance),
            (EquipmentIndex.Weapon1, SameTypeWeaponTemplate.Instance),
            (EquipmentIndex.Weapon2, SameTypeWeaponTemplate.Instance),
            (EquipmentIndex.Weapon3, SameTypeWeaponTemplate.Instance)
         };
      }
   }
}
