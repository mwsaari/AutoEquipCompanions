using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem.Inventory;
using AutoEquipCompanions.Saving;

namespace AutoEquipCompanions.Model
{
    public class AutoEquipModel
    {
        private InventoryLogic _inventoryLogic;

        public AutoEquipModel(InventoryLogic inventoryLogic)
        {
            _inventoryLogic = inventoryLogic;
        }

        public void AutoEquipCompanions()
        {
            var cofigData = Config.CharacterData;
            var heroes = MobileParty.MainParty.MemberRoster
                .GetTroopRoster()
                .Where(x => x.Character.IsHero)
                .Select(x => x.Character.HeroObject)
                .Where(x => !Config.CharacterData.ContainsKey(x.StringId) || Config.CharacterData[x.StringId].CharacterToggle);
            var inventoryGroupedByType = new Dictionary<ItemObject.ItemTypeEnum, ItemRosterElement[]>();
            foreach (var hero in heroes)
            {
                bool hasUpgraded = false;
                foreach (EquipmentIndex slot in Enumerable.Range(0, (int)EquipmentIndex.NumEquipmentSetSlots))
                {
                    if (slot == EquipmentIndex.ExtraWeaponSlot)
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
            var currentEquipment = hero.BattleEquipment.GetEquipmentFromSlot(slot);
            if (currentEquipment.IsEmpty)
            {
                return false;
            }
            var orderedReplacements = MobileParty.MainParty.ItemRoster
                .Where(x => x.EquipmentElement.Item.Type == currentEquipment.Item.Type)
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
                if (!hero.BattleEquipment.Horse.IsEmpty && currentEquipment.Item.HasWeaponComponent && replacement.Item.HasWeaponComponent)
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

        private void DoWeaponSwap(Hero character, EquipmentIndex slot, EquipmentElement replacement)
        {
            var currentEquipment = character.BattleEquipment.GetEquipmentFromSlot(slot);
            character.BattleEquipment.AddEquipmentToSlotWithoutAgent(slot, replacement);
            MobileParty.MainParty.ItemRoster.AddToCounts(replacement, -1);
            MobileParty.MainParty.ItemRoster.AddToCounts(currentEquipment, 1);
        }
    }
}
