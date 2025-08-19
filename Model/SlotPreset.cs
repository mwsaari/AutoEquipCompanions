using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions.Model
{
    public class SlotPreset
    {
        public bool IsEnabled { get; set; } = true;

        public ItemObject.ItemTypeEnum? ItemType { get; set; }

        public ItemObject.ItemUsageSetFlags? UsageSetFlags { get; set; }

        public virtual bool DoesItemMeetRequirements(EquipmentElement equipment)
        {
            return ItemType is not null
                && equipment.Item.ItemType == ItemType
                && UsageSetFlags is not null
                && equipment.Item.PrimaryWeapon is not null
                && MBItem.GetItemUsageSetFlags(equipment.Item.PrimaryWeapon.ItemUsage).HasFlag(UsageSetFlags);
        }

        public double GetCalculatedValue(EquipmentElement equipment)
        {
            return DefaultCalculation(equipment);
        }

        private static double DefaultCalculation(EquipmentElement equipment)
        {
            if (equipment.IsEmpty)
            {
                return 0;
            }
            if (equipment.Item.HasArmorComponent)
            {
                if (equipment.Item.ItemType == ItemObject.ItemTypeEnum.HorseHarness)
                {
                    return equipment.GetModifiedMountBodyArmor();
                }
                return equipment.GetModifiedHeadArmor() + equipment.GetModifiedBodyArmor() + equipment.GetModifiedArmArmor() + equipment.GetModifiedLegArmor();
            }
            return equipment.ItemValue;
        }
        /**
        public enum CompareFields
        {
            Value,
            Weight,
            HeadArmor,
            LegArmor,
            BodyArmor,
            ArmArmor,
            ArmorTotal,
            HitPoints,
            ThrustDamage,
            SwingDamage,
            MissileDamage,
            HighestDamage,
            ThrustSpeed,
            SwingSpeed,
            MissileSpeed,
            Handling,
            StackCount,
            MountSpeed,
            MountManeuver,
            MountCharge,
            MountHitPoints,
        }

        private static double GetValueFromField(EquipmentElement equipment, CompareFields field)
        {
            switch (field)
            {
                case CompareFields.Value:
                    return equipment.ItemValue;
                case CompareFields.Weight:
                    return equipment.Item.Weight;
                case CompareFields.HeadArmor:
                    return equipment.GetModifiedHeadArmor();
                case CompareFields.LegArmor:
                    return equipment.GetModifiedLegArmor();
                case CompareFields.BodyArmor:
                    return equipment.GetModifiedBodyArmor();
                case CompareFields.ArmArmor:
                    return equipment.GetModifiedArmArmor();
                case CompareFields.ArmorTotal:
                    return equipment.GetModifiedHeadArmor() + equipment.GetModifiedBodyArmor() + equipment.GetModifiedArmArmor() + equipment.GetModifiedLegArmor();
                case CompareFields.HitPoints:
                    return equipment.GetModifiedMaximumHitPointsForUsage(0);
                case CompareFields.ThrustDamage:
                    return equipment.GetModifiedThrustDamageForUsage(0);
                case CompareFields.SwingDamage:
                    return equipment.GetModifiedSwingDamageForUsage(0);
                case CompareFields.MissileDamage:
                    return equipment.GetModifiedMissileDamageForUsage(0);
                case CompareFields.HighestDamage:
                    return (new[] { equipment.GetModifiedThrustDamageForUsage(0), equipment.GetModifiedSwingDamageForUsage(0), equipment.GetModifiedMissileDamageForUsage(0) }).Max();
                case CompareFields.ThrustSpeed:
                    return equipment.GetModifiedThrustSpeedForUsage(0);
                case CompareFields.SwingSpeed:
                    return equipment.GetModifiedSwingSpeedForUsage(0);
                case CompareFields.MissileSpeed:
                    return equipment.GetModifiedMissileSpeedForUsage(0);
                case CompareFields.Handling:
                    return equipment.GetModifiedHandlingForUsage(0);
                case CompareFields.StackCount:
                    return equipment.GetModifiedStackCountForUsage(0);
                case CompareFields.MountSpeed:
                    return equipment.Item.HorseComponent.Speed;
                case CompareFields.MountManeuver:
                    return equipment.Item.HorseComponent.Maneuver;
                case CompareFields.MountCharge:
                    return equipment.Item.HorseComponent.ChargeDamage;
                case CompareFields.MountHitPoints:
                    return equipment.Item.HorseComponent.HitPoints;
                default:
                    return 0;
            }
        }
        **/
    }

    public class HorsePreset : SlotPreset
    {
        public bool Camel { get; set; } = false;

        public override bool DoesItemMeetRequirements(EquipmentElement equipment)
        {
            if (equipment.Item.ItemType == ItemObject.ItemTypeEnum.Horse)
            {
                return Camel ^ IsMountCamel(equipment.Item);
            }
            return false;
        }

        private bool IsMountCamel(ItemObject mountObject)
        {
            return mountObject?.ItemComponent is HorseComponent itemComponent && itemComponent.Monster.MonsterUsage == "camel";
        }
    }
}
