using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Base
{
   public abstract class BaseShieldTemplate : ISlotTemplate
   {
      public enum ShieldField
      {
         Value,
         Armor,
         HitPoints
      }

      public abstract ShieldField ComparisonField { get; }

      public abstract string Name { get; }

      public virtual IEnumerable<EquipmentIndex> LegalSlots { get; } = new[]
      {
         EquipmentIndex.Weapon0,
         EquipmentIndex.Weapon1,
         EquipmentIndex.Weapon2,
         EquipmentIndex.Weapon3
      };

      public bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (candidate.IsEmpty || !candidate.Item.HasWeaponComponent)
            return false;
         if (candidate.Item.ItemType != ItemObject.ItemTypeEnum.Shield)
            return false;

         var item = candidate.Item;
         if (item.Difficulty > 0 && item.RelevantSkill != null)
            return item.Difficulty <= hero.GetSkillValue(item.RelevantSkill);

         return true;
      }

      public double GetScore(EquipmentElement candidate)
      {
         if (candidate.IsEmpty)
            return 0;

         switch (ComparisonField)
         {
            case ShieldField.Armor: return candidate.Item.PrimaryWeapon.BodyArmor;
            case ShieldField.HitPoints: return candidate.GetModifiedMaximumHitPointsForUsage(0);
            default: return candidate.ItemValue;
         }
      }
   }
}
