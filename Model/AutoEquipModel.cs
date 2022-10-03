using AutoEquipCompanions.Saving;
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
                    if (TryGetBestReplacement(hero, slot, out EquipmentElement replacement))
                    {
                        DoWeaponSwap(hero, slot, replacement);
                        hasUpgraded = true;
                    }
                }
                if (hasUpgraded)
                {
                    var pronoun = hero.IsFemale ? "her" : "his";
                    InformationManager.DisplayMessage(new InformationMessage($"{hero.Name} upgraded {pronoun} equipment"));
                }
            }
        }

        private bool TryGetBestReplacement(Hero hero, EquipmentIndex slot, out EquipmentElement bestReplacement)
        {
            bestReplacement = EquipmentElement.Invalid;
            var (currentEquipment, itemType) = GetEquipmentInfo(hero, slot);
            if (itemType == ItemObject.ItemTypeEnum.Invalid)
            {
                return false;
            }
            var orderedReplacements = MobileParty.MainParty.ItemRoster
                .Where(x => x.EquipmentElement.Item.Type == itemType)
                .OrderByDescending(x => x.EquipmentElement, EquipmentComparer.Instance)
                .Select(x => x.EquipmentElement);
            foreach (var replacement in orderedReplacements)
            {
                if (currentEquipment.Compare(replacement) >= 0)
                {
                    return false;
                }
                if (replacement.Item.Difficulty > hero.GetSkillValue(replacement.Item.RelevantSkill))
                {
                    return false;
                }
                if (!hero.BattleEquipment.Horse.IsEmpty
                    && (currentEquipment.Item?.HasWeaponComponent ?? false)
                    && replacement.Item.HasWeaponComponent)
                {
                    bool isReplacementNotUsableOnHorse = MBItem.GetItemUsageSetFlags(replacement.Item.PrimaryWeapon.ItemUsage).HasFlag(ItemObject.ItemUsageSetFlags.RequiresNoMount);
                    bool isCurrentNotUsableOnHorse = MBItem.GetItemUsageSetFlags(currentEquipment.Item.PrimaryWeapon.ItemUsage).HasFlag(ItemObject.ItemUsageSetFlags.RequiresNoMount);
                    if (isReplacementNotUsableOnHorse && !isCurrentNotUsableOnHorse)
                    {
                        return false;
                    }
                }
                bestReplacement = replacement;
                return true;
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

        private void DoWeaponSwap(Hero character, EquipmentIndex slot, EquipmentElement replacement)
        {
            var currentEquipment = character.BattleEquipment.GetEquipmentFromSlot(slot);
            character.BattleEquipment.AddEquipmentToSlotWithoutAgent(slot, replacement);
            MobileParty.MainParty.ItemRoster.AddToCounts(replacement, -1);
            if (!currentEquipment.IsEmpty)
            { 
                MobileParty.MainParty.ItemRoster.AddToCounts(currentEquipment, 1); 
            }
        }
    }
}
