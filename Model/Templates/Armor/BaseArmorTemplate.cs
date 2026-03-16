using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Armor
{
   public abstract class BaseArmorTemplate : ISlotTemplate
   {
      public enum ArmorField
      {
         Value,
         ArmorTotal,
         HeadArmor,
         BodyArmor,
         ArmArmor,
         LegArmor
      }

      public abstract ArmorField ComparisonField { get; }

      public abstract string Name { get; }
      public abstract IEnumerable<EquipmentIndex> LegalSlots { get; }

      public bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (candidate.IsEmpty || !candidate.Item.HasArmorComponent)
            return false;

         switch (slot)
         {
            case EquipmentIndex.Head: return candidate.Item.ItemType == ItemObject.ItemTypeEnum.HeadArmor;
            case EquipmentIndex.Cape: return candidate.Item.ItemType == ItemObject.ItemTypeEnum.Cape;
            case EquipmentIndex.Body: return candidate.Item.ItemType == ItemObject.ItemTypeEnum.BodyArmor;
            case EquipmentIndex.Gloves: return candidate.Item.ItemType == ItemObject.ItemTypeEnum.HandArmor;
            case EquipmentIndex.Leg: return candidate.Item.ItemType == ItemObject.ItemTypeEnum.LegArmor;
            default: return false;
         }
      }

      public double GetScore(EquipmentElement candidate)
      {
         if (candidate.IsEmpty)
            return 0;

         switch (ComparisonField)
         {
            case ArmorField.ArmorTotal:
               return candidate.GetModifiedHeadArmor()
                  + candidate.GetModifiedBodyArmor()
                  + candidate.GetModifiedArmArmor()
                  + candidate.GetModifiedLegArmor();
            case ArmorField.HeadArmor: return candidate.GetModifiedHeadArmor();
            case ArmorField.BodyArmor: return candidate.GetModifiedBodyArmor();
            case ArmorField.ArmArmor: return candidate.GetModifiedArmArmor();
            case ArmorField.LegArmor: return candidate.GetModifiedLegArmor();
            default: return candidate.ItemValue;
         }
      }
   }
}
