using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model
{
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

    public enum CompareMethods
    {
        Ordered,
        Sum,
        Product,
    }

    public class SlotPreset
    {

        public ItemObject.ItemUsageSetFlags UsageSetFlags { get; set; } = 0;

        public IList<CompareFields> PresetFields { get; set; } = new List<CompareFields>();

        public CompareMethods CompareMethod { get; set; } = CompareMethods.Ordered;

        public double GetCalculatedValue(EquipmentElement equipment)
        {
            if (!PresetFields.Any())
            {
                return DefaultCalculation(equipment);
            }
            else
            {
                switch (CompareMethod)
                {
                    case CompareMethods.Ordered:
                        return GetOrderedValue(equipment);
                    case CompareMethods.Sum:
                        return GetSumValue(equipment);
                    case CompareMethods.Product:
                        return GetProductValue(equipment);
                }
            }
            return 0;
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

        public double GetOrderedValue(EquipmentElement equipment)
        {
            double sum = 0;
            double factor = 1000 ^ 10;
            foreach (var field in PresetFields)
            {
                sum += GetValueFromField(equipment, field) * factor;
                factor /= 1000;
            }
            return sum;
        }

        public double GetSumValue(EquipmentElement equipment)
        {
            return PresetFields.Sum(field => GetValueFromField(equipment, field));
        }

        public double GetProductValue(EquipmentElement equipment)
        {
            double product = 1;
            foreach (var field in PresetFields)
            {
                product *= GetValueFromField(equipment, field);
            }
            return product;
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
    }
}
