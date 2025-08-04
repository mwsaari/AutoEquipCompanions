using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace AutoEquipCompanions.Patches
{
    [HarmonyPatch(typeof(SPInventoryVM), nameof(SPInventoryVM.ExecuteCompleteTranstactions))]
    public class InventoryPatch
    {
        static void Prefix(List<string> ____lockedItemIDs)
        {
            AutoEquipBehavior.Instance.OverlayViewModel.OnExecuteCompleteTransactions(____lockedItemIDs);
        }
    }
}
