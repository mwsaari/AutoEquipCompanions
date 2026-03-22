using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace AutoEquipCompanions.Model
{
   public class AutoEquipModel
   {
      private readonly InventoryLogic _inventoryLogic;
      private readonly HashSet<string> _lockedItems;

      public AutoEquipModel(InventoryLogic inventoryLogic)
      {
         _inventoryLogic = inventoryLogic;
         var tracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
         _lockedItems = new HashSet<string>(tracker.GetInventoryLocks());
      }

      private IEnumerable<ItemRosterElement> Items => MobileParty.MainParty.ItemRoster
         .Where(x => !_lockedItems.Contains(CampaignUIHelper.GetItemLockStringID(x.EquipmentElement)));

      public void AutoEquipCompanions(Dictionary<string, CharacterSettings> characterSettings,
         bool allSlotsEnabled = false)
      {
         var heroes = MobileParty.MainParty.MemberRoster
            .GetTroopRoster()
            .Where(x => x.Character.IsHero)
            .Select(x => x.Character.HeroObject)
            .Where(x => !characterSettings.ContainsKey(x.StringId) || characterSettings[x.StringId].CharacterToggle);
         foreach (var hero in heroes)
         {
            var hasUpgraded = false;
            try
            {
               var heroSettings = characterSettings.TryGetValue(hero.StringId, out var setting)
                  ? setting
                  : new CharacterSettings().Initialize(allSlotsEnabled);
               foreach (var (slot, template) in heroSettings.Template.Slots.Where(x => heroSettings[x.Slot]))
               {
                  var current = hero.BattleEquipment.GetEquipmentFromSlot(slot);
                  var replacement = GetBestReplacement(hero, slot, template, current);
                  if (replacement != null)
                  {
                     DoEquip(hero, slot, replacement.Value);
                     hasUpgraded = true;
                  }
                  else if (!current.IsEmpty && !template.IsValidFor(current, slot, hero))
                  {
                     DoUnequip(hero, slot, current);
                     hasUpgraded = true;
                  }
               }
            }
            catch (Exception ex)
            {
               InformationManager.DisplayMessage(new InformationMessage($"{ex.Message}"));
            }
            finally
            {
               if (hasUpgraded)
               {
                  var pronoun = hero.IsFemale ? "her" : "his";
                  InformationManager.DisplayMessage(new InformationMessage($"{hero.Name} upgraded {pronoun} equipment"));
               }
            }
         }
      }

      private ItemRosterElement? GetBestReplacement(Hero hero, EquipmentIndex slot, ISlotTemplate template, EquipmentElement current)
      {
         return Items
            .Where(x => template.IsValidFor(x.EquipmentElement, slot, hero))
            .OrderByDescending(x => template.GetScore(x.EquipmentElement))
            .TakeWhile(x => template.IsBetterThan(x.EquipmentElement, current))
            .Cast<ItemRosterElement?>()
            .FirstOrDefault();
      }

      private void DoEquip(Hero character, EquipmentIndex slot, ItemRosterElement replacement)
      {
         _inventoryLogic.AddTransferCommand(
            TransferCommand.Transfer(
               1,
               InventoryLogic.InventorySide.PlayerInventory,
               InventoryLogic.InventorySide.BattleEquipment,
               replacement,
               EquipmentIndex.None,
               slot,
               character.CharacterObject));
      }

      private void DoUnequip(Hero character, EquipmentIndex slot, EquipmentElement item)
      {
         _inventoryLogic.AddTransferCommand(
            TransferCommand.Transfer(
               1,
               InventoryLogic.InventorySide.BattleEquipment,
               InventoryLogic.InventorySide.PlayerInventory,
               new ItemRosterElement(item, 1),
               slot,
               EquipmentIndex.None,
               character.CharacterObject));
      }
   }
}
