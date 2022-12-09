using HarmonyLib;
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
