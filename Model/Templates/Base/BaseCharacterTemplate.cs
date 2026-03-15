using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Base
{
   public abstract class BaseCharacterTemplate : ICharacterTemplate
   {
      private readonly List<(EquipmentIndex Slot, ISlotTemplate Template)> _slots;

      protected BaseCharacterTemplate(List<(EquipmentIndex Slot, ISlotTemplate Template)> slots)
      {
         _slots = slots;
      }

      public abstract string Name { get; }
      public IEnumerable<(EquipmentIndex Slot, ISlotTemplate Template)> Slots => _slots;
   }
}
