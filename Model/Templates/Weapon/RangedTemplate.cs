using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public class RangedTemplate : BaseWeaponTemplate
   {
      public static readonly RangedTemplate Instance = new RangedTemplate();

      public override string Name => "ranged";
      public override string DisplayName => "Ranged";
      public override WeaponField ComparisonField => WeaponField.Value;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Bow,
         ItemObject.ItemTypeEnum.Crossbow
      };
   }
}
