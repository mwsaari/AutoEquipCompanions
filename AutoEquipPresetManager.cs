using AutoEquipCompanions.Model;
using AutoEquipCompanions.ViewModel;
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
    public class AutoEquipPresetManager
    {
        GauntletLayer _layer;
        IGauntletMovie _movie;
        AutoEquipPresetsVM _viewModel;
        SpriteCategory _category;

        internal AutoEquipPresetsVM ViewModel => _viewModel;

        public void OpenPresetPage(ScreenBase screen)
        {
            LoadSprites();
            var inventoryLogic = ((InventoryState)GameStateManager.Current.ActiveState).InventoryLogic;
            var viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
            var autoEquipModel = new AutoEquipModel(inventoryLogic, viewDataTracker);
            _viewModel = new AutoEquipPresetsVM();

            _layer = new GauntletLayer(200, "GauntletLayer", true);
            UIConfig.DoNotUseGeneratedPrefabs = true;
            _movie = _layer.LoadMovie("AutoEquipOverlay", _viewModel);
            _layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            screen.AddLayer(_layer);
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
