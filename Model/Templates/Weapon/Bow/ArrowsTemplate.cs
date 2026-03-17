using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Bow
{
   public class ArrowsTemplate : BaseWeaponTemplate
   {
      public static readonly ArrowsTemplate Instance = new ArrowsTemplate();

      public override string Name => "arrows";
      public override string DisplayName => "Arrows";
      public override WeaponField ComparisonField => WeaponField.MissileDamage;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Arrows
      };
   }
}
