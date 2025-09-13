using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;

namespace AutoEquipCompanions.ViewModel
{
    public class AutoEquipOverlayVM : TaleWorlds.Library.ViewModel
    {
        private readonly GauntletInventoryScreen _inventoryScreen;
        private readonly AutoEquipModel _autoEquipModel;
        private readonly SPInventoryVM _inventoryViewModel;
        private readonly CharacterSettings defaultSettings = new CharacterSettings().Initialize();
        private Dictionary<string, CharacterSettings> _heroToggles;
        private SelectorVM<PresetSelectorItemVM> _selectorOptions;

        public AutoEquipOverlayVM(AutoEquipModel autoEquipModel, GauntletInventoryScreen inventoryScreen) : base()
        {
            _autoEquipModel = autoEquipModel;
            _inventoryScreen = inventoryScreen;
            _inventoryViewModel = GetInventoryVM();
            SettingsToggle = Config.SettingsVisible;
            _heroToggles = Config.CharacterSettings;
            _inventoryViewModel.CharacterList.PropertyChangedWithValue += SelectedCharacterChanged;
            UpdatePresets();
            RefreshValues();
        }

        ~AutoEquipOverlayVM()
        {
            _inventoryViewModel.CharacterList.PropertyChangedWithValue -= SelectedCharacterChanged;
        }

        private SelectorVM<InventoryCharacterSelectorItemVM> CharacterList => _inventoryViewModel.CharacterList;

        private string CurrentHero => CharacterList.SelectedItem?.CharacterID ?? "";

        [DataSourceProperty]
        public HintViewModel SettingsHint { get; private set; } = new HintViewModel()
        {
            HintText = new TaleWorlds.Localization.TextObject("Right click to toggle showing settings.\nCannot Manually AutoEquip with current Locked Settings.")
        };

        [DataSourceProperty]
        public HintViewModel CharacterToggleHint { get; private set; } = new HintViewModel()
        {
            HintText = new TaleWorlds.Localization.TextObject("Left click to toggle auto equip for this character.")
        };

        [DataSourceProperty]
        public bool SettingsToggle { get; set; }

        [DataSourceProperty]
        public string SettingsToggleText => SettingsToggle ? "Hide AEC" : "Show AEC";

        [DataSourceProperty]
        public bool CharacterToggle
        {
            get
            {
                if (_heroToggles.TryGetValue(CurrentHero, out var value))
                {
                    return value.CharacterToggle;
                }
                return true;
            }
            set
            {
                if (_heroToggles.ContainsKey(CurrentHero))
                {
                    _heroToggles[CurrentHero].CharacterToggle = value;
                }
                else
                {
                    var characterSettings = new CharacterSettings().Initialize();
                    characterSettings.CharacterToggle = value;
                    _heroToggles.Add(CurrentHero, characterSettings);
                }
            }
        }

        [DataSourceProperty]
        public SelectorVM<PresetSelectorItemVM> PresetOptions
        {
            get => _selectorOptions;
            set
            {
                if (_selectorOptions != value)
                {
                    _selectorOptions = value;
                    OnPropertyChanged(nameof(PresetOptions));
                }
            }
        }

        private void OnPresetSelected(SelectorVM<PresetSelectorItemVM> selectorVM)
        {
            if (_heroToggles.TryGetValue(CurrentHero, out var characterSettings))
            {
                characterSettings.Preset = selectorVM.SelectedItem.Preset;
            }
            else
            {
                characterSettings = new CharacterSettings().Initialize();
                characterSettings.Preset = selectorVM.SelectedItem.Preset;
            }
        }

        public void UpdatePresets()
        {
            if (_selectorOptions == null)
            {
                _selectorOptions = new SelectorVM<PresetSelectorItemVM>(-1, new Action<SelectorVM<PresetSelectorItemVM>>(OnPresetSelected));
            }
        }

        public void ToggleSettings()
        {
            SettingsToggle = !SettingsToggle;
            OnPropertyChanged(nameof(SettingsToggle));
            OnPropertyChanged(nameof(SettingsToggleText));
        }

        public void ToggleCharacter()
        {
            CharacterToggle = !CharacterToggle;
            OnPropertyChanged(nameof(CharacterToggle));
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            OnPropertyChanged(nameof(SettingsToggle));
            OnPropertyChanged(nameof(SettingsToggleText));
            OnPropertyChanged(nameof(CharacterToggle));
            OnPropertyChanged(nameof(HeadToggle));
            OnPropertyChanged(nameof(CapeToggle));
            OnPropertyChanged(nameof(BodyToggle));
            OnPropertyChanged(nameof(GlovesToggle));
            OnPropertyChanged(nameof(LegToggle));
            OnPropertyChanged(nameof(HorseToggle));
            OnPropertyChanged(nameof(HarnessToggle));
            OnPropertyChanged(nameof(Weapon0Toggle));
            OnPropertyChanged(nameof(Weapon1Toggle));
            OnPropertyChanged(nameof(Weapon2Toggle));
            OnPropertyChanged(nameof(Weapon3Toggle));
            OnPropertyChanged(nameof(PresetOptions));
        }

