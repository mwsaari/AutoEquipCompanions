using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions.Model.Templating
{
   public class WeaponSlotTemplate : SlotTemplate
   {
      public enum WeaponField
      {
         Value,
         ThrustDamage,
         SwingDamage,
         MissileDamage,
         HighestDamage,
         ThrustSpeed,
         SwingSpeed,
         MissileSpeed,
         Handling,
         StackCount,
      }

      private static readonly ItemObject.ItemTypeEnum[] AllowedItemTypes =
      {
         ItemObject.ItemTypeEnum.OneHandedWeapon,
         ItemObject.ItemTypeEnum.TwoHandedWeapon,
         ItemObject.ItemTypeEnum.Polearm,
         ItemObject.ItemTypeEnum.Thrown,
         ItemObject.ItemTypeEnum.Shield,
         ItemObject.ItemTypeEnum.Bow,
         ItemObject.ItemTypeEnum.Crossbow,
         ItemObject.ItemTypeEnum.Arrows,
         ItemObject.ItemTypeEnum.Bolts,
      };

      public WeaponField ComparisonField { get; set; } = WeaponField.Value;

      /// <summary>Null means any weapon type. Must be one of the values in AllowedItemTypes.</summary>
      public ItemObject.ItemTypeEnum? ItemType { get; set; } = null;

      public ItemObject.ItemUsageSetFlags? RequiredUsageFlags { get; set; }

      public override bool DoesItemQualify(EquipmentElement equipment)
      {
         if (equipment.IsEmpty || !equipment.Item.HasWeaponComponent)
            return false;
         if (ItemType.HasValue)
            return equipment.Item.ItemType == ItemType.Value;
         if (!IsAllowedItemType(equipment.Item.ItemType))
            return false;
         if (RequiredUsageFlags.HasValue)
            return MBItem.GetItemUsageSetFlags(equipment.Item.PrimaryWeapon.ItemUsage).HasFlag(RequiredUsageFlags.Value);
         return true;
      }

      private static bool IsAllowedItemType(ItemObject.ItemTypeEnum itemType)
      {
         foreach (var allowed in AllowedItemTypes)
            if (itemType == allowed)
               return true;
         return false;
      }

      public override double GetScore(EquipmentElement equipment)
      {
         if (equipment.IsEmpty)
            return 0;
         switch (ComparisonField)
         {
            case WeaponField.ThrustDamage:
               return equipment.GetModifiedThrustDamageForUsage(0);
            case WeaponField.SwingDamage:
               return equipment.GetModifiedSwingDamageForUsage(0);
            case WeaponField.MissileDamage:
               return equipment.GetModifiedMissileDamageForUsage(0);
            case WeaponField.HighestDamage:
               return Math.Max(
                  Math.Max(equipment.GetModifiedThrustDamageForUsage(0), equipment.GetModifiedSwingDamageForUsage(0)),
                  equipment.GetModifiedMissileDamageForUsage(0));
            case WeaponField.ThrustSpeed:
               return equipment.GetModifiedThrustSpeedForUsage(0);
            case WeaponField.SwingSpeed:
               return equipment.GetModifiedSwingSpeedForUsage(0);
            case WeaponField.MissileSpeed:
               return equipment.GetModifiedMissileSpeedForUsage(0);
            case WeaponField.Handling:
               return equipment.GetModifiedHandlingForUsage(0);
            case WeaponField.StackCount:
               return equipment.GetModifiedStackCountForUsage(0);
            default:
               return equipment.ItemValue;
         }
      }
   }
}
