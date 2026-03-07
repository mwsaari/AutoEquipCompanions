using AutoEquipCompanions.Model;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace AutoEquipCompanions.ViewModel
{
   public class PresetSelectorItemVM : SelectorItemVM
   {

      public PresetSelectorItemVM(Preset preset) : base(preset.Name)
      {
         Preset = preset;
      }

      public Preset Preset { get; set; }
   }
}
