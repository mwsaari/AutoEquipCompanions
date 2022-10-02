using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace AutoEquipCompanions.Patches
{
    [HarmonyPatch(typeof(SPInventoryVM), nameof(SPInventoryVM.ExecuteCompleteTranstactions))]
    public class InventoryPatch
    {
        static void Prefix()
        {
            AutoEquipBehavior.Instance.ViewModel.OnExecuteCompleteTransactions();
        }
    }
}
