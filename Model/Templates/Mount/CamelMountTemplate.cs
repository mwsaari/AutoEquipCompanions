namespace AutoEquipCompanions.Model.Templates.Mount
{
   public class CamelMountTemplate : DefaultMountTemplate
   {
      public new static readonly CamelMountTemplate Instance = new CamelMountTemplate();

      public override string Name => "camel_mount";
      public override string DisplayName => "Camel";
      public override bool UseCamel => true;
   }
}
