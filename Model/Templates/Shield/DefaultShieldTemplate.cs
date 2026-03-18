namespace AutoEquipCompanions.Model.Templates.Shield
{
   public class DefaultShieldTemplate : BaseShieldTemplate
   {
      public static readonly DefaultShieldTemplate Instance = new DefaultShieldTemplate();

      public override string Name => "default_shield";
      public override string DisplayName => "Shield";
      public override ShieldField ComparisonField => ShieldField.Value;
   }
}
