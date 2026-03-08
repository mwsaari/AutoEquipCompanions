using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templating
{
   public class HorseHarnessSlotTemplate : SlotTemplate
   {
      public enum HarnessField
      {
         Value,
         MountBodyArmor,
      }

      public HarnessField ComparisonField { get; set; } = HarnessField.Value;

      /// <summary>
      /// When true, only qualifies harness for camels. When false, only qualifies harness for horses.
      /// Matched against the harness ArmorComponent.FamilyType at equip time.
      /// </summary>
      public bool Camel { get; set; } = false;

      public override bool DoesItemQualify(EquipmentElement equipment)
      {
         return !equipment.IsEmpty
            && equipment.Item.ItemType == ItemObject.ItemTypeEnum.HorseHarness;
      }

      public override double GetScore(EquipmentElement equipment)
      {
         if (equipment.IsEmpty)
            return 0;
         switch (ComparisonField)
         {
            case HarnessField.MountBodyArmor:
               return equipment.GetModifiedMountBodyArmor();
            default:
               return equipment.ItemValue;
         }
      }
   }
}
