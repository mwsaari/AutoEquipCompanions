using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templating
{
   public class CharacterTemplate
   {
      public string Name { get; set; }

      public List<SlotTemplate> Slots { get; set; }

      public SlotTemplate this[EquipmentIndex index]
      {
         get => Slots[(int)index];
         set => Slots[(int)index] = value;
      }
   }
}
