using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Armor;
using AutoEquipCompanions.Model.Templates.Character;
using AutoEquipCompanions.Model.Templates.Mount;
using AutoEquipCompanions.Model.Templates.Shield;
using AutoEquipCompanions.Model.Templates.Weapon;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Saving
{
   public class CharacterSettings
   {
      private static readonly IReadOnlyDictionary<string, ISlotTemplate> SlotTemplates =
         new Dictionary<string, ISlotTemplate>
         {
            { DefaultArmorTemplate.Instance.Name, DefaultArmorTemplate.Instance },
            { DefaultMountTemplate.Instance.Name, DefaultMountTemplate.Instance },
            { CamelMountTemplate.Instance.Name, CamelMountTemplate.Instance },
            { DefaultWeaponTemplate.Instance.Name, DefaultWeaponTemplate.Instance },
            { SameTypeWeaponTemplate.Instance.Name, SameTypeWeaponTemplate.Instance },
            { MissileTemplate.Instance.Name, MissileTemplate.Instance },
            { MountWeaponTemplate.Instance.Name, MountWeaponTemplate.Instance },
            { CouchableLanceTemplate.Instance.Name, CouchableLanceTemplate.Instance },
            { DefaultShieldTemplate.Instance.Name, DefaultShieldTemplate.Instance },
         };

      private readonly Dictionary<EquipmentIndex, bool> _slotToggles = new Dictionary<EquipmentIndex, bool>();

      public bool CharacterToggle { get; set; }
      public ICharacterTemplate Template { get; set; }

      public bool this[EquipmentIndex index]
      {
         get => _slotToggles.TryGetValue(index, out var v) ? v : true;
         set => _slotToggles[index] = value;
      }

      public CharacterSettings Initialize()
      {
         CharacterToggle = true;
         Template = CharacterTemplate.Instance;
         for (var i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
            _slotToggles[(EquipmentIndex)i] = true;
         this[EquipmentIndex.Horse] = false;
         this[EquipmentIndex.HorseHarness] = false;
         return this;
      }

      public JObject ToJson()
      {
         var templateJson = new JObject { ["Name"] = Template.Name };

         if (Template is CustomCharacterTemplate)
         {
            var slotTemplatesJson = new JObject();
            foreach (var (slot, slotTemplate) in Template.Slots)
               slotTemplatesJson[slot.ToString()] = slotTemplate.Name;
            templateJson["SlotTemplates"] = slotTemplatesJson;
         }

         foreach (var kv in _slotToggles)
            templateJson[kv.Key.ToString()] = kv.Value;

         return new JObject
         {
            ["CharacterToggle"] = CharacterToggle,
            ["Template"] = templateJson
         };
      }

      public static CharacterSettings FromJson(JObject obj)
      {
         var settings = new CharacterSettings();
         settings.CharacterToggle = obj["CharacterToggle"]?.Value<bool>() ?? true;

         var templateObj = (JObject)obj["Template"];
         if (templateObj == null)
            return settings.Initialize();

         settings.Template = ResolveTemplate(templateObj);

         for (var i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
         {
            var slot = (EquipmentIndex)i;
            var token = templateObj[slot.ToString()];
            if (token != null)
               settings[slot] = token.Value<bool>();
         }

         return settings;
      }

      private static ICharacterTemplate ResolveTemplate(JObject templateObj)
      {
         var name = templateObj["Name"]?.Value<string>();
         var slotTemplatesObj = (JObject)templateObj["SlotTemplates"];

         if (slotTemplatesObj == null)
            return TemplateRegistry.Resolve(name) ?? TemplateRegistry.Null;

         var slots = new List<(EquipmentIndex, ISlotTemplate)>();
         foreach (var prop in slotTemplatesObj.Properties())
         {
            if (!Enum.TryParse<EquipmentIndex>(prop.Name, out var index))
               continue;
            if (!SlotTemplates.TryGetValue(prop.Value.Value<string>(), out var slotTemplate))
               continue;
            slots.Add((index, slotTemplate));
         }

         return new CustomCharacterTemplate(name, slots);
      }
   }
}
