using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Armor
{
   public class MediumArmorTemplate : BaseArmorTemplate
   {
      public static readonly MediumArmorTemplate Instance = new MediumArmorTemplate();

      public override string Name => "medium_armor";
      public override string DisplayName => "Medium Armor";
      public override ArmorField ComparisonField => ArmorField.ArmorTotal;
      protected override ItemObject.ItemTiers? MaxTier => ItemObject.ItemTiers.Tier3;

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
