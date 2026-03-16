using System.Collections.Generic;
using AutoEquipCompanions.Model.Templates.Base;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Base
{
   // Exposes protected members of BaseWeaponTemplate for direct testing
   internal class ExposedWeaponTemplate : BaseWeaponTemplate
   {
      public static readonly ExposedWeaponTemplate Instance = new ExposedWeaponTemplate();

      public override string Name => "Exposed";
      public override WeaponField ComparisonField => WeaponField.Value;
      public override IEnumerable<ItemObject.ItemTypeEnum> AllowedItemTypes { get; } = new[]
      {
         ItemObject.ItemTypeEnum.OneHandedWeapon,
         ItemObject.ItemTypeEnum.TwoHandedWeapon,
         ItemObject.ItemTypeEnum.Polearm,
         ItemObject.ItemTypeEnum.Bow,
         ItemObject.ItemTypeEnum.Crossbow,
         ItemObject.ItemTypeEnum.Thrown,
      };

      public bool TestIsBastardSword(EquipmentElement e) => IsBastardSword(e);
      public bool TestMeetsDifficulty(EquipmentElement e, Hero h) => MeetsDifficultyRequirement(e, h);
   }

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

   public class MeetsDifficultyRequirementTests
   {
      [Fact]
      public void ZeroDifficulty_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon, difficulty: 0);
         var hero = Helpers.MakeHero();
         Assert.True(ExposedWeaponTemplate.Instance.TestMeetsDifficulty(el, hero));
      }

      [Fact]
      public void SkillExactlyMeetsDifficulty_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 50);
         var hero = Helpers.MakeHero(oneHandedSkill: 50);
         Assert.True(ExposedWeaponTemplate.Instance.TestMeetsDifficulty(el, hero));
      }

      [Fact]
      public void SkillExceedsDifficulty_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 50);
         var hero = Helpers.MakeHero(oneHandedSkill: 200);
         Assert.True(ExposedWeaponTemplate.Instance.TestMeetsDifficulty(el, hero));
      }

      [Fact]
      public void SkillBelowDifficulty_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 50);
         var hero = Helpers.MakeHero(oneHandedSkill: 25);
         Assert.False(ExposedWeaponTemplate.Instance.TestMeetsDifficulty(el, hero));
      }

      [Fact]
      public void ZeroSkill_ItemHasDifficulty_ReturnsFalse()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.OneHandedWeapon,
            WeaponClass.OneHandedSword, difficulty: 30);
         var hero = Helpers.MakeHero();
         Assert.False(ExposedWeaponTemplate.Instance.TestMeetsDifficulty(el, hero));
      }

      [Fact]
      public void TwoHandedDifficulty_MetByTwoHandedSkill_ReturnsTrue()
      {
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, difficulty: 60);
         var hero = Helpers.MakeHero(twoHandedSkill: 100);
         Assert.True(ExposedWeaponTemplate.Instance.TestMeetsDifficulty(el, hero));
      }

      [Fact]
      public void TwoHandedDifficulty_OneHandedSkillAlone_ReturnsFalse()
      {
         // No bastard sword exception in MeetsDifficultyRequirement — uses RelevantSkill only
         var el = Helpers.MakeWeapon(ItemObject.ItemTypeEnum.TwoHandedWeapon,
            WeaponClass.TwoHandedSword, difficulty: 60);
         var hero = Helpers.MakeHero(oneHandedSkill: 200, twoHandedSkill: 0);
         Assert.False(ExposedWeaponTemplate.Instance.TestMeetsDifficulty(el, hero));
      }
   }

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
