using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class DisabledCharacterTemplate : BaseCharacterTemplate
   {
      public static readonly DisabledCharacterTemplate Instance = new DisabledCharacterTemplate();

      public override string Name => "disabled";
      public override string DisplayName => "Disabled";
      public override bool DefaultEnabled => false;

      private DisabledCharacterTemplate() : base(new List<(EquipmentIndex, ISlotTemplate)>
      {
         (EquipmentIndex.Head, EmptySlotTemplate.Instance),
         (EquipmentIndex.Cape, EmptySlotTemplate.Instance),
         (EquipmentIndex.Body, EmptySlotTemplate.Instance),
         (EquipmentIndex.Gloves, EmptySlotTemplate.Instance),
         (EquipmentIndex.Leg, EmptySlotTemplate.Instance),
         (EquipmentIndex.Horse, EmptySlotTemplate.Instance),
         (EquipmentIndex.HorseHarness, EmptySlotTemplate.Instance),
         (EquipmentIndex.Weapon0, EmptySlotTemplate.Instance),
         (EquipmentIndex.Weapon1, EmptySlotTemplate.Instance),
         (EquipmentIndex.Weapon2, EmptySlotTemplate.Instance),
         (EquipmentIndex.Weapon3, EmptySlotTemplate.Instance),
      }) { }
   }
}
