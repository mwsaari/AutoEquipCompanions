using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templating
{
   public class ArmorSlotTemplate : SlotTemplate
   {
      public enum ArmorField
      {
         Value,
         ArmorTotal,
         HeadArmor,
         BodyArmor,
         ArmArmor,
         LegArmor,
      }

      public ArmorField ComparisonField { get; set; } = ArmorField.Value;

      public override bool DoesItemQualify(EquipmentElement equipment)
      {
         return !equipment.IsEmpty
            && equipment.Item.HasArmorComponent
            && equipment.Item.ItemType != ItemObject.ItemTypeEnum.HorseHarness;
      }

      public override double GetScore(EquipmentElement equipment)
      {
         if (equipment.IsEmpty)
            return 0;
         switch (ComparisonField)
         {
            case ArmorField.ArmorTotal:
               return equipment.GetModifiedHeadArmor() + equipment.GetModifiedBodyArmor()
                  + equipment.GetModifiedArmArmor() + equipment.GetModifiedLegArmor();
            case ArmorField.HeadArmor:
               return equipment.GetModifiedHeadArmor();
            case ArmorField.BodyArmor:
               return equipment.GetModifiedBodyArmor();
            case ArmorField.ArmArmor:
               return equipment.GetModifiedArmArmor();
            case ArmorField.LegArmor:
               return equipment.GetModifiedLegArmor();
            default:
               return equipment.ItemValue;
         }
      }
   }
}
