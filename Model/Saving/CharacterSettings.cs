using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Saving
{
   public class CharacterSettings
   {
      public bool CharacterToggle { get; set; }
      public List<bool> EquipmentToggles { get; set; }

      public bool this[EquipmentIndex index]
      {
         get => EquipmentToggles[(int)index];
         set => EquipmentToggles[(int)index] = value;
      }

      public CharacterSettings Initialize()
      {
         CharacterToggle = true;
         EquipmentToggles = Enumerable.Repeat(true, (int)EquipmentIndex.NumEquipmentSetSlots).ToList();
         this[EquipmentIndex.Horse] = false;
         this[EquipmentIndex.HorseHarness] = false;
         return this;
      }
   }
}
