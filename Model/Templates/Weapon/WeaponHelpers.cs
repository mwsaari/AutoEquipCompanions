using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public static class WeaponHelpers
   {
      public static bool RequiresNoMount(EquipmentElement element)
      {
         var itemUsage = element.Item.PrimaryWeapon?.ItemUsage;
         if (string.IsNullOrEmpty(itemUsage))
            return false;
         return MBItem.GetItemUsageSetFlags(itemUsage)
            .HasFlag(ItemObject.ItemUsageSetFlags.RequiresNoMount);
      }

      public static bool IsCouchable(EquipmentElement element)
      {
         return element.Item.Weapons.Any(w =>
            w.WeaponDescriptionId != null &&
            w.WeaponDescriptionId.IndexOf("couch", StringComparison.OrdinalIgnoreCase) >= 0);
      }
   }
}
