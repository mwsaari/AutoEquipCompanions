using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public class MissileTemplate : BaseWeaponTemplate
   {
      public static readonly MissileTemplate Instance = new MissileTemplate();

      public override string Name => "Missile";
      public override WeaponField ComparisonField => WeaponField.MissileDamage;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Arrows,
         ItemObject.ItemTypeEnum.Bolts
      };
   }
}
