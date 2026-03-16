namespace AutoEquipCompanions.Model.Templates.Mount
{
   public class CamelMountTemplate : DefaultMountTemplate
   {
      public new static readonly CamelMountTemplate Instance = new CamelMountTemplate();

      public override string Name => "Camel Mount";
      public override bool UseCamel => true;
   }
}
