using AutoEquipCompanions.Model.Saving;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Saving
{
   public class GameSettingsTests
   {
      [Fact]
      public void BastardSwordsAreOneHanded_DefaultTrue()
      {
         Assert.True(new GameSettings().BastardSwordsAreOneHanded);
      }

      [Fact]
      public void CanAutoEquipLocked_DefaultFalse()
      {
         Assert.False(new GameSettings().CanAutoEquipLocked);
      }

      [Fact]
      public void DebugEnabled_DefaultFalse()
      {
         Assert.False(new GameSettings().DebugEnabled);
      }

      [Fact]
      public void BastardSwordsAreOneHanded_CanBeSetFalse()
      {
         var settings = new GameSettings();
         settings.BastardSwordsAreOneHanded = false;
         Assert.False(settings.BastardSwordsAreOneHanded);
      }
   }
}
