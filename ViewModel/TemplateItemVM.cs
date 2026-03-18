using System;
using TaleWorlds.Library;

namespace AutoEquipCompanions.ViewModel
{
   public class TemplateItemVM : TaleWorlds.Library.ViewModel
   {
      private readonly Action _onSelect;

      public TemplateItemVM(string name, bool isSelected, Action onSelect)
      {
         Name = name;
         IsSelected = isSelected;
         _onSelect = onSelect;
      }

      [DataSourceProperty]
      public string Name { get; set; }

      [DataSourceProperty]
      public bool IsSelected { get; set; }

      public void ExecuteSelect() => _onSelect();
   }
}
