using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates
{
   public class EmptySlotTemplate : ISlotTemplate
   {
      public static readonly EmptySlotTemplate Instance = new EmptySlotTemplate();

      public string Name => "empty";
      public string DisplayName => "Empty";
      public IEnumerable<EquipmentIndex> LegalSlots => new[]
      {
         EquipmentIndex.Head,
         EquipmentIndex.Cape,
         EquipmentIndex.Body,
         EquipmentIndex.Gloves,
         EquipmentIndex.Leg,
         EquipmentIndex.Horse,
         EquipmentIndex.HorseHarness,
         EquipmentIndex.Weapon0,
         EquipmentIndex.Weapon1,
         EquipmentIndex.Weapon2,
         EquipmentIndex.Weapon3,
      };

      public bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero) => false;
      public double GetScore(EquipmentElement candidate) => 0;
   }
}
