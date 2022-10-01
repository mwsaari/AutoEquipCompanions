using AutoEquipCompanions.Model;
using AutoEquipCompanions.ViewModel;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
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
        AutoEquipModel _model;
        GauntletLayer _layer;
        IGauntletMovie _movie;
        AutoEquipOverlayVM _viewModel;

        public override void RegisterEvents()
        {
            CampaignEvents.PlayerInventoryExchangeEvent.AddNonSerializedListener(this, AutoEquipCompanionsEvent);
            ScreenManager.OnPushScreen += OnPushScreen;
            ScreenManager.OnPopScreen += OnPopScreen;  
        }

        private void AutoEquipCompanionsEvent(List<(ItemRosterElement, int)> _, List<(ItemRosterElement, int)> __, bool ___)
        {
            if (_model == null)
            {
                _model = new AutoEquipModel();
            }
            _model.AutoEquipCompanions();
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
            _layer = null;
            _movie = null;
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}
