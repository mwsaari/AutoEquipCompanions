using AutoEquipCompanions.Model.Templates.Base;

namespace AutoEquipCompanions.Model.Templates
{
   public class DefaultShieldTemplate : BaseShieldTemplate
   {
      public static readonly DefaultShieldTemplate Instance = new DefaultShieldTemplate();

      public override string Name => "Default Shield";
      public override ShieldField ComparisonField => ShieldField.Value;
   }
}
