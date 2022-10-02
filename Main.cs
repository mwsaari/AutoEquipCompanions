using AutoEquipCompanions.Saving;
using HarmonyLib;
using System.Linq;
using TaleWorlds.CampaignSystem;
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
            if (starterObject is CampaignGameStarter campaignGameStarter)
            {
                campaignGameStarter.AddBehavior(AutoEquipBehavior.Instance);
            }
        }

        public override void OnGameEnd(Game game)
        {
            AutoEquipBehavior.Instance.DeRegisterEvents();
        }
    }
}
