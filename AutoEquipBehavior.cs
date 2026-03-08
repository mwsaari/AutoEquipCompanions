using AutoEquipCompanions.Model.Saving;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace AutoEquipCompanions
{
   public class AutoEquipBehavior : CampaignBehaviorBase
   {
      private const string SaveKey = "AECharacterSettings";

      private readonly InventoryStateListener _listener;

      public AutoEquipBehavior()
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

      private void OnInventoryClosed()
      {
         InformationManager.DisplayMessage(new InformationMessage("[AutoEquipV2] Inventory closed."));
      }

      public override void SyncData(IDataStore dataStore)
      {
         if (dataStore.IsSaving)
         {
            var json = CampaignSettings.Serialize();
            dataStore.SyncData(SaveKey, ref json);
         }
         if (dataStore.IsLoading)
         {
            var json = "";
            if (dataStore.SyncData(SaveKey, ref json))
               CampaignSettings.Deserialize(json);
            else
               CampaignSettings.Initialize();
         }
      }
   }
}
