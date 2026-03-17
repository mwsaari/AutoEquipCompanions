using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Armor
{
   public class HeavyArmorTemplate : BaseArmorTemplate
   {
      public static readonly HeavyArmorTemplate Instance = new HeavyArmorTemplate();

      public override string Name => "heavy_armor";
      public override string DisplayName => "Heavy Armor";
      public override ArmorField ComparisonField => ArmorField.ArmorTotal;

      public override IEnumerable<EquipmentIndex> LegalSlots { get; } = new[]
      {
         EquipmentIndex.Head,
         EquipmentIndex.Cape,
         EquipmentIndex.Body,
         EquipmentIndex.Gloves,
         EquipmentIndex.Leg
      };
   }
}
