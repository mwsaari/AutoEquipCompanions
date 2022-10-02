using AutoEquipCompanions.Model;
using AutoEquipCompanions.Saving;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions
{
    public class Main : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("top.boom.patch.autoequipcompanions").PatchAll();
            Config.Initialize();
        }

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            if(starterObject is CampaignGameStarter campaignGameStarter)
            {
                campaignGameStarter.AddBehavior(AutoEquipBehavior.Instance);
            }
        }
    }
}
