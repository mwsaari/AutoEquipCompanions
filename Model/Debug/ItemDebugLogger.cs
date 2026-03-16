using System;
using System.Reflection;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions.Model.Debug
{
   public static class ItemDebugLogger
   {
      public static void DumpAll(InventoryLogic inventoryLogic)
      {
         var sb = new StringBuilder();
         sb.AppendLine($"=== AutoEquipCompanions Item Dump: {DateTime.Now} ===");
         sb.AppendLine();

         DumpLeftInventory(sb, inventoryLogic);
         DumpPartyInventory(sb);
         DumpAllHeroEquipment(sb);

         Logger.WriteToItemDebug(sb.ToString());
      }

      private static void DumpLeftInventory(StringBuilder sb, InventoryLogic inventoryLogic)
      {
         sb.AppendLine("========== Left Inventory (Loot/Merchant) ==========");
         sb.AppendLine();

         try
         {
            var rosters = typeof (InventoryLogic)
               .GetField("_rosters", BindingFlags.NonPublic | BindingFlags.Instance)
               ?.GetValue(inventoryLogic) as ItemRoster[];

            if (rosters == null || rosters.Length == 0)
            {
               sb.AppendLine("  (could not access left roster)");
               sb.AppendLine();
               return;
            }

            foreach (var element in rosters[0])
            {
               if (element.IsEmpty)
                  continue;
               AppendItemElement(sb, element.EquipmentElement, element.Amount);
            }
         }
         catch (Exception ex)
         {
            sb.AppendLine($"  ERROR: {ex}");
         }

         sb.AppendLine();
      }

      private static void DumpPartyInventory(StringBuilder sb)
      {
         sb.AppendLine("========== Party Inventory ==========");
         sb.AppendLine();

         try
         {
            foreach (var element in MobileParty.MainParty.ItemRoster)
            {
               if (element.IsEmpty)
                  continue;
               AppendItemElement(sb, element.EquipmentElement, element.Amount);
            }
         }
         catch (Exception ex)
         {
            sb.AppendLine($"  ERROR: {ex}");
         }

         sb.AppendLine();
      }

      private static void DumpAllHeroEquipment(StringBuilder sb)
      {
         try
         {
            foreach (var element in MobileParty.MainParty.MemberRoster.GetTroopRoster())
            {
               if (!element.Character.IsHero)
                  continue;
               DumpHeroEquipment(sb, element.Character.HeroObject);
            }
         }
         catch (Exception ex)
         {
            sb.AppendLine($"  ERROR iterating party members: {ex}");
            sb.AppendLine();
         }
      }

      private static void DumpHeroEquipment(StringBuilder sb, Hero hero)
      {
         sb.AppendLine($"========== {hero.Name} Equipment ==========");
         sb.AppendLine();

         try
         {
            for (var i = 0; i < (int) EquipmentIndex.NumEquipmentSetSlots; i++)
            {
               var index = (EquipmentIndex) i;
               var slot = hero.BattleEquipment.GetEquipmentFromSlot(index);
               if (slot.IsEmpty)
               {
                  sb.AppendLine($"  [{index}] (empty)");
                  continue;
               }

               sb.Append($"  [{index}] ");
               AppendItemElement(sb, slot, 1);
            }
         }
         catch (Exception ex)
         {
            sb.AppendLine($"  ERROR: {ex}");
         }

         sb.AppendLine();
      }

      private static void AppendItemElement(StringBuilder sb, EquipmentElement element, int amount)
      {
         var item = element.Item;
         if (amount > 1)
            sb.AppendLine($"{item.Name} x{amount}");
         else
            sb.AppendLine($"{item.Name}");

         sb.AppendLine($"    Id:         {item.StringId}");
         sb.AppendLine($"    ItemType:   {item.ItemType}");
         sb.AppendLine($"    Tier:       {item.Tier}");
         sb.AppendLine($"    Value:      {element.ItemValue}");
         sb.AppendLine($"    Difficulty: {item.Difficulty}");
         if (item.RelevantSkill != null)
            sb.AppendLine($"    Skill:      {item.RelevantSkill.Name}");

         if (item.HasWeaponComponent)
         {
            var weapon = item.PrimaryWeapon;
            var usage = weapon.ItemUsage;
            var flags = !string.IsNullOrEmpty(usage)
               ? MBItem.GetItemUsageSetFlags(usage)
               : 0;

            sb.AppendLine($"    ItemUsage:  {usage}");
            sb.AppendLine($"    UsageFlags: {flags}");
            sb.AppendLine($"    ThrustDmg:  {element.GetModifiedThrustDamageForUsage(0)}");
            sb.AppendLine($"    SwingDmg:   {element.GetModifiedSwingDamageForUsage(0)}");
            sb.AppendLine($"    ThrustSpd:  {element.GetModifiedThrustSpeedForUsage(0)}");
            sb.AppendLine($"    SwingSpd:   {element.GetModifiedSwingSpeedForUsage(0)}");
            sb.AppendLine($"    Handling:   {element.GetModifiedHandlingForUsage(0)}");
            sb.AppendLine($"    MissileDmg: {element.GetModifiedMissileDamageForUsage(0)}");
            sb.AppendLine($"    MissileSpd: {element.GetModifiedMissileSpeedForUsage(0)}");
         }
         else if (item.HasArmorComponent)
         {
            sb.AppendLine($"    HeadArmor:  {element.GetModifiedHeadArmor()}");
            sb.AppendLine($"    BodyArmor:  {element.GetModifiedBodyArmor()}");
            sb.AppendLine($"    LegArmor:   {element.GetModifiedLegArmor()}");
            sb.AppendLine($"    ArmArmor:   {element.GetModifiedArmArmor()}");
         }
      }
   }
}
