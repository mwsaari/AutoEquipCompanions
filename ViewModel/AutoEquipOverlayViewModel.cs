using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory;
using TaleWorlds.ScreenSystem;

namespace AutoEquipCompanions.ViewModel
{
    public class AutoEquipOverlayVM : TaleWorlds.Library.ViewModel
    {
        private readonly GauntletInventoryScreen _inventoryScreen;
        private readonly InventoryState _inventoryState;
        private readonly SPInventoryVM _inventoryViewModel;
        private GauntletLayer _settingsLayer;
        private SettingsViewModel _settingsVM;
        private IGauntletMovie _settingsMovie;
        private HintViewModel _settingsHint;

        public AutoEquipOverlayVM(GauntletInventoryScreen inventoryScreen) : base()
        {
            _inventoryScreen = inventoryScreen;
            _inventoryState = (InventoryState)GameStateManager.Current.ActiveState;
            _inventoryViewModel = GetInventoryVM();
            _settingsHint = new HintViewModel(new TextObject("AutoEquipCompanions"));
        }

        private SPInventoryVM GetInventoryVM()
        {
            // This is written as a loop for safety. But really inventory screen will be first layer, and inventoryVM will be first view.
            var gauntletLayers = _inventoryScreen.Layers.OfType<GauntletLayer>();
            foreach (var view in gauntletLayers.SelectMany(x => x._moviesAndDatasources))
            {
                if (view.Item2 is SPInventoryVM inventoryVM)
                {
                    return inventoryVM;
                }
            }
            return null;
        }

        [DataSourceProperty]
        public HintViewModel SettingHint
        {
            get => _settingsHint;
            set
            {
                if (value != _settingsHint)
                {
                    _settingsHint = value;
                    OnPropertyChanged(nameof(SettingHint));
                }
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
        }

        public void OpenAutoEquipSettings()
        {
            if(_settingsLayer is null)
            {
                _settingsLayer = new GauntletLayer(201);
                _settingsLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
                _settingsLayer.InputRestrictions.SetInputRestrictions(true, TaleWorlds.Library.InputUsageMask.All);
                _settingsVM = new SettingsViewModel();
                _settingsMovie = _settingsLayer.LoadMovie("AutoEquipSettingsPopup", _settingsVM);
                _settingsLayer.IsFocusLayer = true;
                ScreenManager.TrySetFocus(_settingsLayer);
                _inventoryScreen.AddLayer(_settingsLayer);
            }
        }

        public void CloseAutoEquipSettings()
        {
            if(_settingsLayer is not null)
            {
                _settingsLayer.ReleaseMovie(_settingsMovie);
                _inventoryScreen.RemoveLayer(_settingsLayer);
                _settingsLayer.InputRestrictions.ResetInputRestrictions();
                _settingsLayer = null;
                _settingsMovie = null;
                RefreshValues();
            }
        }
    }
}
