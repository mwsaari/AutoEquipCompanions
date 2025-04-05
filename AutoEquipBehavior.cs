using AutoEquipCompanions.Saving;
using AutoEquipCompanions.ViewModel;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem;
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
        AutoEquipOverlayVM _viewModel;
        SpriteCategory _category;

        internal AutoEquipOverlayVM ViewModel => _viewModel;

        private static bool _showUI = true;

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
            LoadSprites();
            _viewModel = new AutoEquipOverlayVM(inventoryScreen);
            if (_showUI)
            {
                _layer = new GauntletLayer(200, "GauntletLayer", true);
                UIConfig.DoNotUseGeneratedPrefabs = true;
                _movie = _layer.LoadMovie("AutoEquipOverlay", _viewModel);
                _layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                inventoryScreen.AddLayer(_layer);
            }
            else
            {
                var mainHeroId = Campaign.Current.MainParty.LeaderHero.StringId;
                if (Config.CharacterSettings.TryGetValue(mainHeroId, out var characterSettings))
                {
                    characterSettings.CharacterToggle = false;
                }
                else
                {
                    characterSettings = new CharacterSettings().Initialize();
                    characterSettings.CharacterToggle = false;
                    Config.CharacterSettings.Add(mainHeroId, characterSettings);
                }
            }
        }


        private void OnPopScreen(ScreenBase poppedScreen)
        {
            if (poppedScreen is not GauntletInventoryScreen inventoryScreen
                || _layer is null)
            {
                return;
            }
            if (_showUI)
            {
                _category.Unload();
                _layer.ReleaseMovie(_movie);
                _layer = null;
                _movie = null;
            }
            _viewModel = null;
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
