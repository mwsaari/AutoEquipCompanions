using System.Collections.Generic;
using System.Linq;
using AutoEquipCompanions.Model;
using AutoEquipCompanions.Model.Saving;
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
      private readonly SPInventoryVM _inventoryViewModel;

      public AutoEquipOverlayVM(AutoEquipModel autoEquipModel, GauntletInventoryScreen inventoryScreen)
      {
         _autoEquipModel = autoEquipModel;
         _inventoryScreen = inventoryScreen;
         _inventoryViewModel = GetInventoryVM();
         if (_inventoryViewModel == null)
            return;

         SettingsToggle = CampaignSettings.SettingsVisible;
         _heroToggles = CampaignSettings.CharacterSettings;
         _inventoryViewModel.CharacterList.PropertyChangedWithValue += SelectedCharacterChanged;
         RefreshValues();
      }

      private string CurrentHero => _inventoryViewModel?.CharacterList.SelectedItem?.CharacterID;
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
      public bool CharacterToggle
      {
         get
         {
            if (!HasCurrentHero)
               return true;
            return !_heroToggles.TryGetValue(CurrentHero, out var value) || value.CharacterToggle;
         }
         set
         {
            if (!HasCurrentHero)
               return;
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

      private bool GetSlotToggle(EquipmentIndex index)
      {
         return HasCurrentHero && _heroToggles.TryGetValue(CurrentHero, out var s)
            ? s[index] : _defaultSettings[index];
      }

      public override sealed void RefreshValues()
      {
         base.RefreshValues();
         OnPropertyChanged(nameof (SettingsToggle));
         OnPropertyChanged(nameof (SettingsToggleText));
         OnPropertyChanged(nameof (CharacterToggle));
         OnPropertyChanged(nameof (HeadToggle));
         OnPropertyChanged(nameof (CapeToggle));
         OnPropertyChanged(nameof (BodyToggle));
         OnPropertyChanged(nameof (GlovesToggle));
         OnPropertyChanged(nameof (LegToggle));
         OnPropertyChanged(nameof (HorseToggle));
         OnPropertyChanged(nameof (HarnessToggle));
         OnPropertyChanged(nameof (Weapon0Toggle));
         OnPropertyChanged(nameof (Weapon1Toggle));
         OnPropertyChanged(nameof (Weapon2Toggle));
         OnPropertyChanged(nameof (Weapon3Toggle));
      }

      public void ToggleSettings()
      {
         SettingsToggle = !SettingsToggle;
         OnPropertyChanged(nameof (SettingsToggle));
         OnPropertyChanged(nameof (SettingsToggleText));
      }

      public void ToggleCharacter()
      {
         CharacterToggle = !CharacterToggle;
         OnPropertyChanged(nameof (CharacterToggle));
      }

      public void ToggleHead()
      {
         ToggleEquipment(EquipmentIndex.Head);
         OnPropertyChanged(nameof (HeadToggle));
      }

      public void ToggleCape()
      {
         ToggleEquipment(EquipmentIndex.Cape);
         OnPropertyChanged(nameof (CapeToggle));
      }

      public void ToggleBody()
      {
         ToggleEquipment(EquipmentIndex.Body);
         OnPropertyChanged(nameof (BodyToggle));
      }

      public void ToggleGloves()
      {
         ToggleEquipment(EquipmentIndex.Gloves);
         OnPropertyChanged(nameof (GlovesToggle));
      }

      public void ToggleLeg()
      {
         ToggleEquipment(EquipmentIndex.Leg);
         OnPropertyChanged(nameof (LegToggle));
      }

      public void ToggleHorse()
      {
         ToggleEquipment(EquipmentIndex.Horse);
         OnPropertyChanged(nameof (HorseToggle));
      }

      public void ToggleHarness()
      {
         ToggleEquipment(EquipmentIndex.HorseHarness);
         OnPropertyChanged(nameof (HarnessToggle));
      }

      public void ToggleWeapon0()
      {
         ToggleEquipment(EquipmentIndex.Weapon0);
         OnPropertyChanged(nameof (Weapon0Toggle));
      }

      public void ToggleWeapon1()
      {
         ToggleEquipment(EquipmentIndex.Weapon1);
         OnPropertyChanged(nameof (Weapon1Toggle));
      }

      public void ToggleWeapon2()
      {
         ToggleEquipment(EquipmentIndex.Weapon2);
         OnPropertyChanged(nameof (Weapon2Toggle));
      }

      public void ToggleWeapon3()
      {
         ToggleEquipment(EquipmentIndex.Weapon3);
         OnPropertyChanged(nameof (Weapon3Toggle));
      }

      private void ToggleEquipment(EquipmentIndex index)
      {
         if (!HasCurrentHero)
            return;
         if (_heroToggles.TryGetValue(CurrentHero, out var characterSettings))
         {
            characterSettings[index] = !characterSettings[index];
         }
         else
         {
            characterSettings = new CharacterSettings().Initialize();
            characterSettings[index] = !characterSettings[index];
            _heroToggles.Add(CurrentHero, characterSettings);
         }
      }

      public void RunAutoEquip()
      {
         if (!Main.GameSettings.CanAutoEquipLocked)
            return;
         _autoEquipModel.AutoEquipCompanions(_heroToggles);
         _inventoryViewModel.RefreshValues();
      }

      public void OnExecuteCompleteTransactions()
      {
         CampaignSettings.SettingsVisible = SettingsToggle;
         _autoEquipModel.AutoEquipCompanions(CampaignSettings.CharacterSettings);
      }

      public override void OnFinalize()
      {
         base.OnFinalize();
         if (_inventoryViewModel != null)
            _inventoryViewModel.CharacterList.PropertyChangedWithValue -= SelectedCharacterChanged;
      }
   }
}
