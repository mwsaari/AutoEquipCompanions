using AutoEquipCompanions.Model;
using AutoEquipCompanions.Saving;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
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
        private Dictionary<string, CharacterSettings> _characterToggles;

        public AutoEquipOverlayVM(GauntletInventoryScreen inventoryScreen) : base()
        {
            _inventoryScreen = inventoryScreen;
            _autoEquipModel = new AutoEquipModel(((InventoryState)GameStateManager.Current.ActiveState).InventoryLogic);
            _inventoryViewModel = GetInventoryVM();
            _characterToggles = Config.CharacterData;
            _inventoryViewModel.CharacterList.PropertyChangedWithValue += SelectedCharacterChanged;
        }

        ~AutoEquipOverlayVM()
        {
            _inventoryViewModel.CharacterList.PropertyChangedWithValue -= SelectedCharacterChanged;
        }

        private SelectorVM<InventoryCharacterSelectorItemVM> CharacterList => _inventoryViewModel.CharacterList;

        private string CurrentCharacter => CharacterList.SelectedItem.CharacterID;

        [DataSourceProperty]
        public bool CharacterToggle
        {
            get
            {
                if(_characterToggles.TryGetValue(CurrentCharacter, out var value))
                {
                    return value.CharacterToggle;
                }
                return true;
            }
            set
            {
                if (_characterToggles.ContainsKey(CurrentCharacter))
                {
                    _characterToggles[CurrentCharacter].CharacterToggle = value;
                }
                else
                {
                    _characterToggles.Add(CurrentCharacter, new CharacterSettings() { CharacterToggle = value });
                }
                OnPropertyChanged();
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            OnPropertyChanged(nameof(CharacterToggle));
        }

        public void OnExecuteCompleteTransactions()
        {
            Config.CharacterData = _characterToggles;
            _autoEquipModel.AutoEquipCompanions();
        }

        public void ToggleEnableAEC()
        {
            CharacterToggle = !CharacterToggle;
        }

        private void SelectedCharacterChanged(object sender, PropertyChangedWithValueEventArgs e)
        {
            RefreshValues();
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
    }
}
