using AutoEquipCompanions.Model;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace AutoEquipCompanions.ViewModel
{
    public class PresetSelectorItemVM : SelectorItemVM
    {
        public Preset Preset { get; set; }

        public PresetSelectorItemVM(Preset preset) : base(preset.Name)
        {
            Preset = preset;
        }
    }
}
