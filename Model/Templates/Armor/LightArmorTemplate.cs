using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Armor
{
   public class LightArmorTemplate : BaseArmorTemplate
   {
      public static readonly LightArmorTemplate Instance = new LightArmorTemplate();

      public override string Name => "light_armor";
      public override string DisplayName => "Light Armor";
      public override ArmorField ComparisonField => ArmorField.ArmorTotal;
      protected override ItemObject.ItemTiers? MaxTier => ItemObject.ItemTiers.Tier2;

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
