using AutoEquipCompanions.Model;
using AutoEquipCompanions.Saving;
using AutoEquipCompanions.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace AutoEquipCompanions
{
    public class AutoEquipBehavior : CampaignBehaviorBase
    {
        internal static AutoEquipBehavior Instance = new AutoEquipBehavior();

        AutoEquipModel _model;
        GauntletLayer _layer;
        IGauntletMovie _movie;
        AutoEquipOverlayVM _viewModel;

        internal AutoEquipOverlayVM ViewModel => _viewModel;

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
            if (pushedScreen is not GauntletInventoryScreen inventoryScreen)
            {
                return;
            }
            _viewModel = new AutoEquipOverlayVM(inventoryScreen);
            _layer = new GauntletLayer(200);
            _movie = _layer.LoadMovie("AutoEquipOverlay", _viewModel);
            _layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            inventoryScreen.AddLayer(_layer);
        }

        private void OnPopScreen(ScreenBase poppedScreen)
        {
            if (poppedScreen is not GauntletInventoryScreen inventoryScreen
                || _layer is null)
            {
                return;
            }
            _layer.ReleaseMovie(_movie);
            inventoryScreen.RemoveLayer(_layer);
            _viewModel = null;
            _layer = null;
            _movie = null;
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
    }
}