        public void ToggleEquipment(EquipmentIndex index)
        {
            if (_heroToggles.TryGetValue(CurrentHero, out var characterSettings))
            {
                characterSettings[index] = !characterSettings[index];
            }
            else
            {
                characterSettings = new CharacterSettings().Initialize();
                characterSettings[index] = false;
                _heroToggles.Add(CurrentHero, characterSettings);
            }
        }

        public void RunAutoEquip()
        {
            if (!Config.GeneralSettings.CanAutoEquipIgnoreLockedItems)
            {
                return;
            }
            _autoEquipModel.AutoEquipCompanions(_heroToggles);
            _inventoryViewModel.RefreshValues();
        }

        public void OnExecuteCompleteTransactions(IEnumerable<string> lockedItemIDs)
        {
            Config.SettingsVisible = SettingsToggle;
            Config.CharacterSettings = _heroToggles;
            if (!Config.GeneralSettings.CanAutoEquipIgnoreLockedItems)
            {
                _autoEquipModel.AutoEquipCompanions(Config.CharacterSettings, lockedItemIDs);
            }
            _autoEquipModel.AutoEquipCompanions(Config.CharacterSettings);
        }

        private void SelectedCharacterChanged(object sender, PropertyChangedWithValueEventArgs e)
        {
            RefreshValues();
        }

        private SPInventoryVM GetInventoryVM()
        {
            // This is written as a loop for safety. But really inventory screen will be first layer, and inventoryVM will be first view.
            var gauntletLayers = _inventoryScreen.Layers.OfType<GauntletLayer>();
            foreach (var view in gauntletLayers.SelectMany(x => x.MoviesAndDataSources))
            {
                if (view.Item2 is SPInventoryVM inventoryVM)
                {
                    return inventoryVM;
                }
            }
            return null;
        }

        #region Armor
        [DataSourceProperty]
        public bool HeadToggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Head] : defaultSettings[EquipmentIndex.Head];

        public void ToggleHead()
        {
            ToggleEquipment(EquipmentIndex.Head);
            OnPropertyChanged(nameof(HeadToggle));
        }

        [DataSourceProperty]
        public bool CapeToggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Cape] : defaultSettings[EquipmentIndex.Cape];

        public void ToggleCape()
        {
            ToggleEquipment(EquipmentIndex.Cape);
            OnPropertyChanged(nameof(CapeToggle));
        }

        [DataSourceProperty]
        public bool BodyToggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Body] : defaultSettings[EquipmentIndex.Body];

        public void ToggleBody()
        {
            ToggleEquipment(EquipmentIndex.Body);
            OnPropertyChanged(nameof(BodyToggle));
        }

        [DataSourceProperty]
        public bool GlovesToggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Gloves] : defaultSettings[EquipmentIndex.Gloves];

        public void ToggleGloves()
        {
            ToggleEquipment(EquipmentIndex.Gloves);
            OnPropertyChanged(nameof(GlovesToggle));
        }

        [DataSourceProperty]
        public bool LegToggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Leg] : defaultSettings[EquipmentIndex.Leg];

        public void ToggleLeg()
        {
            ToggleEquipment(EquipmentIndex.Leg);
            OnPropertyChanged(nameof(LegToggle));
        }
        #endregion

        #region Horse
        [DataSourceProperty]
        public bool HorseToggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Horse] : defaultSettings[EquipmentIndex.Horse];

        public void ToggleHorse()
        {
            ToggleEquipment(EquipmentIndex.Horse);
            OnPropertyChanged(nameof(HorseToggle));
        }

        [DataSourceProperty]
        public bool HarnessToggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.HorseHarness] : defaultSettings[EquipmentIndex.HorseHarness];

        public void ToggleHarness()
        {
            ToggleEquipment(EquipmentIndex.HorseHarness);
            OnPropertyChanged(nameof(HarnessToggle));
        }
        #endregion

        #region Weapons
        [DataSourceProperty]
        public bool Weapon0Toggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Weapon0] : defaultSettings[EquipmentIndex.Weapon0];

        public void ToggleWeapon0()
        {
            ToggleEquipment(EquipmentIndex.Weapon0);
            OnPropertyChanged(nameof(Weapon0Toggle));
        }

        [DataSourceProperty]
        public bool Weapon1Toggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Weapon1] : defaultSettings[EquipmentIndex.Weapon1];

        public void ToggleWeapon1()
        {
            ToggleEquipment(EquipmentIndex.Weapon1);
            OnPropertyChanged(nameof(Weapon1Toggle));
        }

        [DataSourceProperty]
        public bool Weapon2Toggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Weapon2] : defaultSettings[EquipmentIndex.Weapon2];

        public void ToggleWeapon2()
        {
            ToggleEquipment(EquipmentIndex.Weapon2);
            OnPropertyChanged(nameof(Weapon2Toggle));
        }

        [DataSourceProperty]
        public bool Weapon3Toggle => _heroToggles.TryGetValue(CurrentHero, out var characterSettings)
            ? characterSettings[EquipmentIndex.Weapon3] : defaultSettings[EquipmentIndex.Weapon3];

        public void ToggleWeapon3()
        {
            ToggleEquipment(EquipmentIndex.Weapon3);
            OnPropertyChanged(nameof(Weapon3Toggle));
        }
        #endregion

    }
}
