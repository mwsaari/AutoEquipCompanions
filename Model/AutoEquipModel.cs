using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Character;
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

      public AutoEquipModel(InventoryLogic inventoryLogic) : this(inventoryLogic, GetLockedItemIds())
      {
      }

      internal AutoEquipModel(InventoryLogic inventoryLogic, HashSet<string> lockedItemIds)
      {
         _inventoryLogic = inventoryLogic;
         _lockedItems = lockedItemIds;
      }

      private static HashSet<string> GetLockedItemIds()
      {
         var tracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
         return new HashSet<string>(tracker.GetInventoryLocks());
      }

      private IEnumerable<ItemRosterElement> Items => MobileParty.MainParty.ItemRoster
         .Where(x => Main.GameSettings.CanAutoEquipLockedItems
            || !_lockedItems.Contains(CampaignUIHelper.GetItemLockStringID(x.EquipmentElement)));

      public void AutoEquipCompanions(Dictionary<string, CharacterSettings> characterSettings)
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
                  : new CharacterSettings().Initialize();
               foreach (var decision in DetermineSlotChanges(hero, heroSettings, Items))
               {
                  if (decision.Replacement != null)
                     DoEquip(hero, decision.Slot, decision.Replacement.Value);
                  else
                     DoUnequip(hero, decision.Slot, decision.CurrentToUnequip);
                  hasUpgraded = true;
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

      internal IEnumerable<SlotDecision> DetermineSlotChanges(
         Hero hero, CharacterSettings heroSettings, IEnumerable<ItemRosterElement> itemPool)
      {
         var candidates = itemPool.ToList();
         var characterTemplate = Main.GameSettings.UseTemplates ? heroSettings.Template : CharacterTemplate.Instance;
         foreach (var (slot, template) in characterTemplate.Slots.Where(x => heroSettings[x.Slot]))
         {
            var current = hero.BattleEquipment.GetEquipmentFromSlot(slot);
            var replacement = GetBestReplacement(candidates, hero, slot, template, current);
            if (replacement != null)
               yield return SlotDecision.Equip(slot, replacement.Value);
            else if (!current.IsEmpty && !template.IsValidFor(current, slot, hero))
               yield return SlotDecision.Unequip(slot, current);
         }
      }

      internal ItemRosterElement? GetBestReplacement(
         IEnumerable<ItemRosterElement> candidates, Hero hero, EquipmentIndex slot, ISlotTemplate template, EquipmentElement current)
      {
         return candidates
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

   internal readonly struct SlotDecision
   {
      public EquipmentIndex Slot { get; }
      public ItemRosterElement? Replacement { get; }
      public EquipmentElement CurrentToUnequip { get; }

      private SlotDecision(EquipmentIndex slot, ItemRosterElement? replacement, EquipmentElement currentToUnequip)
      {
         Slot = slot;
         Replacement = replacement;
         CurrentToUnequip = currentToUnequip;
      }

      public static SlotDecision Equip(EquipmentIndex slot, ItemRosterElement replacement) =>
         new SlotDecision(slot, replacement, default);

      public static SlotDecision Unequip(EquipmentIndex slot, EquipmentElement current) =>
         new SlotDecision(slot, null, current);
   }
}
