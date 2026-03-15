using AutoEquipCompanions.Model.Templates.Base;

namespace AutoEquipCompanions.Model.Templates
{
   public class DefaultMountTemplate : BaseMountTemplate
   {
      public static readonly DefaultMountTemplate Instance = new DefaultMountTemplate();

      public override string Name => "Default Mount";
      public override bool UseCamel => false;
      public override HorseField HorseComparisonField => HorseField.Value;
      public override HarnessField HarnessComparisonField => HarnessField.Value;
   }

   public class CamelMountTemplate : DefaultMountTemplate
   {
      public new static readonly CamelMountTemplate Instance = new CamelMountTemplate();

      public override string Name => "Camel Mount";
      public override bool UseCamel => true;
   }
}
