using AutoEquipCompanions.Model.Templates;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon.Base
{
   public class IsBetterThanTests
   {
      [Fact]
      public void EqualScore_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon);
         Assert.False(ExposedWeaponTemplate.Instance.IsBetterThan(el, el));
      }

      [Fact]
      public void BothEmpty_ReturnsFalse()
      {
         Assert.False(ExposedWeaponTemplate.Instance.IsBetterThan(default, default));
      }
   }
}
