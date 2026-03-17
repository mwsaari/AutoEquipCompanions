namespace AutoEquipCompanions.Model.Templates.Mount
{
   public class DefaultMountTemplate : BaseMountTemplate
   {
      public static readonly DefaultMountTemplate Instance = new DefaultMountTemplate();

      public override string Name => "default_mount";
      public override string DisplayName => "Horse";
      public override bool UseCamel => false;
      public override HorseField HorseComparisonField => HorseField.Value;
      public override HarnessField HarnessComparisonField => HarnessField.MountBodyArmor;
   }
}
