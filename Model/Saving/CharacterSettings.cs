using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Saving
{
   public class CharacterSettings
   {
      private Preset preset;

      public bool this[EquipmentIndex index]
      {
         get => EquipmentToggles[(int)index];
         set => EquipmentToggles[(int)index] = value;
      }

      public bool CharacterToggle { get; set; }
      public List<bool> EquipmentToggles { get; set; }

      [JsonProperty]
      public int? PresetId { get; private set; }

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

      public CharacterSettings Initialize()
      {
         CharacterToggle = true;
         EquipmentToggles = Enumerable.Repeat(true, (int)EquipmentIndex.NumEquipmentSetSlots).ToList();
         this[EquipmentIndex.Horse] = false;
         this[EquipmentIndex.HorseHarness] = false;
         return this;
      }

      [OnDeserialized]
      private void OnDeserialized(StreamingContext context)
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
