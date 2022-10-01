using AutoEquipCompanions.Model;
using AutoEquipCompanions.Saving;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
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
        private readonly AutoEquipModel _autoEquipModel;
        private readonly SPInventoryVM _inventoryViewModel;

        public AutoEquipOverlayVM(GauntletInventoryScreen inventoryScreen) : base()
        {
            _inventoryScreen = inventoryScreen;
            _autoEquipModel = new AutoEquipModel(((InventoryState)GameStateManager.Current.ActiveState).InventoryLogic);
            _inventoryViewModel = GetInventoryVM();
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

        public string CurrentCharacter => _inventoryViewModel.CharacterList.SelectedItem.CharacterID;

        [DataSourceProperty]
        public bool CharacterToggle => AutoEquipConfig.CharacterData[CurrentCharacter].CharacterToggle;

        [DataSourceProperty]
        public bool[] SlotToggles => AutoEquipConfig.CharacterData[CurrentCharacter].ItemToggles;

        public override void RefreshValues()
        {
            base.RefreshValues();
            OnPropertyChanged(nameof(CharacterToggle));
            OnPropertyChanged(nameof(SlotToggles));
        }
    }
}
