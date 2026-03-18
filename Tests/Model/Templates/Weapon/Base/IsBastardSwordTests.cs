using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon.Base
{
   public class IsBastardSwordTests
   {
      [Fact]
      public void EmptyUsage_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon, itemUsage: "");
         Assert.False(ExposedWeaponTemplate.Instance.TestIsBastardSword(el));
      }

      [Fact]
      public void TrueTwoHanded_NoRshield_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            itemUsage: "twohanded_block_swing_thrust");
         Assert.False(ExposedWeaponTemplate.Instance.TestIsBastardSword(el));
      }

      [Theory]
      [InlineData("onehanded_block_rshield_swing_thrust")]
      [InlineData("onehanded_block_rshield_swing")]
      [InlineData("onehanded_rshield_axe")]
      [InlineData("onehanded_polearm_block_long_rshield_thrust")]
      [InlineData("onehanded_polearm_block_rshield_thrust")]
      public void RshieldUsages_ReturnTrue(string usage)
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon, itemUsage: usage);
         Assert.True(ExposedWeaponTemplate.Instance.TestIsBastardSword(el));
      }

      [Fact]
      public void CheckIsUsageBased_NotTypeBased()
      {
         // IsBastardSword only checks the usage string, not ItemType
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            itemUsage: "onehanded_block_rshield_swing_thrust");
         Assert.True(ExposedWeaponTemplate.Instance.TestIsBastardSword(el));
      }
   }
}
