using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Saving
{
    public class CharacterSettings
    {
        private Preset preset = null;

        public CharacterSettings Initialize()
        {
            CharacterToggle = true;
            EquipmentToggles = Enumerable.Repeat(true, (int)EquipmentIndex.NumEquipmentSetSlots).ToList();
            this[EquipmentIndex.Horse] = false;
            this[EquipmentIndex.HorseHarness] = false;
            return this;
        }

        public bool this[EquipmentIndex index]
        {
            get { return EquipmentToggles[(int)index]; }
            set { EquipmentToggles[(int)index] = value; }
        }

        public bool CharacterToggle { get; set; }
        public List<bool> EquipmentToggles { get; set; }

        [JsonProperty]
        public int? PresetId { get; private set; } = null;

        [JsonIgnore]
        public Preset Preset
        {
            get => preset;
            set
            {
                PresetId = value?.Id;
                preset = value;
            }
        }

        [OnDeserialized]
        private void OnDeserialized()
        {
            if (PresetId is not null)
            {
                Preset = Config.Presets.FirstOrDefault(x => x.Id == PresetId);
                if (Preset is null)
                {
                    // TODO add message about failing to find preset for companion
                    PresetId = null;
                }
            }
        }
    }
}
