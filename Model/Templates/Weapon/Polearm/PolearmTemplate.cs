using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Polearm
{
   public class PolearmTemplate : BaseWeaponTemplate
   {
      public static readonly PolearmTemplate Instance = new PolearmTemplate();

      public override string Name => "polearm";
      public override string DisplayName => "Polearm";
      public override WeaponField ComparisonField => WeaponField.Value;

      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.Polearm
      };
   }
}
