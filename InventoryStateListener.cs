using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace AutoEquipCompanions
{
   public class InventoryStateListener : IGameStateManagerListener
   {
      private readonly Action _onInventoryClosed;

      public InventoryStateListener(Action onInventoryClosed)
      {
         _onInventoryClosed = onInventoryClosed;
      }

      void IGameStateManagerListener.OnPushState(GameState gameState, bool isTopGameState)
      {
         if (gameState is InventoryState inventoryState)
         {
            inventoryState.DoneLogicExtrasDelegate += _onInventoryClosed;
         }
      }

      void IGameStateManagerListener.OnCreateState(GameState gameState) { }
      void IGameStateManagerListener.OnPopState(GameState gameState) { }
      void IGameStateManagerListener.OnCleanStates() { }
      void IGameStateManagerListener.OnSavedGameLoadFinished() { }

      public void Register()
      {
         Game.Current.GameStateManager.RegisterListener(this);
      }

      public void Unregister()
      {
         Game.Current.GameStateManager.UnregisterListener(this);
      }
   }
}
