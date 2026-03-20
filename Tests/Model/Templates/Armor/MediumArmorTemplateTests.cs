using AutoEquipCompanions.Model.Templates.Armor;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Armor
{
   public class MediumArmorTemplateTests
   {
      [Theory]
      [InlineData(ItemObject.ItemTiers.Tier1)]
      [InlineData(ItemObject.ItemTiers.Tier2)]
      [InlineData(ItemObject.ItemTiers.Tier3)]
      public void WithinTierCap_ReturnsTrue(ItemObject.ItemTiers tier)
      {
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor, tier);
         var hero = Helpers.MakeHero();
         Assert.True(MediumArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      [Theory]
      [InlineData(ItemObject.ItemTiers.Tier4)]
      [InlineData(ItemObject.ItemTiers.Tier5)]
      [InlineData(ItemObject.ItemTiers.Tier6)]
      public void AboveTierCap_ReturnsFalse(ItemObject.ItemTiers tier)
      {
         var el = Helpers.MakeArmor(ItemObject.ItemTypeEnum.HeadArmor, tier);
         var hero = Helpers.MakeHero();
         Assert.False(MediumArmorTemplate.Instance.IsValidFor(el, EquipmentIndex.Head, hero));
      }

      [Fact]
      public void EmptyCandidate_ReturnsFalse()
      {
         var hero = Helpers.MakeHero();
         Assert.False(MediumArmorTemplate.Instance.IsValidFor(default, EquipmentIndex.Head, hero));
      }
   }
}
