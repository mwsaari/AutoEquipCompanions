using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace AutoEquipCompanions
{
   public class AutoEquipBehavior : CampaignBehaviorBase
   {
      private const string CharacterSettingsKey = "AECharacterToggles";
      private const string PresetsKey = "AEPresets";
      internal static AutoEquipBehavior Instance = new AutoEquipBehavior();
      private SpriteCategory _category;

      private GauntletLayer _layer;
      private GauntletMovieIdentifier _movie;

      internal AutoEquipOverlayVM OverlayViewModel { get; private set; }

      public override void RegisterEvents()
      {
         ScreenManager.OnPushScreen += OnPushScreen;
         ScreenManager.OnPopScreen += OnPopScreen;
      }

      public void DeRegisterEvents()
      {
         ScreenManager.OnPushScreen -= OnPushScreen;
         ScreenManager.OnPopScreen -= OnPopScreen;
      }

      private void OnPushScreen(ScreenBase pushedScreen)
      {
         if (pushedScreen is GauntletInventoryScreen inventoryScreen
             && _layer is null)
         {
            OpenOverlay(inventoryScreen);
         }
      }

      private void OnPopScreen(ScreenBase poppedScreen)
      {
         if (poppedScreen is GauntletInventoryScreen inventoryScreen
             && _layer is not null)
         {
            if (_category is not null)
            {
               _category.Unload();
            }
            if (_movie is not null)
            {
               _layer.ReleaseMovie(_movie);
               _movie = null;
            }
            _layer = null;
            OverlayViewModel = null;
         }
      }

      private void OpenOverlay(GauntletInventoryScreen inventoryScreen)
      {
         LoadSprites();
         var inventoryLogic = ((InventoryState)GameStateManager.Current.ActiveState).InventoryLogic;
         var autoEquipModel = new AutoEquipModel(inventoryLogic);
         OverlayViewModel = new AutoEquipOverlayVM(autoEquipModel, inventoryScreen);
         _layer = new GauntletLayer("GauntletLayer", 200, true);
         UIConfig.DoNotUseGeneratedPrefabs = true;
         _movie = _layer.LoadMovie("AutoEquipOverlay", OverlayViewModel);
         _layer.InputRestrictions.SetInputRestrictions();
         inventoryScreen.AddLayer(_layer);
      }

      public override void SyncData(IDataStore dataStore)
      {
         if (dataStore.IsSaving)
         {
            var characterData = Config.CreateCharacterSaveData();
            dataStore.SyncData(CharacterSettingsKey, ref characterData);

            var presetData = Config.CreatePresetSaveData();
            dataStore.SyncData(PresetsKey, ref presetData);
         }
         if (dataStore.IsLoading)
         {
            var characterData = "";
            var presetData = "";
            if (dataStore.SyncData(CharacterSettingsKey, ref characterData)
                && dataStore.SyncData(PresetsKey, ref presetData))
            {
               Config.ReadSaveData(characterData, presetData);
            }
            else
            {
               Config.Initialize();
            }
         }
      }

      private void LoadSprites()
      {
         var spriteData = UIResourceManager.SpriteData;
         var resourceContext = UIResourceManager.ResourceContext;
         var resourceDepot = UIResourceManager.ResourceDepot;

         _category = spriteData.SpriteCategories["ui_partyscreen"]; // select which category to load, put your category name here
         _category.Load(resourceContext, resourceDepot); // load the selected category
      }
   }
}
