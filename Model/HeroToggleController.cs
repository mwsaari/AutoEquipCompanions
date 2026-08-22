using System;
using System.Collections.Generic;
using AutoEquipCompanions.Model.Saving;
using TaleWorlds.Core;

namespace AutoEquipCompanions.Model
{
   public class HeroToggleController
   {
      private readonly Func<string> _getCurrentHero;
      private readonly Dictionary<string, CharacterSettings> _heroToggles;
      private readonly CharacterSettings _defaultSettings;

      public HeroToggleController(
         Func<string> getCurrentHero,
         Dictionary<string, CharacterSettings> heroToggles,
         CharacterSettings defaultSettings)
      {
         _getCurrentHero = getCurrentHero;
         _heroToggles = heroToggles;
         _defaultSettings = defaultSettings;
      }

      private string CurrentHero => _getCurrentHero();
      private bool HasCurrentHero => CurrentHero != null;

      public bool CharacterToggle
      {
         get
         {
            if (!HasCurrentHero)
               return true;
            return !_heroToggles.TryGetValue(CurrentHero, out var value) || value.CharacterToggle;
         }
         set
         {
            if (!HasCurrentHero)
               return;
            if (_heroToggles.ContainsKey(CurrentHero))
            {
               _heroToggles[CurrentHero].CharacterToggle = value;
            }
            else
            {
               var characterSettings = new CharacterSettings().Initialize();
               characterSettings.CharacterToggle = value;
               _heroToggles.Add(CurrentHero, characterSettings);
            }
         }
      }

      public bool GetSlotToggle(EquipmentIndex index)
      {
         return HasCurrentHero && _heroToggles.TryGetValue(CurrentHero, out var s)
            ? s[index] : _defaultSettings[index];
      }

      public void ToggleEquipment(EquipmentIndex index)
      {
         if (!HasCurrentHero)
            return;
         if (_heroToggles.TryGetValue(CurrentHero, out var characterSettings))
         {
            characterSettings[index] = !characterSettings[index];
         }
         else
         {
            characterSettings = new CharacterSettings().Initialize();
            characterSettings[index] = !characterSettings[index];
            _heroToggles.Add(CurrentHero, characterSettings);
         }
      }
   }
}
