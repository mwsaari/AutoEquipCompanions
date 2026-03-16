using AutoEquipCompanions.Model.Templates.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates
{
   public class DefaultWeaponTemplate : BaseWeaponTemplate
   {
      public static readonly DefaultWeaponTemplate Instance = new DefaultWeaponTemplate();

      public override string Name => "Default Weapon";
      public override WeaponField ComparisonField => WeaponField.Value;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.OneHandedWeapon,
         ItemObject.ItemTypeEnum.TwoHandedWeapon,
         ItemObject.ItemTypeEnum.Polearm,
         ItemObject.ItemTypeEnum.Thrown,
         ItemObject.ItemTypeEnum.Bow,
         ItemObject.ItemTypeEnum.Crossbow
      };
   }

   public class SameTypeWeaponTemplate : BaseWeaponTemplate
   {
      public static readonly SameTypeWeaponTemplate Instance = new SameTypeWeaponTemplate();

      public override string Name => "Same Type Weapon";
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
         var candidateType = GetEffectiveType(candidate);
         var currentType = GetEffectiveType(current);
         return candidateType == currentType;
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

   public class MissileTemplate : BaseWeaponTemplate
   {
      public static readonly MissileTemplate Instance = new MissileTemplate();

      public override string Name => "Missile";
      public override WeaponField ComparisonField => WeaponField.MissileDamage;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Arrows,
         ItemObject.ItemTypeEnum.Bolts
      };
   }

   public class MountWeaponTemplate : DefaultWeaponTemplate
   {
      public new static readonly MountWeaponTemplate Instance = new MountWeaponTemplate();

      public override string Name => "Mount Weapon";
      public override ItemObject.ItemUsageSetFlags ExcludedUsageFlags { get; set; } = ItemObject.ItemUsageSetFlags.RequiresNoMount;
   }

   public class CouchableLanceTemplate : MountWeaponTemplate
   {
      public new static readonly CouchableLanceTemplate Instance = new CouchableLanceTemplate();

      public override string Name => "Couchable Lance";

      public override bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (!base.IsValidFor(candidate, slot, hero))
            return false;

         return candidate.Item.Weapons.Any(x =>
            x.WeaponDescriptionId != null && x.WeaponDescriptionId.IndexOf("couch", StringComparison.OrdinalIgnoreCase) >= 0);
      }
   }
}
