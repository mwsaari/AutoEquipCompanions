using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public class AmmoTemplate : BaseWeaponTemplate
   {
      public static readonly AmmoTemplate Instance = new AmmoTemplate();

      public override string Name => "ammo";
      public override string DisplayName => "Ammo";
      public override WeaponField ComparisonField => WeaponField.MissileDamage;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Arrows,
         ItemObject.ItemTypeEnum.Bolts
      };
   }
}
