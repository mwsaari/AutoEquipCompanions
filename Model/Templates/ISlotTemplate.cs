using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates
{
   public interface ISlotTemplate
   {
      string Name { get; }
      string DisplayName { get; }
      IEnumerable<EquipmentIndex> LegalSlots { get; }
      bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero);
      double GetScore(EquipmentElement candidate);
   }

   public static class SlotTemplateExtensions
   {
      public static bool IsBetterThan(this ISlotTemplate template, EquipmentElement candidate, EquipmentElement current)
      {
         return template.GetScore(candidate) > template.GetScore(current);
      }
   }
}
