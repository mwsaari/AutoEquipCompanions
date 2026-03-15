using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.ScreenSystem;

namespace AutoEquipCompanions
{
   public class AutoEquipBehavior : CampaignBehaviorBase
   {
      private const string SaveKey = "AECharacterSettings";

      private readonly InventoryStateListener _listener;

      private GauntletLayer _overlayLayer;
      private AutoEquipOverlayVM _overlayVM;

      public AutoEquipBehavior()
      {
         _listener = new InventoryStateListener(OnInventoryClosed);
      }

      public override void RegisterEvents()
      {
         _listener.Register();
         ScreenManager.OnPushScreen += OnScreenPushed;
      }

      public void UnregisterEvents()
      {
         _listener.Unregister();
         ScreenManager.OnPushScreen -= OnScreenPushed;
      }

      private void OnScreenPushed(ScreenBase screen)
      {
         if (screen is not GauntletInventoryScreen inventoryScreen)
            return;

         var inventoryState = Game.Current.GameStateManager.ActiveState as InventoryState;
         if (inventoryState == null)
            return;

         var model = new AutoEquipModel(inventoryState.InventoryLogic);
         _overlayVM = new AutoEquipOverlayVM(model, inventoryScreen);
         _overlayLayer = new GauntletLayer("AutoEquipOverlay", 16) { IsFocusLayer = false };
         _overlayLayer.LoadMovie("AutoEquipOverlay", _overlayVM);
         inventoryScreen.AddLayer(_overlayLayer);
      }

      private void OnInventoryClosed(InventoryLogic inventoryLogic)
      {
         _overlayVM?.OnExecuteCompleteTransactions();
         _overlayVM?.OnFinalize();
         _overlayVM = null;
         _overlayLayer = null;
      }

      public override void SyncData(IDataStore dataStore)
      {
         if (dataStore.IsSaving)
         {
            var json = CampaignSettings.Serialize();
            dataStore.SyncData(SaveKey, ref json);
         }
         if (dataStore.IsLoading)
         {
            var json = "";
            if (dataStore.SyncData(SaveKey, ref json))
               CampaignSettings.Deserialize(json);
            else
               CampaignSettings.Initialize();
         }
      }
   }
}
