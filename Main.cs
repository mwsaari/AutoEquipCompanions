using AutoEquipCompanions.Model.Saving;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions
{
   public class Main : MBSubModuleBase
   {
      public static GameSettings GameSettings { get; private set; }

      private AutoEquipBehavior _behavior;

      protected override void OnSubModuleLoad()
      {
         base.OnSubModuleLoad();
         GameSettings = GameSettings.Load();
         InGameSettings.Initialize();
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
