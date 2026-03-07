using AutoEquipCompanions.Model.Saving;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace AutoEquipCompanions
{
   public class AutoEquipBehaviorV2 : CampaignBehaviorBase
   {
      private const string CharacterSettingsKey = "AECharacterToggles";
      private const string PresetsKey = "AEPresets";

      private readonly InventoryStateListener _listener;

      public AutoEquipBehaviorV2()
      {
         _listener = new InventoryStateListener(OnInventoryClosed);
      }

      public override void RegisterEvents()
      {
         _listener.Register();
      }

      public void UnregisterEvents()
      {
         _listener.Unregister();
      }

      public override void SyncData(IDataStore dataStore)
      {
         if (dataStore.IsSaving)
         {
            var characterData = Config.CreateCharacterSaveData();
            dataStore.SyncData(CharacterSettingsKey, ref characterData);

            var presetData = Config.CreatePresetSaveData();
            dataStore.SyncData(PresetsKey, ref presetData);
         }

         if (dataStore.IsLoading)
         {
            var characterData = "";
            var presetData = "";
            if (dataStore.SyncData(CharacterSettingsKey, ref characterData)
                && dataStore.SyncData(PresetsKey, ref presetData))
            {
               Config.ReadSaveData(characterData, presetData);
            }
            else
            {
               Config.Initialize();
            }
         }
      }

      private void OnInventoryClosed()
      {
         InformationManager.DisplayMessage(new InformationMessage("[AutoEquipV2] Inventory closed."));
      }
   }
}
