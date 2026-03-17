using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public class DefaultWeaponTemplate : BaseWeaponTemplate
   {
      public static readonly DefaultWeaponTemplate Instance = new DefaultWeaponTemplate();

      public override string Name => "default_weapon";
      public override string DisplayName => "Weapon";
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
}
