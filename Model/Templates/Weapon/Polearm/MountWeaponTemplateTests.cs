using AutoEquipCompanions.Model.Templates.Weapon.Polearm;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon
{
   public class MountWeaponTemplateTests
   {
      [Fact]
      public void PolearmType_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm);
         var hero = Helpers.MakeHero();
         Assert.True(MountWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(MountWeaponTemplate.Instance.IsValidFor(default, EquipmentIndex.Weapon0, hero));
      }

      [Fact]
      public void EmptyItemUsage_ExcludedFlagDoesNotReject()
      {
         // ExcludedUsageFlags = RequiresNoMount is only checked against real usage-flag data,
         // which this headless harness can't load (see MBItem.GetItemUsageSetFlags); an empty
         // usage string never carries that flag, so validity falls through to difficulty only.
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.Polearm, WeaponClass.OneHandedPolearm, itemUsage: "");
         var hero = Helpers.MakeHero();
         Assert.True(MountWeaponTemplate.Instance.IsValidFor(el, EquipmentIndex.Weapon0, hero));
      }
   }
}
