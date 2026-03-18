using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.OneHanded
{
   public class OneHandedWeaponTemplate : BaseWeaponTemplate
   {
      public static readonly OneHandedWeaponTemplate Instance = new OneHandedWeaponTemplate();

      public override string Name => "one_handed_weapon";
      public override string DisplayName => "One-Handed";
      public override WeaponField ComparisonField => WeaponField.Value;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.OneHandedWeapon,
         ItemObject.ItemTypeEnum.TwoHandedWeapon
      };

      public override bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (candidate.IsEmpty || !candidate.Item.HasWeaponComponent)
            return false;
         var isOneHanded = candidate.Item.ItemType == ItemObject.ItemTypeEnum.OneHandedWeapon;
         var isBastard = Main.GameSettings.BastardSwordsAreOneHanded && IsBastardSword(candidate);
         if (!isOneHanded && !isBastard)
            return false;
         return MeetsDifficultyRequirement(candidate, hero);
      }
   }
}
