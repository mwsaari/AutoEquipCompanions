using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model
{
    public class Preset
    {
        public string Name { get; set; }

        public SlotPreset this[EquipmentIndex index]
        {
            get { return Slots[(int)index]; }
            set { Slots[(int)index] = value; }
        }
        public List<SlotPreset> Slots { get; set; }
    }
}
