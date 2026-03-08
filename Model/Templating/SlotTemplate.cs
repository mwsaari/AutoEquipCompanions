using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templating
{
   public abstract class SlotTemplate
   {
      public bool IsEnabled { get; set; } = true;

      public abstract bool DoesItemQualify(EquipmentElement equipment);

      public abstract double GetScore(EquipmentElement equipment);

      public bool IsBetterThan(EquipmentElement candidate, EquipmentElement current)
      {
         return GetScore(candidate) > GetScore(current);
      }
   }
}
