using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
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
         ItemObject.ItemTypeEnum.OneHandedWeapon
      };
   }
}
