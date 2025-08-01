﻿using AutoEquipCompanions.Saving;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions.Model
{
    public class AutoEquipModel
    {
        private InventoryLogic _inventoryLogic;

        public AutoEquipModel(InventoryLogic inventoryLogic)
        {
            _inventoryLogic = inventoryLogic;
        }

        public void AutoEquipCompanions(Dictionary<string, CharacterSettings> characterSettings)
        {
            var heroes = MobileParty.MainParty.MemberRoster
                .GetTroopRoster()
                .Where(x => x.Character.IsHero)
                .Select(x => x.Character.HeroObject)
                .Where(x => !Config.CharacterSettings.ContainsKey(x.StringId) || characterSettings[x.StringId].CharacterToggle);
            var inventoryGroupedByType = new Dictionary<ItemObject.ItemTypeEnum, ItemRosterElement[]>();
            foreach (var hero in heroes)
            {
                bool hasUpgraded = false;
                try
                {
                    CharacterSettings heroSettings;
                    if (characterSettings.ContainsKey(hero.StringId))
                    {
                        heroSettings = characterSettings[hero.StringId];
                    }
                    else
                    {
                        heroSettings = new CharacterSettings().Initialize();
                    }
                    foreach (EquipmentIndex slot in Enumerable.Range(0, (int)EquipmentIndex.NumEquipmentSetSlots))
                    {
                        if (slot == EquipmentIndex.ExtraWeaponSlot || !heroSettings[slot])
                        {
                            continue;
                        }
                        if (TryGetBestReplacement(hero, slot, out ItemRosterElement replacement))
                        {
                            DoWeaponSwap(hero, slot, replacement);
                            hasUpgraded = true;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"{ex.Message}"));
                    continue;
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

        private bool TryGetBestReplacement(Hero hero, EquipmentIndex slot, out ItemRosterElement bestReplacement)
        {
            bestReplacement = ItemRosterElement.Invalid;
            var (currentEquipment, itemType) = GetEquipmentInfo(hero, slot);
            if (itemType == ItemObject.ItemTypeEnum.Invalid)
            {
                return false;
            }
            var orderedReplacements = MobileParty.MainParty.ItemRoster
                .Where(x => x.EquipmentElement.Item.Type == itemType)
                .OrderByDescending(x => x.EquipmentElement, EquipmentComparer.Instance);
            foreach (var replacement in orderedReplacements)
            {
                try
                {
                    var currentItem = currentEquipment.Item;
                    var replacementItem = replacement.EquipmentElement.Item;
                    if (currentEquipment.Compare(replacement.EquipmentElement) >= 0)
                    {
                        continue;
                    }
                    if (replacementItem.Difficulty > hero.GetSkillValue(replacementItem.RelevantSkill))
                    {
                        continue;
                    }
                    if (!hero.BattleEquipment.Horse.IsEmpty && currentItem is not null && currentItem.HasWeaponComponent && replacementItem.HasWeaponComponent)
                    {
                        bool isReplacementNotUsableOnHorse = MBItem.GetItemUsageSetFlags(replacementItem.PrimaryWeapon.ItemUsage).HasFlag(ItemObject.ItemUsageSetFlags.RequiresNoMount);
                        bool isCurrentNotUsableOnHorse = MBItem.GetItemUsageSetFlags(currentItem.PrimaryWeapon.ItemUsage).HasFlag(ItemObject.ItemUsageSetFlags.RequiresNoMount);
                        if (isReplacementNotUsableOnHorse && !isCurrentNotUsableOnHorse)
                        {
                            continue;
                        }
                    }
                    if (itemType == ItemObject.ItemTypeEnum.Horse
                        && currentItem.HorseComponent.Monster.FamilyType != replacementItem.HorseComponent.Monster.FamilyType)
                    {
                        continue;
                    }
                    if (itemType == ItemObject.ItemTypeEnum.HorseHarness)
                    {
                        var horse = GetEquipmentInfo(hero, EquipmentIndex.Horse).Item1;
                        if (horse.IsEmpty) { return false; }
                        if (replacementItem.ArmorComponent.FamilyType != horse.Item.HorseComponent.Monster.FamilyType)
                        {
                            continue;
                        }
                    }
                    bestReplacement = replacement;
                    return true;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception($"Error processing replacement for {hero.Name} in slot {slot} with weapon {replacement}", ex);
                }
            }
            return false;
        }

        private (EquipmentElement, ItemObject.ItemTypeEnum) GetEquipmentInfo(Hero hero, EquipmentIndex slot)
        {
            var currentEquipment = hero.BattleEquipment.GetEquipmentFromSlot(slot);
            if (currentEquipment.IsEmpty)
            {
                ItemObject.ItemTypeEnum itemType;
                switch (slot)
                {
                    case EquipmentIndex.Head:
                        itemType = ItemObject.ItemTypeEnum.HeadArmor;
                        break;
                    case EquipmentIndex.Cape:
                        itemType = ItemObject.ItemTypeEnum.Cape;
                        break;
                    case EquipmentIndex.Body:
                        itemType = ItemObject.ItemTypeEnum.BodyArmor;
                        break;
                    case EquipmentIndex.Gloves:
                        itemType = ItemObject.ItemTypeEnum.HandArmor;
                        break;
                    case EquipmentIndex.Leg:
                        itemType = ItemObject.ItemTypeEnum.LegArmor;
                        break;
                    case EquipmentIndex.Horse:
                        itemType = ItemObject.ItemTypeEnum.Horse;
                        break;
                    case EquipmentIndex.HorseHarness:
                        itemType = ItemObject.ItemTypeEnum.HorseHarness;
                        break;
                    default:
                        itemType = ItemObject.ItemTypeEnum.Invalid;
                        break;
                }
                return (currentEquipment, itemType);
            }
            else
            {
                return (currentEquipment, currentEquipment.Item.ItemType);
            }
        }

        private void DoWeaponSwap(Hero character, EquipmentIndex slot, ItemRosterElement replacement)
        {
            _inventoryLogic.AddTransferCommand(TransferCommand.Transfer(1, InventoryLogic.InventorySide.PlayerInventory, InventoryLogic.InventorySide.Equipment, replacement, EquipmentIndex.None, slot, character.CharacterObject, false));
        }
    }
}
