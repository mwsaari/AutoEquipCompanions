using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.Core.ItemObject;

namespace AutoEquipCompanions
{
    public class AutoEquipBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.PlayerInventoryExchangeEvent.AddNonSerializedListener(this, AutoEquipCompanionsEvent);
        }

        private void AutoEquipCompanionsEvent(List<(ItemRosterElement, int)> _, List<(ItemRosterElement, int)> __, bool ___)
        {
            var heroes = MobileParty.MainParty.MemberRoster
                .GetTroopRoster()
                .Where(x => x.Character.IsHero && !x.Character.IsPlayerCharacter)
                .Select(x => x.Character.HeroObject);
            var inventoryGroupedByType = new Dictionary<ItemTypeEnum, ItemRosterElement[]>();
            foreach (var hero in heroes)
            {
                bool hasUpgraded = false;
                foreach (EquipmentIndex slot in Utilities.Equipments)
                {
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
            if(currentEquipment.IsEmpty)
            {
                return false;
            }
            var orderedReplacements = MobileParty.MainParty.ItemRoster
                .Where(x => x.EquipmentElement.Item.Type == currentEquipment.Item.Type)
                .OrderByDescending(x => x.EquipmentElement, EquipmentComparer.Instance)
                .Select(x => x.EquipmentElement);
            foreach(var replacement in orderedReplacements)
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
                    bool isReplacementNotUsableOnHorse = MBItem.GetItemUsageSetFlags(replacement.Item.PrimaryWeapon.ItemUsage).HasFlag(ItemUsageSetFlags.RequiresNoMount);
                    bool isCurrentNotUsableOnHorse = MBItem.GetItemUsageSetFlags(currentEquipment.Item.PrimaryWeapon.ItemUsage).HasFlag(ItemUsageSetFlags.RequiresNoMount);
                    if(isReplacementNotUsableOnHorse && !isCurrentNotUsableOnHorse)
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

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}
