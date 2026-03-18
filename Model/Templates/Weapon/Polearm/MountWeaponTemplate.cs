using AutoEquipCompanions.Model.Templates.Weapon;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Polearm
{
   public class MountWeaponTemplate : DefaultWeaponTemplate
   {
      public new static readonly MountWeaponTemplate Instance = new MountWeaponTemplate();

      public override string Name => "mount_weapon";
      public override string DisplayName => "Mounted Combat";
      public override ItemObject.ItemUsageSetFlags ExcludedUsageFlags { get; set; } = ItemObject.ItemUsageSetFlags.RequiresNoMount;
   }
}
