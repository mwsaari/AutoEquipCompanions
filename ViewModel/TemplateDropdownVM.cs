using System;
using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates;
using TaleWorlds.Library;

namespace AutoEquipCompanions.ViewModel
{
   public class TemplateDropdownVM : TaleWorlds.Library.ViewModel
   {
      private readonly Func<ICharacterTemplate> _getTemplate;
      private readonly Action<ICharacterTemplate> _setTemplate;
      private readonly MBBindingList<TemplateItemVM> _templateItems = new MBBindingList<TemplateItemVM>();
      private bool _isTemplateListOpen;

      public TemplateDropdownVM(Func<ICharacterTemplate> getTemplate, Action<ICharacterTemplate> setTemplate)
      {
         _getTemplate = getTemplate;
         _setTemplate = setTemplate;
         RebuildItems();
      }

      [DataSourceProperty]
      public string CurrentTemplateName => _getTemplate()?.DisplayName ?? "";

      [DataSourceProperty]
      public bool IsTemplateListOpen
      {
         get => _isTemplateListOpen;
         private set
         {
            _isTemplateListOpen = value;
            OnPropertyChanged(nameof(IsTemplateListOpen));
         }
      }

      [DataSourceProperty]
      public MBBindingList<TemplateItemVM> TemplateItems => _templateItems;

      public void ToggleTemplateList()
      {
         IsTemplateListOpen = !IsTemplateListOpen;
      }

      public void Refresh()
      {
         IsTemplateListOpen = false;
         RebuildItems();
         OnPropertyChanged(nameof(CurrentTemplateName));
      }

      private void RebuildItems()
      {
         _templateItems.Clear();
         var currentTemplate = _getTemplate();
         var templates = TemplateRegistry.CharacterTemplates;
         for (var i = 0; i < templates.Count; i++)
         {
            var index = i;
            _templateItems.Add(new TemplateItemVM(
               templates[i].DisplayName,
               templates[i] == currentTemplate,
               () => SelectAt(index)
            ));
         }
         OnPropertyChanged(nameof(TemplateItems));
      }

      private void SelectAt(int index)
      {
         var templates = TemplateRegistry.CharacterTemplates;
         var isInRange = index >= 0 && index < templates.Count;
         if (!isInRange)
            return;
         _setTemplate(templates[index]);
         IsTemplateListOpen = false;
         OnPropertyChanged(nameof(CurrentTemplateName));
         RebuildItems();
      }
   }
}
