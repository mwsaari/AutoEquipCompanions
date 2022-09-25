using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions
{
    public class EquipmentComparer : Comparer<EquipmentElement>
    {
        public static readonly EquipmentComparer Instance = new EquipmentComparer();

        public override int Compare(EquipmentElement x, EquipmentElement y)
        {
            return MattsBSMagicCalculation(x) - MattsBSMagicCalculation(y);
        }

        private int MattsBSMagicCalculation(EquipmentElement item)
        {
            return ((int)item.Item.Tier * 1000) + item.Item.Value;
        }

        public static EquipmentElement Max(EquipmentElement x, EquipmentElement y)
        {
            return Instance.Compare(x, y) >= 0 ? x : y;
        }
    }
}
