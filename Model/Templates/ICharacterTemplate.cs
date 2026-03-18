using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates
{
   public interface ICharacterTemplate
   {
      string Name { get; }
      string DisplayName { get; }
      bool DefaultEnabled { get; }
      IEnumerable<(EquipmentIndex Slot, ISlotTemplate Template)> Slots { get; }
   }
}
