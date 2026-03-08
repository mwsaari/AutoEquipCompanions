using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templating
{
   public class HorseSlotTemplate : SlotTemplate
   {
      public enum HorseField
      {
         Value,
         Speed,
         Maneuver,
         ChargeDamage,
         HitPoints,
      }

      public HorseField ComparisonField { get; set; } = HorseField.Value;

      public bool Camel { get; set; } = false;

      public override bool DoesItemQualify(EquipmentElement equipment)
      {
         if (equipment.IsEmpty || equipment.Item.ItemType != ItemObject.ItemTypeEnum.Horse)
            return false;
         var isCamel = equipment.Item.ItemComponent is HorseComponent horse
            && horse.Monster.MonsterUsage == "camel";
         return Camel == isCamel;
      }

      public override double GetScore(EquipmentElement equipment)
      {
         if (equipment.IsEmpty)
            return 0;
         var horse = equipment.Item.HorseComponent;
         if (horse == null)
            return 0;
         switch (ComparisonField)
         {
            case HorseField.Speed:
               return horse.Speed;
            case HorseField.Maneuver:
               return horse.Maneuver;
            case HorseField.ChargeDamage:
               return horse.ChargeDamage;
            case HorseField.HitPoints:
               return horse.HitPoints;
            default:
               return equipment.ItemValue;
         }
      }
   }
}
