using AutoEquipCompanions.Model.Saving;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions
{
   public class Main : MBSubModuleBase
   {

      private AutoEquipBehavior _behavior;
      public static GameSettings GameSettings { get; private set; } = new GameSettings();

      protected override void OnSubModuleLoad()
      {
         base.OnSubModuleLoad();
         GameSettings.Load();
         CampaignSettings.Initialize();
      }

      protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
      {
         if (starterObject is CampaignGameStarter campaignGameStarter)
         {
            _behavior = new AutoEquipBehavior();
            campaignGameStarter.AddBehavior(_behavior);
         }
      }

      public override void OnGameEnd(Game game)
      {
         _behavior?.UnregisterEvents();
      }
   }
}
