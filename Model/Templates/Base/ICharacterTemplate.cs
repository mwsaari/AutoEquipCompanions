using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Base
{
   public interface ICharacterTemplate
   {
      string Name { get; }
      IEnumerable<(EquipmentIndex Slot, ISlotTemplate Template)> Slots { get; }
   }
}
