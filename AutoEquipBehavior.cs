using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace AutoEquipCompanions
{
    public class AutoEquipBehavior : CampaignBehaviorBase
    {
        internal static AutoEquipBehavior Instance = new AutoEquipBehavior();

        GauntletLayer _layer;
        IGauntletMovie _movie;
        AutoEquipOverlayVM _overlayViewModel;
        SpriteCategory _category;

        internal AutoEquipOverlayVM OverlayViewModel => _overlayViewModel;

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
                || _layer is not null)
            {
                _category.Unload();
                _layer.ReleaseMovie(_movie);
                _layer = null;
                _movie = null;
                _overlayViewModel = null;
            }
        }

        private void OpenOverlay(GauntletInventoryScreen inventoryScreen)
        {
            LoadSprites();
            var inventoryLogic = ((InventoryState)GameStateManager.Current.ActiveState).InventoryLogic;
            var autoEquipModel = new AutoEquipModel(inventoryLogic);
            _overlayViewModel = new AutoEquipOverlayVM(autoEquipModel, inventoryScreen);
            _layer = new GauntletLayer(200, "GauntletLayer", true);
            UIConfig.DoNotUseGeneratedPrefabs = true;
            _movie = _layer.LoadMovie("AutoEquipOverlay", _overlayViewModel);
            _layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            inventoryScreen.AddLayer(_layer);
        }

        public override void SyncData(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                var saveData = Config.CreateSaveData();
                dataStore.SyncData("AECharacterToggles", ref saveData);
            }
            if (dataStore.IsLoading)
            {
                var saveData = "";
                if (dataStore.SyncData("AECharacterToggles", ref saveData) && !string.IsNullOrEmpty(saveData))
                {
                    Config.ReadSaveData(saveData);
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
            var resourceDepot = UIResourceManager.UIResourceDepot;

            _category = spriteData.SpriteCategories["ui_partyscreen"]; // select which category to load, put your category name here
            _category.Load(resourceContext, resourceDepot); // load the selected category
        }
    }
}
