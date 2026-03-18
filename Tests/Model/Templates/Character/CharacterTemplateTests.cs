using AutoEquipCompanions.Model.Templates.Character;
using System.Linq;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Templates.Character
{
   public class CharacterTemplateTests
   {
      [Fact]
      public void AllSlotIndices_AreUnique()
      {
         var slots = CharacterTemplate.Instance.Slots.Select(x => x.Slot).ToList();
         Assert.Equal(slots.Count, slots.Distinct().Count());
      }
   }
}
