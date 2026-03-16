using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates
{
   public interface ICharacterTemplate
   {
      string Name { get; }
      IEnumerable<(EquipmentIndex Slot, ISlotTemplate Template)> Slots { get; }
   }
}
