using System.Collections.Generic;
using System.Linq;
using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Character;
using SandBox.GauntletUI;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace AutoEquipCompanions.ViewModel
{
   public class AutoEquipOverlayVM : TaleWorlds.Library.ViewModel
   {
      private readonly AutoEquipModel _autoEquipModel;
      private readonly CharacterSettings _defaultSettings = new CharacterSettings().Initialize();
      private readonly Dictionary<string, CharacterSettings> _heroToggles;
      private readonly GauntletInventoryScreen _inventoryScreen;
      private readonly HeroToggleController _toggles;
      private SPInventoryVM _boundInventoryViewModel;

      public AutoEquipOverlayVM(AutoEquipModel autoEquipModel, GauntletInventoryScreen inventoryScreen)
      {
         _autoEquipModel = autoEquipModel;
         _inventoryScreen = inventoryScreen;

         SettingsToggle = CampaignSettings.SettingsVisible;
         _heroToggles = CampaignSettings.CharacterSettings;
         _toggles = new HeroToggleController(() => CurrentHero, _heroToggles, _defaultSettings);
         TemplateDropdown = new TemplateDropdownVM(GetCurrentTemplate, SetCurrentTemplate);
         TryBindLiveInventoryVM();
      }

      public bool TryBindLiveInventoryVM()
      {
         if (_boundInventoryViewModel != null)
            return true;

         var vm = GetInventoryVM();
         if (vm == null)
            return false;

         _boundInventoryViewModel = vm;
         vm.CharacterList.PropertyChangedWithValue += SelectedCharacterChanged;
         RefreshValues();
         return true;
      }

      private SPInventoryVM InventoryViewModel => _boundInventoryViewModel ?? GetInventoryVM();

      private string CurrentHero => InventoryViewModel?.CharacterList.SelectedItem?.CharacterID;
      private bool HasCurrentHero => CurrentHero != null;

      [DataSourceProperty]
      public HintViewModel SettingsHint { get; private set; } = new HintViewModel
      {
         HintText = new TextObject("Right click to toggle showing settings.\nCannot Manually AutoEquip with current Locked Settings.")
      };

      [DataSourceProperty]
      public HintViewModel CharacterToggleHint { get; private set; } = new HintViewModel
      {
         HintText = new TextObject("Left click to toggle auto equip for this character.")
      };

      [DataSourceProperty]
      public bool SettingsToggle { get; set; }

      [DataSourceProperty]
      public string SettingsToggleText => SettingsToggle ? "Hide AEC" : "Show AEC";

      [DataSourceProperty]
      public TemplateDropdownVM TemplateDropdown { get; private set; }

      [DataSourceProperty]
      public bool TemplatesEnabled => Main.GameSettings.UseTemplates;

      [DataSourceProperty]
      public bool DebugModeEnabled => Main.GameSettings.DebugEnabled;

      [DataSourceProperty]
      public string TemplatesModeButtonText => TemplatesEnabled ? "UI: Templates" : "UI: Classic";

      [DataSourceProperty]
      public bool CharacterToggle
      {
         get => _toggles.CharacterToggle;
         set => _toggles.CharacterToggle = value;
      }

      [DataSourceProperty]
      public bool HeadToggle => GetSlotToggle(EquipmentIndex.Head);

      [DataSourceProperty]
      public bool CapeToggle => GetSlotToggle(EquipmentIndex.Cape);

      [DataSourceProperty]
      public bool BodyToggle => GetSlotToggle(EquipmentIndex.Body);

      [DataSourceProperty]
      public bool GlovesToggle => GetSlotToggle(EquipmentIndex.Gloves);

      [DataSourceProperty]
      public bool LegToggle => GetSlotToggle(EquipmentIndex.Leg);

      [DataSourceProperty]
      public bool HorseToggle => GetSlotToggle(EquipmentIndex.Horse);

      [DataSourceProperty]
      public bool HarnessToggle => GetSlotToggle(EquipmentIndex.HorseHarness);

      [DataSourceProperty]
      public bool Weapon0Toggle => GetSlotToggle(EquipmentIndex.Weapon0);

      [DataSourceProperty]
      public bool Weapon1Toggle => GetSlotToggle(EquipmentIndex.Weapon1);

      [DataSourceProperty]
      public bool Weapon2Toggle => GetSlotToggle(EquipmentIndex.Weapon2);

      [DataSourceProperty]
      public bool Weapon3Toggle => GetSlotToggle(EquipmentIndex.Weapon3);

      private SPInventoryVM GetInventoryVM()
      {
         var gauntletLayers = _inventoryScreen.Layers.OfType<GauntletLayer>();
         foreach (var view in gauntletLayers.Select(x => x.GetMovieIdentifier("Inventory")?.DataSource))
         {
            if (view is SPInventoryVM inventoryVM)
               return inventoryVM;
         }
         return null;
      }

      private void SelectedCharacterChanged(object sender, PropertyChangedWithValueEventArgs e)
      {
         RefreshValues();
      }

      private bool GetSlotToggle(EquipmentIndex index) => _toggles.GetSlotToggle(index);

      private ICharacterTemplate GetCurrentTemplate()
      {
         if (!HasCurrentHero)
            return CharacterTemplate.Instance;
         return _heroToggles.TryGetValue(CurrentHero, out var s) ? s.Template : _defaultSettings.Template;
      }

      private void SetCurrentTemplate(ICharacterTemplate template)
      {
         if (!HasCurrentHero)
            return;
         if (_heroToggles.TryGetValue(CurrentHero, out var settings))
            settings.Template = template;
         else
         {
            settings = new CharacterSettings().Initialize();
            settings.Template = template;
            _heroToggles.Add(CurrentHero, settings);
         }
      }

      public override void RefreshValues()
      {
         base.RefreshValues();
         TemplateDropdown?.Refresh();
         OnPropertyChanged(nameof(SettingsToggle));
         OnPropertyChanged(nameof(SettingsToggleText));
         OnPropertyChanged(nameof(TemplatesEnabled));
         OnPropertyChanged(nameof(TemplatesModeButtonText));
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
      }

      public void ToggleSettings()
      {
         SettingsToggle = !SettingsToggle;
         TemplateDropdown?.Refresh();
         OnPropertyChanged(nameof(SettingsToggle));
         OnPropertyChanged(nameof(SettingsToggleText));
      }

      public void ToggleTemplatesMode()
      {
         Main.GameSettings.UseTemplates = !Main.GameSettings.UseTemplates;
         Main.GameSettings.Save();
         OnPropertyChanged(nameof(TemplatesEnabled));
         OnPropertyChanged(nameof(TemplatesModeButtonText));
      }

      public void ToggleCharacter()
      {
         CharacterToggle = !CharacterToggle;
         TemplateDropdown?.Refresh();
         OnPropertyChanged(nameof(CharacterToggle));
      }

      public void ToggleHead()
      {
         ToggleEquipment(EquipmentIndex.Head);
         OnPropertyChanged(nameof(HeadToggle));
      }

      public void ToggleCape()
      {
         ToggleEquipment(EquipmentIndex.Cape);
         OnPropertyChanged(nameof(CapeToggle));
      }

      public void ToggleBody()
      {
         ToggleEquipment(EquipmentIndex.Body);
         OnPropertyChanged(nameof(BodyToggle));
      }

      public void ToggleGloves()
      {
         ToggleEquipment(EquipmentIndex.Gloves);
         OnPropertyChanged(nameof(GlovesToggle));
      }

      public void ToggleLeg()
      {
         ToggleEquipment(EquipmentIndex.Leg);
         OnPropertyChanged(nameof(LegToggle));
      }

      public void ToggleHorse()
      {
         ToggleEquipment(EquipmentIndex.Horse);
         OnPropertyChanged(nameof(HorseToggle));
      }

      public void ToggleHarness()
      {
         ToggleEquipment(EquipmentIndex.HorseHarness);
         OnPropertyChanged(nameof(HarnessToggle));
      }

      public void ToggleWeapon0()
      {
         ToggleEquipment(EquipmentIndex.Weapon0);
         OnPropertyChanged(nameof(Weapon0Toggle));
      }

      public void ToggleWeapon1()
      {
         ToggleEquipment(EquipmentIndex.Weapon1);
         OnPropertyChanged(nameof(Weapon1Toggle));
      }

      public void ToggleWeapon2()
      {
         ToggleEquipment(EquipmentIndex.Weapon2);
         OnPropertyChanged(nameof(Weapon2Toggle));
      }

      public void ToggleWeapon3()
      {
         ToggleEquipment(EquipmentIndex.Weapon3);
         OnPropertyChanged(nameof(Weapon3Toggle));
      }

      private void ToggleEquipment(EquipmentIndex index) => _toggles.ToggleEquipment(index);

      public void RunAutoEquip()
      {
         _autoEquipModel.AutoEquipCompanions(_heroToggles);
         InventoryViewModel?.RefreshValues();
      }

      public void OnExecuteCompleteTransactions()
      {
         CampaignSettings.SettingsVisible = SettingsToggle;
         _autoEquipModel.AutoEquipCompanions(CampaignSettings.CharacterSettings);
      }

      public override void OnFinalize()
      {
         base.OnFinalize();
         if (_boundInventoryViewModel != null)
            _boundInventoryViewModel.CharacterList.PropertyChangedWithValue -= SelectedCharacterChanged;
      }
   }
}
