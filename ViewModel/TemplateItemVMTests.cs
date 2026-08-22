using AutoEquipCompanions.ViewModel;
using Xunit;

namespace AutoEquipCompanions.Test.ViewModel
{
   public class TemplateItemVMTests
   {
      [Fact]
      public void Constructor_SetsNameAndIsSelected()
      {
         var vm = new TemplateItemVM("Infantry Captain", true, () => { });
         Assert.Equal("Infantry Captain", vm.Name);
         Assert.True(vm.IsSelected);
      }

      [Fact]
      public void ExecuteSelect_InvokesCallback()
      {
         var invoked = false;
         var vm = new TemplateItemVM("X", false, () => invoked = true);
         vm.ExecuteSelect();
         Assert.True(invoked);
      }
   }
}
