using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Crossbow
{
   public class CrossbowTemplate : BaseWeaponTemplate
   {
      public static readonly CrossbowTemplate Instance = new CrossbowTemplate();

      public override string Name => "crossbow";
      public override string DisplayName => "Crossbow";
      public override WeaponField ComparisonField => WeaponField.Value;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Crossbow
      };
   }
}
