using AutoEquipCompanions.Model.Templates;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public abstract class BaseWeaponTemplate : ISlotTemplate
   {
      public enum WeaponField
      {
         Value,
         ThrustDamage,
         SwingDamage,
         MissileDamage,
         HighestDamage,
         ThrustSpeed,
         SwingSpeed,
         MissileSpeed,
         Handling,
         StackCount
      }

      public abstract IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; }

      public abstract WeaponField ComparisonField { get; }
      public virtual ItemObject.ItemUsageSetFlags RequiredUsageFlags { get; set; } = 0;
      public virtual ItemObject.ItemUsageSetFlags ExcludedUsageFlags { get; set; } = 0;

      public abstract string Name { get; }

      public virtual IEnumerable<EquipmentIndex> LegalSlots { get; } = new[]
      {
         EquipmentIndex.Weapon0,
         EquipmentIndex.Weapon1,
         EquipmentIndex.Weapon2,
         EquipmentIndex.Weapon3
      };

      public virtual bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (candidate.IsEmpty || !candidate.Item.HasWeaponComponent)
            return false;
         if (!IsAllowedItemType(candidate.Item.ItemType))
            return false;

         if (RequiredUsageFlags != 0 || ExcludedUsageFlags != 0)
         {
            var itemUsage = candidate.Item.PrimaryWeapon.ItemUsage;
            var usageFlags = !string.IsNullOrEmpty(itemUsage)
               ? MBItem.GetItemUsageSetFlags(itemUsage)
               : 0;
            if (!usageFlags.HasFlag(RequiredUsageFlags))
               return false;
            if ((usageFlags & ExcludedUsageFlags) != 0)
               return false;
         }

         return MeetsDifficultyRequirement(candidate, hero);
      }

      protected static bool MeetsDifficultyRequirement(EquipmentElement element, Hero hero)
      {
         var item = element.Item;
         if (item.Difficulty <= 0 || item.RelevantSkill == null)
            return true;
         return hero.GetSkillValue(item.RelevantSkill) >= item.Difficulty;
      }

      protected static bool IsBastardSword(EquipmentElement element)
      {
         var usage = element.Item.PrimaryWeapon?.ItemUsage;
         return !string.IsNullOrEmpty(usage) && usage.Contains("rshield");
      }

      public double GetScore(EquipmentElement candidate)
      {
         if (candidate.IsEmpty)
            return 0;

         switch (ComparisonField)
         {
            case WeaponField.ThrustDamage: return candidate.GetModifiedThrustDamageForUsage(0);
            case WeaponField.SwingDamage: return candidate.GetModifiedSwingDamageForUsage(0);
            case WeaponField.MissileDamage: return candidate.GetModifiedMissileDamageForUsage(0);
            case WeaponField.HighestDamage:
               return Math.Max(
                  Math.Max(candidate.GetModifiedThrustDamageForUsage(0), candidate.GetModifiedSwingDamageForUsage(0)),
                  candidate.GetModifiedMissileDamageForUsage(0));
            case WeaponField.ThrustSpeed: return candidate.GetModifiedThrustSpeedForUsage(0);
            case WeaponField.SwingSpeed: return candidate.GetModifiedSwingSpeedForUsage(0);
            case WeaponField.MissileSpeed: return candidate.GetModifiedMissileSpeedForUsage(0);
            case WeaponField.Handling: return candidate.GetModifiedHandlingForUsage(0);
            case WeaponField.StackCount: return candidate.GetModifiedStackCountForUsage(0);
            default: return candidate.ItemValue;
         }
      }

      private bool IsAllowedItemType(ItemObject.ItemTypeEnum itemType)
      {
         foreach (var allowed in AllowedItemTypes)
         {
            if (itemType == allowed)
               return true;
         }
         return false;
      }
   }
}
