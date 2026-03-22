using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Debug;
using AutoEquipCompanions.Model.Saving;
using System.Collections.Generic;
using AutoEquipCompanions.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace AutoEquipCompanions
{
   public class AutoEquipBehavior : CampaignBehaviorBase
   {
      private const string SaveKey = "AECharacterSettings";

      private readonly InventoryStateListener _listener;
      private GauntletLayer _overlayLayer;
      private AutoEquipOverlayVM _overlayVM;

      private SpriteCategory _spriteCategory;

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

         if (Main.GameSettings.DebugEnabled)
            ItemDebugLogger.DumpAll(inventoryState.InventoryLogic);

         var uiVersion = Main.GameSettings.UIVersion;
         if (uiVersion == 0)
            return;

         LoadSprites();
         var model = new AutoEquipModel(inventoryState.InventoryLogic);
         _overlayLayer = new GauntletLayer("AutoEquipOverlay", 16);
         _overlayLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Mouse);
         _overlayVM = new AutoEquipOverlayVM(model, inventoryScreen);
         _overlayLayer.LoadMovie("AutoEquipOverlay", _overlayVM);
         inventoryScreen.AddLayer(_overlayLayer);
      }

      private void LoadSprites()
      {
         var spriteData = UIResourceManager.SpriteData;
         var resourceContext = UIResourceManager.ResourceContext;
         var resourceDepot = UIResourceManager.ResourceDepot;
         _spriteCategory = spriteData.SpriteCategories["ui_partyscreen"];
         _spriteCategory.Load(resourceContext, resourceDepot);
      }

      private void OnInventoryClosed(InventoryLogic inventoryLogic)
      {
         var uiVersion = Main.GameSettings.UIVersion;
         if (uiVersion == 0)
         {
            var model = new AutoEquipModel(inventoryLogic);
            model.AutoEquipCompanions(new Dictionary<string, CharacterSettings>(),
               allSlotsEnabled: true);
            return;
         }

         _overlayVM?.OnExecuteCompleteTransactions();
         _overlayVM?.OnFinalize();
         _overlayVM = null;
         _overlayLayer = null;
      }

      public override void SyncData(IDataStore dataStore)
      {
         if (dataStore.IsSaving)
         {
            var json = CampaignSettings.Save();
            dataStore.SyncData(SaveKey, ref json);
         }
         if (dataStore.IsLoading)
         {
            var json = "";
            if (dataStore.SyncData(SaveKey, ref json))
               CampaignSettings.Load(json);
            else
               CampaignSettings.Initialize();
         }
      }
   }
}
