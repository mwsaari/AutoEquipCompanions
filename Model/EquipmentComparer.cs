using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model
{
    public class EquipmentComparer : Comparer<EquipmentElement>
    {
        public static readonly EquipmentComparer Instance = new EquipmentComparer();

        public override int Compare(EquipmentElement x, EquipmentElement y)
        {
            return MattsBSMagicCalculation(x) - MattsBSMagicCalculation(y);
        }

        private int MattsBSMagicCalculation(EquipmentElement equipment)
        {
            if (equipment.IsEmpty)
            {
                return 0;
            }
            if (equipment.Item.HasArmorComponent)
            {
                if(equipment.Item.ItemType == ItemObject.ItemTypeEnum.HorseHarness)
                {
                    return equipment.GetModifiedMountBodyArmor();
                }
                return equipment.GetModifiedHeadArmor() + equipment.GetModifiedBodyArmor() + equipment.GetModifiedArmArmor() + equipment.GetModifiedLegArmor(); 
            }
            return equipment.ItemValue;
        }

        public static EquipmentElement Max(EquipmentElement x, EquipmentElement y)
        {
            return Instance.Compare(x, y) >= 0 ? x : y;
        }
    }

    public static class EquipmentElementExtensions
    {
        public static int Compare(this EquipmentElement x, EquipmentElement y)
        {
            return EquipmentComparer.Instance.Compare(x, y);
        }
    }
}
