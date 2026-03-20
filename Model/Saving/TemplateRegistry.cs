using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Character;
using AutoEquipCompanions.Model.Templates.Mount;
using AutoEquipCompanions.Model.Templates.Shield;
using AutoEquipCompanions.Model.Templates.Weapon;
using AutoEquipCompanions.Model.Templates.Weapon.Bow;
using AutoEquipCompanions.Model.Templates.Weapon.Crossbow;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using AutoEquipCompanions.Model.Templates.Weapon.Polearm;
using AutoEquipCompanions.Model.Templates.Weapon.Thrown;
using System.Collections.Generic;
using System.Linq;

namespace AutoEquipCompanions.Model.Saving
{
   public static class TemplateRegistry
   {
      public static readonly ICharacterTemplate Null = DisabledCharacterTemplate.Instance;

      public static readonly List<ICharacterTemplate> CharacterTemplates =
         new List<ICharacterTemplate>
         {
            CharacterTemplate.Instance,
            InfantryCaptainTemplate.Instance,
            CavalryCaptainTemplate.Instance,
            HorseArcherTemplate.Instance,
            BowCaptainTemplate.Instance,
            CrossbowCaptainTemplate.Instance,
         };

      private static readonly Dictionary<string, ICharacterTemplate> _characterTemplates =
         CharacterTemplates.ToDictionary(t => t.Name);

      private static readonly Dictionary<string, ISlotTemplate> _slotTemplates =
         new Dictionary<string, ISlotTemplate>
         {
            // One-Handed
            { OneHandedWeaponTemplate.Instance.Name, OneHandedWeaponTemplate.Instance },

            // Polearm
            { PolearmTemplate.Instance.Name, PolearmTemplate.Instance },
            { MountWeaponTemplate.Instance.Name, MountWeaponTemplate.Instance },
            { CouchableLanceTemplate.Instance.Name, CouchableLanceTemplate.Instance },

            // Bow
            { BowTemplate.Instance.Name, BowTemplate.Instance },
            { ArrowsTemplate.Instance.Name, ArrowsTemplate.Instance },

            // Crossbow
            { CrossbowTemplate.Instance.Name, CrossbowTemplate.Instance },
            { BoltsTemplate.Instance.Name, BoltsTemplate.Instance },

            // Thrown
            { ThrownTemplate.Instance.Name, ThrownTemplate.Instance },

            // General
            { RangedTemplate.Instance.Name, RangedTemplate.Instance },
            { AmmoTemplate.Instance.Name, AmmoTemplate.Instance },
            { DefaultWeaponTemplate.Instance.Name, DefaultWeaponTemplate.Instance },
            { SameTypeWeaponTemplate.Instance.Name, SameTypeWeaponTemplate.Instance },

            // Armor
            { LightArmorTemplate.Instance.Name, LightArmorTemplate.Instance },
            { MediumArmorTemplate.Instance.Name, MediumArmorTemplate.Instance },
            { DefaultArmorTemplate.Instance.Name, DefaultArmorTemplate.Instance },

            // Mount
            { LightMountTemplate.Instance.Name, LightMountTemplate.Instance },
            { DefaultMountTemplate.Instance.Name, DefaultMountTemplate.Instance },
            { CamelMountTemplate.Instance.Name, CamelMountTemplate.Instance },

            // Shield
            { DefaultShieldTemplate.Instance.Name, DefaultShieldTemplate.Instance },
         };

      public static void Register(ICharacterTemplate template)
         => _characterTemplates[template.Name] = template;

      public static ICharacterTemplate Resolve(string name)
         => name != null && _characterTemplates.TryGetValue(name, out var t) ? t : null;

      public static ISlotTemplate ResolveSlotTemplate(string name)
         => name != null && _slotTemplates.TryGetValue(name, out var t) ? t : null;
   }
}
