using AutoEquipCompanions.Model.Templates.Character;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Templates
{
   public static class TemplateRegistry
   {
      public static readonly ICharacterTemplate Null = new NullCharacterTemplate();

      private static readonly Dictionary<string, ICharacterTemplate> _templates =
         new Dictionary<string, ICharacterTemplate>
         {
            { CharacterTemplate.Instance.Name, CharacterTemplate.Instance },
         };

      public static void Register(ICharacterTemplate template)
         => _templates[template.Name] = template;

      public static ICharacterTemplate Resolve(string name)
         => name != null && _templates.TryGetValue(name, out var template) ? template : null;

      private class NullCharacterTemplate : BaseCharacterTemplate
      {
         public NullCharacterTemplate()
            : base(new List<(EquipmentIndex, ISlotTemplate)>()) { }

         public override string Name => "Unknown";
      }
   }
}
