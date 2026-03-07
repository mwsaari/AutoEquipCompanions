using AutoEquipCompanions.Model.Saving;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions
{
   public class Main : MBSubModuleBase
   {
      private AutoEquipBehaviorV2 _behaviorV2;
      private ModConfig _modConfig;

      protected override void OnSubModuleLoad()
      {
         base.OnSubModuleLoad();
         _modConfig = ModConfig.Load();
         Config.Initialize();

         if (!_modConfig.UseV2)
         {
            new Harmony("top.boom.patch.autoequipcompanions").PatchAll();
         }
      }

      protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
      {
         if (starterObject is CampaignGameStarter campaignGameStarter)
         {
            if (_modConfig.UseV2)
            {
               _behaviorV2 = new AutoEquipBehaviorV2();
               campaignGameStarter.AddBehavior(_behaviorV2);
            }
            else
            {
               campaignGameStarter.AddBehavior(AutoEquipBehavior.Instance);
            }
         }
      }

      public override void OnGameEnd(Game game)
      {
         if (_modConfig.UseV2)
            _behaviorV2?.UnregisterEvents();
         else
            AutoEquipBehavior.Instance.DeRegisterEvents();
      }
   }
}
