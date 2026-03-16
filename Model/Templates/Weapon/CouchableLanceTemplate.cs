using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon
{
   public class CouchableLanceTemplate : MountWeaponTemplate
   {
      public new static readonly CouchableLanceTemplate Instance = new CouchableLanceTemplate();

      public override string Name => "Couchable Lance";

      public override bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (!base.IsValidFor(candidate, slot, hero))
            return false;

         return candidate.Item.Weapons.Any(x =>
            x.WeaponDescriptionId != null && x.WeaponDescriptionId.IndexOf("couch", StringComparison.OrdinalIgnoreCase) >= 0);
      }
   }
}
