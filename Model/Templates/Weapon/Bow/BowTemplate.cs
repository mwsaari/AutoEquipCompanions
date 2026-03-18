using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Bow
{
   public class BowTemplate : BaseWeaponTemplate
   {
      public static readonly BowTemplate Instance = new BowTemplate();

      public override string Name => "bow";
      public override string DisplayName => "Bow";
      public override WeaponField ComparisonField => WeaponField.Value;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Bow
      };
   }
}
