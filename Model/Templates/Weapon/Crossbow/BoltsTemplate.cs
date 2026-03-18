using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Crossbow
{
   public class BoltsTemplate : BaseWeaponTemplate
   {
      public static readonly BoltsTemplate Instance = new BoltsTemplate();

      public override string Name => "bolts";
      public override string DisplayName => "Bolts";
      public override WeaponField ComparisonField => WeaponField.MissileDamage;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Bolts
      };
   }
}
