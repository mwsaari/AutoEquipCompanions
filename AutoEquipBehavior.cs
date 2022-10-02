using AutoEquipCompanions.Model;
using AutoEquipCompanions.Saving;
using AutoEquipCompanions.ViewModel;
using Newtonsoft.Json;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
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

        private void OnPushScreen(ScreenBase pushedScreen)
        {
            if(pushedScreen is not GauntletInventoryScreen inventoryScreen)
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
            if (poppedScreen is not GauntletInventoryScreen inventoryScreen)
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
                var saveData = JsonConvert.SerializeObject(Config.CharacterSettings);
                dataStore.SyncData("AECharacterToggles", ref saveData);
            }
            if (dataStore.IsLoading)
            {
                var jsonString = "";
                if (dataStore.SyncData("AECharacterToggles", ref jsonString) && !string.IsNullOrEmpty(jsonString))
                {
                    Config.CharacterSettings = JsonConvert.DeserializeObject<Dictionary<string, CharacterSettings>>(jsonString);
                }
                else
                {
                    Config.CharacterSettings = new Dictionary<string, CharacterSettings>();
                }
            }
        }
    }
}
