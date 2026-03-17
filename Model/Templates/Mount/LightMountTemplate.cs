using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Mount
{
   public class LightMountTemplate : BaseMountTemplate
   {
      public static readonly LightMountTemplate Instance = new LightMountTemplate();

      public override string Name => "light_mount";
      public override string DisplayName => "Light Horse";
      public override bool UseCamel => false;
      public override HorseField HorseComparisonField => HorseField.Speed;
      public override HarnessField HarnessComparisonField => HarnessField.MountBodyArmor;
      protected override ItemObject.ItemTiers? MaxTier => ItemObject.ItemTiers.Tier2;
   }
}
