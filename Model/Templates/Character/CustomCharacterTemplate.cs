using AutoEquipCompanions.Model.Templates;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates.Character
{
   public class CustomCharacterTemplate : BaseCharacterTemplate
   {
      private readonly string _name;

      public CustomCharacterTemplate(string name, IEnumerable<(EquipmentIndex, ISlotTemplate)> slots)
         : base(slots.ToList())
      {
         _name = name;
      }

      public override string Name => _name;
   }
}
