using AutoEquipCompanions.Model.Templates;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public class SameTypeWeaponTemplate : BaseWeaponTemplate
   {
      public static readonly SameTypeWeaponTemplate Instance = new SameTypeWeaponTemplate();

      public override string Name => "same_type_weapon";
      public override string DisplayName => "Match Current Type";
      public override WeaponField ComparisonField => WeaponField.Value;
      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = Array.Empty<ItemObject.ItemTypeEnum>();

      public override bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (candidate.IsEmpty || !candidate.Item.HasWeaponComponent)
            return false;

         var current = hero.BattleEquipment.GetEquipmentFromSlot(slot);
         if (current.IsEmpty)
            return false;

         if (!IsSameEffectiveType(candidate, current))
            return false;

         return MeetsDifficultyRequirement(candidate, hero);
      }

      private static bool IsSameEffectiveType(EquipmentElement candidate, EquipmentElement current)
      {
         return GetEffectiveType(candidate) == GetEffectiveType(current);
      }

      private static ItemObject.ItemTypeEnum GetEffectiveType(EquipmentElement element)
      {
         if (Main.GameSettings.BastardSwordsAreOneHanded
             && element.Item.ItemType == ItemObject.ItemTypeEnum.TwoHandedWeapon
             && IsBastardSword(element))
            return ItemObject.ItemTypeEnum.OneHandedWeapon;
         return element.Item.ItemType;
      }
   }
}
