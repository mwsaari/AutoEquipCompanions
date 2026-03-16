using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public class MountWeaponTemplate : DefaultWeaponTemplate
   {
      public new static readonly MountWeaponTemplate Instance = new MountWeaponTemplate();

      public override string Name => "Mount Weapon";
      public override ItemObject.ItemUsageSetFlags ExcludedUsageFlags { get; set; } = ItemObject.ItemUsageSetFlags.RequiresNoMount;
   }
}
