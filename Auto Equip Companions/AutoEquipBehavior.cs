using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
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
            var partyHeroCharacters = MobileParty.MainParty.MemberRoster
                .GetTroopRoster()
                .Where(x => x.Character.IsHero && !x.Character.IsPlayerCharacter)
                .Select(x => x.Character.HeroObject);
            var inventoryGroupedByType = new Dictionary<ItemTypeEnum, ItemRosterElement[]>();
            foreach (var character in partyHeroCharacters)
            {
                foreach (EquipmentIndex slot in Utilities.Equipments)
                {
                    var currentItem = character.BattleEquipment.GetEquipmentFromSlot(slot);
                    if (!currentItem.IsEmpty
                        && currentItem.Item.Type is var ItemType
                        && MobileParty.MainParty.ItemRoster
                            .Where(x => x.EquipmentElement.Item.Type == ItemType)
                            .OrderByDescending(x => x.EquipmentElement, EquipmentComparer.Instance) is var replacements
                        && !replacements.IsEmpty()
                        && replacements.First().EquipmentElement is var bestReplacement
                        && EquipmentComparer.Instance.Compare(currentItem, bestReplacement) < 0)                        
                    {
                        character.BattleEquipment.AddEquipmentToSlotWithoutAgent(slot, bestReplacement);
                        MobileParty.MainParty.ItemRoster.AddToCounts(bestReplacement, -1);
                        MobileParty.MainParty.ItemRoster.AddToCounts(currentItem, 1);
                    }
                }
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
    }
}
