using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Thrown
{
   public class ThrownTemplate : BaseWeaponTemplate
   {
      public static readonly ThrownTemplate Instance = new ThrownTemplate();

      public override string Name => "thrown";
      public override string DisplayName => "Thrown";
      public override WeaponField ComparisonField => WeaponField.MissileDamage;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Thrown
      };
   }
}
