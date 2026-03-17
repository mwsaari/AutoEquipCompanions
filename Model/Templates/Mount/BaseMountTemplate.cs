using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Mount
{
   public abstract class BaseMountTemplate : ISlotTemplate
   {
      public enum HarnessField
      {
         Value,
         MountBodyArmor
      }

      public enum HorseField
      {
         Value,
         Speed,
         Maneuver,
         ChargeDamage,
         HitPoints
      }

      public abstract HorseField HorseComparisonField { get; }
      public abstract HarnessField HarnessComparisonField { get; }
      public abstract bool UseCamel { get; }

      protected virtual ItemObject.ItemTiers? MaxTier => null;

      public abstract string Name { get; }
      public abstract string DisplayName { get; }

      public virtual IEnumerable<EquipmentIndex> LegalSlots { get; } = new[]
      {
         EquipmentIndex.Horse,
         EquipmentIndex.HorseHarness
      };

      public bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (candidate.IsEmpty)
            return false;

         switch (slot)
         {
            case EquipmentIndex.Horse:
               if (candidate.Item.ItemType != ItemObject.ItemTypeEnum.Horse)
                  return false;

               var isCamel = candidate.Item.ItemComponent is HorseComponent horse
                  && horse.Monster.MonsterUsage == "camel";
               if (UseCamel != isCamel)
                  return false;

               var item = candidate.Item;
               if (item.Difficulty > 0 && item.RelevantSkill != null)
                  return item.Difficulty <= hero.GetSkillValue(item.RelevantSkill);

               return true;
            case EquipmentIndex.HorseHarness:
               if (candidate.Item.ItemType != ItemObject.ItemTypeEnum.HorseHarness)
                  return false;

               if (MaxTier.HasValue && candidate.Item.Tier > MaxTier.Value)
                  return false;

               var equippedHorse = hero.BattleEquipment.Horse;
               if (!equippedHorse.IsEmpty)
                  return candidate.Item.ArmorComponent?.FamilyType == equippedHorse.Item.HorseComponent?.Monster.FamilyType;

               return true;
            default:
               return false;
         }
      }

      public double GetScore(EquipmentElement candidate)
      {
         if (candidate.IsEmpty)
            return 0;

         if (candidate.Item.ItemType == ItemObject.ItemTypeEnum.Horse)
         {
            var horse = candidate.Item.HorseComponent;
            if (horse == null)
               return 0;

            switch (HorseComparisonField)
            {
               case HorseField.Speed: return horse.Speed;
               case HorseField.Maneuver: return horse.Maneuver;
               case HorseField.ChargeDamage: return horse.ChargeDamage;
               case HorseField.HitPoints: return horse.HitPoints;
               default: return candidate.ItemValue;
            }
         }

         switch (HarnessComparisonField)
         {
            case HarnessField.MountBodyArmor: return candidate.GetModifiedMountBodyArmor();
            default: return candidate.ItemValue;
         }
      }
   }
}
