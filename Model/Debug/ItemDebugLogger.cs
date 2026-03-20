using System;
using System.Collections.Generic;
using System.Linq;
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
         var timestamp = DateTime.Now.ToString();
         var byType = new SortedDictionary<ItemObject.ItemTypeEnum, StringBuilder>();

         CollectLeftInventory(byType, inventoryLogic, timestamp);
         CollectPartyInventory(byType, timestamp);

         foreach (var kvp in byType)
            Logger.WriteToItemDebug($"debug_items_{kvp.Key}.txt", kvp.Value.ToString());

         var heroSb = new StringBuilder();
         AppendHeader(heroSb, timestamp);
         DumpAllHeroEquipment(heroSb);
         Logger.WriteToItemDebug("debug_items_heroes.txt", heroSb.ToString());
      }

      private static void CollectLeftInventory(
         SortedDictionary<ItemObject.ItemTypeEnum, StringBuilder> byType,
         InventoryLogic inventoryLogic,
         string timestamp)
      {
         try
         {
            var rosters = typeof (InventoryLogic)
               .GetField("_rosters", BindingFlags.NonPublic | BindingFlags.Instance)
               ?.GetValue(inventoryLogic) as ItemRoster[];

            if (rosters == null || rosters.Length == 0)
               return;

            CollectItems(byType, rosters[0].Where(e => !e.IsEmpty).ToList(), "Left Inventory (Loot/Merchant)", timestamp);
         }
         catch { }
      }

      private static void CollectPartyInventory(
         SortedDictionary<ItemObject.ItemTypeEnum, StringBuilder> byType,
         string timestamp)
      {
         try
         {
            CollectItems(byType, MobileParty.MainParty.ItemRoster.Where(e => !e.IsEmpty).ToList(), "Party Inventory", timestamp);
         }
         catch { }
      }

      private static void CollectItems(
         SortedDictionary<ItemObject.ItemTypeEnum, StringBuilder> byType,
         List<ItemRosterElement> items,
         string sectionName,
         string timestamp)
      {
         var groups = items
            .GroupBy(e => e.EquipmentElement.Item.ItemType)
            .OrderBy(g => g.Key.ToString());

         foreach (var group in groups)
         {
            if (!byType.TryGetValue(group.Key, out var sb))
            {
               sb = new StringBuilder();
               AppendHeader(sb, timestamp);
               byType[group.Key] = sb;
            }

            sb.AppendLine($"========== {sectionName} ==========");
            sb.AppendLine();

            foreach (var e in group.OrderBy(e => e.EquipmentElement.Item.Name.ToString()))
               AppendItemElement(sb, e.EquipmentElement, e.Amount);

            sb.AppendLine();
         }
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

      private static void AppendHeader(StringBuilder sb, string timestamp)
      {
         sb.AppendLine($"=== AutoEquipCompanions Item Dump: {timestamp} ===");
         sb.AppendLine();
      }

      private static void AppendItemElement(StringBuilder sb, EquipmentElement element, int amount)
      {
         var item = element.Item;
         var modifier = element.ItemModifier;

         if (amount > 1)
            sb.AppendLine($"{item.Name} x{amount}");
         else
            sb.AppendLine($"{item.Name}");

         sb.AppendLine($"    Id:          {item.StringId}");
         sb.AppendLine($"    ItemType:    {item.ItemType}");
         sb.AppendLine($"    Tier:        {item.Tier}");
         sb.AppendLine($"    Value:       {element.ItemValue}");
         sb.AppendLine($"    Weight:      {item.Weight:F2}");
         sb.AppendLine($"    Difficulty:  {item.Difficulty}");
         if (item.RelevantSkill != null)
            sb.AppendLine($"    Skill:       {item.RelevantSkill.Name}");
         sb.AppendLine($"    Culture:     {item.Culture?.StringId ?? "—"}");
         sb.AppendLine($"    Category:    {item.ItemCategory?.StringId ?? "—"}");
         sb.AppendLine($"    Civilian:    {item.IsCivilian}");
         sb.AppendLine($"    Unique:      {item.IsUniqueItem}");
         sb.AppendLine($"    Crafted:     {item.IsCraftedWeapon}");
         sb.AppendLine($"    Appearance:  {item.Appearance:F2}");
         sb.AppendLine($"    Effectivns:  {item.Effectiveness:F2}");
         sb.AppendLine($"    ItemFlags:   {item.ItemFlags}");

         if (modifier != null)
            sb.AppendLine($"    Modifier:    {modifier.Name} ({modifier.ItemQuality})");

         if (item.HasWeaponComponent)
            AppendWeaponBlock(sb, element, item);
         else if (item.ItemType == ItemObject.ItemTypeEnum.Horse)
            AppendHorseBlock(sb, element, item);
         else if (item.ItemType == ItemObject.ItemTypeEnum.HorseHarness)
            AppendHarnessBlock(sb, element, item);
         else if (item.HasArmorComponent)
            AppendArmorBlock(sb, element, item);
         else if (item.HasBannerComponent)
            AppendBannerBlock(sb, item);

         sb.AppendLine();
      }

      private static void AppendWeaponBlock(StringBuilder sb, EquipmentElement element, ItemObject item)
      {
         var w = item.PrimaryWeapon;
         var usage = w.ItemUsage;
         var usageFlags = !string.IsNullOrEmpty(usage) ? MBItem.GetItemUsageSetFlags(usage) : 0;

         sb.AppendLine($"    WeaponDesc:  {w.WeaponDescriptionId}");
         sb.AppendLine($"    WeaponClass: {w.WeaponClass}");
         sb.AppendLine($"    WeaponTier:  {w.WeaponTier}");
         sb.AppendLine($"    Usages:      {item.Weapons.Count}");
         sb.AppendLine($"    ItemUsage:   {usage}");
         sb.AppendLine($"    UsageFlags:  {usageFlags}");
         sb.AppendLine($"    WeaponFlags: {w.WeaponFlags}");
         sb.AppendLine($"    Length:      {w.WeaponLength}");
         sb.AppendLine($"    SweetSpot:   {w.SweetSpotReach:F2}");
         sb.AppendLine($"    Accuracy:    {w.Accuracy}");
         sb.AppendLine($"    MaxData:     {w.MaxDataValue}");
         sb.AppendLine($"    ThrustDmg:   {element.GetModifiedThrustDamageForUsage(0)} ({w.ThrustDamageType})");
         sb.AppendLine($"    ThrustSpd:   {element.GetModifiedThrustSpeedForUsage(0)}");
         sb.AppendLine($"    SwingDmg:    {element.GetModifiedSwingDamageForUsage(0)} ({w.SwingDamageType})");
         sb.AppendLine($"    SwingSpd:    {element.GetModifiedSwingSpeedForUsage(0)}");
         sb.AppendLine($"    Handling:    {element.GetModifiedHandlingForUsage(0)}");
         sb.AppendLine($"    MissileDmg:  {element.GetModifiedMissileDamageForUsage(0)}");
         sb.AppendLine($"    MissileSpd:  {element.GetModifiedMissileSpeedForUsage(0)}");
         sb.AppendLine($"    AmmoClass:   {w.AmmoClass}");
         sb.AppendLine($"    ReloadPhase: {w.ReloadPhaseCount}");
      }

      private static void AppendHorseBlock(StringBuilder sb, EquipmentElement element, ItemObject item)
      {
         var h = item.HorseComponent;

         sb.AppendLine($"    Rideable:    {h.IsRideable}");
         sb.AppendLine($"    PackAnimal:  {h.IsPackAnimal}");
         sb.AppendLine($"    Speed:       {element.GetModifiedMountSpeed(default (EquipmentElement))} (base {h.Speed})");
         sb.AppendLine($"    Maneuver:    {element.GetModifiedMountManeuver(default (EquipmentElement))} (base {h.Maneuver})");
         sb.AppendLine($"    Charge:      {element.GetModifiedMountCharge(default (EquipmentElement))} (base {h.ChargeDamage})");
         sb.AppendLine($"    HitPoints:   {element.GetModifiedMountHitPoints()} (base {h.HitPoints} + bonus {h.HitPointBonus})");
      }

      private static void AppendHarnessBlock(StringBuilder sb, EquipmentElement element, ItemObject item)
      {
         var armor = item.ArmorComponent;

         sb.AppendLine($"    Material:    {armor.MaterialType}");
         sb.AppendLine($"    FamilyType:  {armor.FamilyType}");
         sb.AppendLine($"    MountArmor:  {element.GetModifiedMountBodyArmor()}");
         sb.AppendLine($"    SpeedBonus:  {armor.SpeedBonus}");
         sb.AppendLine($"    ManeuvrBons: {armor.ManeuverBonus}");
         sb.AppendLine($"    ChargeBonus: {armor.ChargeBonus}");
      }

      private static void AppendArmorBlock(StringBuilder sb, EquipmentElement element, ItemObject item)
      {
         var armor = item.ArmorComponent;

         sb.AppendLine($"    Material:    {armor.MaterialType}");
         sb.AppendLine($"    MeshType:    {armor.BodyMeshType}");
         sb.AppendLine($"    FamilyType:  {armor.FamilyType}");
         sb.AppendLine($"    HeadArmor:   {element.GetModifiedHeadArmor()}");
         sb.AppendLine($"    BodyArmor:   {element.GetModifiedBodyArmor()}");
         sb.AppendLine($"    LegArmor:    {element.GetModifiedLegArmor()}");
         sb.AppendLine($"    ArmArmor:    {element.GetModifiedArmArmor()}");
         sb.AppendLine($"    Stealth:     {armor.StealthFactor}");
      }

      private static void AppendBannerBlock(StringBuilder sb, ItemObject item)
      {
         var banner = item.BannerComponent;

         sb.AppendLine($"    BannerLevel: {banner.BannerLevel}");
         sb.AppendLine($"    BannerEffect:{banner.BannerEffect?.StringId ?? "—"}");
         sb.AppendLine($"    BannerBonus: {banner.GetBannerEffectBonus():F2}");
      }
   }
}
