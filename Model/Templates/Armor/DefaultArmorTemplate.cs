using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Armor
{
   public class DefaultArmorTemplate : BaseArmorTemplate
   {
      public static readonly DefaultArmorTemplate Instance = new DefaultArmorTemplate();

      public override string Name => "default_armor";
      public override string DisplayName => "Armor";
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
