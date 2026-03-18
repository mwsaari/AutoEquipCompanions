using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Weapon.Base
{
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
}
