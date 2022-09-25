using System.Collections.Generic;
using TaleWorlds.Core;
using static TaleWorlds.Core.ItemObject;

namespace AutoEquipCompanions
{
    public class EquipmentComparer : Comparer<EquipmentElement>
    {
        public static readonly EquipmentComparer Instance = new EquipmentComparer();

        private static readonly List<ItemTypeEnum> _armorItemTypeEnums = new List<ItemTypeEnum>() { ItemTypeEnum.HeadArmor, ItemTypeEnum.Cape, ItemTypeEnum.BodyArmor, ItemTypeEnum.LegArmor, ItemTypeEnum.HandArmor }; 

        public override int Compare(EquipmentElement x, EquipmentElement y)
        {
            return MattsBSMagicCalculation(x) - MattsBSMagicCalculation(y);
        }

        private int MattsBSMagicCalculation(EquipmentElement equipment)
        {
            if (_armorItemTypeEnums.Contains(equipment.Item.ItemType))
            {
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
