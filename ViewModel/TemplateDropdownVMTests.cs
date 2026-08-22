using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Character;
using AutoEquipCompanions.ViewModel;
using Xunit;

namespace AutoEquipCompanions.Test.ViewModel
{
   public class TemplateDropdownVMTests
   {
      [Fact]
      public void Constructor_BuildsOneItemPerRegisteredCharacterTemplate()
      {
         var vm = new TemplateDropdownVM(() => CharacterTemplate.Instance, _ => { });
         Assert.Equal(TemplateRegistry.CharacterTemplates.Count, vm.TemplateItems.Count);
      }

      [Fact]
      public void Constructor_MarksOnlyCurrentTemplateAsSelected()
      {
         var vm = new TemplateDropdownVM(() => InfantryCaptainTemplate.Instance, _ => { });
         var index = TemplateRegistry.CharacterTemplates.IndexOf(InfantryCaptainTemplate.Instance);

         for (var i = 0; i < vm.TemplateItems.Count; i++)
            Assert.Equal(i == index, vm.TemplateItems[i].IsSelected);
      }

      [Fact]
      public void CurrentTemplateName_ReflectsGetTemplateDisplayName()
      {
         var vm = new TemplateDropdownVM(() => InfantryCaptainTemplate.Instance, _ => { });
         Assert.Equal(InfantryCaptainTemplate.Instance.DisplayName, vm.CurrentTemplateName);
      }

      [Fact]
      public void ToggleTemplateList_FlipsIsTemplateListOpen()
      {
         var vm = new TemplateDropdownVM(() => CharacterTemplate.Instance, _ => { });
         Assert.False(vm.IsTemplateListOpen);
         vm.ToggleTemplateList();
         Assert.True(vm.IsTemplateListOpen);
         vm.ToggleTemplateList();
         Assert.False(vm.IsTemplateListOpen);
      }

      [Fact]
      public void SelectingItem_CallsSetTemplateAndClosesList()
      {
         ICharacterTemplate selected = null;
         var vm = new TemplateDropdownVM(() => CharacterTemplate.Instance, t => selected = t);
         vm.ToggleTemplateList();

         var index = TemplateRegistry.CharacterTemplates.IndexOf(InfantryCaptainTemplate.Instance);
         vm.TemplateItems[index].ExecuteSelect();

         Assert.Same(InfantryCaptainTemplate.Instance, selected);
         Assert.False(vm.IsTemplateListOpen);
      }

      [Fact]
      public void Refresh_ClosesListAndRebuildsSelection()
      {
         ICharacterTemplate current = CharacterTemplate.Instance;
         var vm = new TemplateDropdownVM(() => current, _ => { });
         vm.ToggleTemplateList();

         current = InfantryCaptainTemplate.Instance;
         vm.Refresh();

         Assert.False(vm.IsTemplateListOpen);
         var index = TemplateRegistry.CharacterTemplates.IndexOf(InfantryCaptainTemplate.Instance);
         Assert.True(vm.TemplateItems[index].IsSelected);
      }
   }
}
