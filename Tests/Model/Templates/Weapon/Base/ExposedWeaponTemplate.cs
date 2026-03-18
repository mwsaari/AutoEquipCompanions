using AutoEquipCompanions.Model.Templates.Weapon;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon.Base
{
   // Exposes protected members of BaseWeaponTemplate for direct testing
   internal class ExposedWeaponTemplate : BaseWeaponTemplate
   {
      public static readonly ExposedWeaponTemplate Instance = new ExposedWeaponTemplate();

      public override string Name => "Exposed";
      public override string DisplayName => "Exposed";
      public override WeaponField ComparisonField => WeaponField.Value;
      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.OneHandedWeapon,
         ItemObject.ItemTypeEnum.TwoHandedWeapon,
         ItemObject.ItemTypeEnum.Polearm,
         ItemObject.ItemTypeEnum.Bow,
         ItemObject.ItemTypeEnum.Crossbow,
         ItemObject.ItemTypeEnum.Thrown,
      };

      public bool TestIsBastardSword(EquipmentElement e) => IsBastardSword(e);
      public bool TestMeetsDifficulty(EquipmentElement e, Hero h) => MeetsDifficultyRequirement(e, h);
   }
}
