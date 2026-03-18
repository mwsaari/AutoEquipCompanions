using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Weapon.Polearm
{
   public class CouchableLanceTemplate : MountWeaponTemplate
   {
      public new static readonly CouchableLanceTemplate Instance = new CouchableLanceTemplate();

      public override string Name => "couchable_lance";
      public override string DisplayName => "Couchable Lance";

      public override bool IsValidFor(EquipmentElement candidate, EquipmentIndex slot, Hero hero)
      {
         if (!base.IsValidFor(candidate, slot, hero))
            return false;

         return WeaponHelpers.IsCouchable(candidate);
      }
   }
}
