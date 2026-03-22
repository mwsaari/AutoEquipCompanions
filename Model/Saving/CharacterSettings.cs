using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Character;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model.Saving
{
   public class CharacterSettings
   {
      private readonly Dictionary<EquipmentIndex, bool> _slotToggles = new Dictionary<EquipmentIndex, bool>();

      public bool CharacterToggle { get; set; }
      public ICharacterTemplate Template { get; set; }

      public bool this[EquipmentIndex index]
      {
         get => !_slotToggles.TryGetValue(index, out var v) || v;
         set => _slotToggles[index] = value;
      }

      public CharacterSettings Initialize(bool allSlotsEnabled = false)
      {
         Template = CharacterTemplate.Instance;
         CharacterToggle = Template.DefaultEnabled;
         for (var i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
            _slotToggles[(EquipmentIndex)i] = true;
         if (!allSlotsEnabled)
         {
            this[EquipmentIndex.Horse] = false;
            this[EquipmentIndex.HorseHarness] = false;
         }
         return this;
      }

      public JObject ToJson()
      {
         var json = new JObject { ["CharacterToggle"] = CharacterToggle };
         WriteTemplate(json);
         WriteToggles(json);
         return json;
      }

      private void WriteTemplate(JObject json)
      {
         json["Template"] = Template.Name;
         if (Template is CustomCharacterTemplate)
            json["SlotTemplates"] = WriteSlotTemplates();
      }

      private JObject WriteSlotTemplates()
      {
         var json = new JObject();
         foreach (var (slot, slotTemplate) in Template.Slots)
            json[slot.ToString()] = slotTemplate.Name;
         return json;
      }

      private void WriteToggles(JObject json)
      {
         foreach (var kv in _slotToggles)
            json[kv.Key.ToString()] = kv.Value;
      }

      public static CharacterSettings FromJson(JObject obj)
      {
         var settings = new CharacterSettings();
         settings.Template = ReadTemplate(obj);
         settings.CharacterToggle = obj["CharacterToggle"]?.Value<bool>() ?? settings.Template.DefaultEnabled;
         settings.ApplyToggles(obj);
         return settings;
      }

      private static ICharacterTemplate ReadTemplate(JObject obj)
      {
         var name = obj["Template"]?.Value<string>();
         var slotTemplatesObj = (JObject)obj["SlotTemplates"];
         if (slotTemplatesObj == null)
            return TemplateRegistry.Resolve(name) ?? TemplateRegistry.Null;
         return new CustomCharacterTemplate(name, ReadSlotTemplates(slotTemplatesObj));
      }

      private static List<(EquipmentIndex, ISlotTemplate)> ReadSlotTemplates(JObject obj)
      {
         var slots = new List<(EquipmentIndex, ISlotTemplate)>();
         foreach (var prop in obj.Properties())
         {
            if (!Enum.TryParse<EquipmentIndex>(prop.Name, out var index))
               continue;
            var slotTemplate = TemplateRegistry.ResolveSlotTemplate(prop.Value.Value<string>());
            if (slotTemplate != null)
               slots.Add((index, slotTemplate));
         }
         return slots;
      }

      private void ApplyToggles(JObject obj)
      {
         for (var i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
         {
            var slot = (EquipmentIndex)i;
            var token = obj[slot.ToString()];
            if (token != null)
               this[slot] = token.Value<bool>();
         }
      }
   }
}
