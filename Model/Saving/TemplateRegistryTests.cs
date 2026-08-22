using AutoEquipCompanions.Model.Saving;
using AutoEquipCompanions.Model.Templates;
using AutoEquipCompanions.Model.Templates.Character;
using AutoEquipCompanions.Model.Templates.Weapon.OneHanded;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using Xunit;

namespace AutoEquipCompanions.Test.Model.Saving
{
   public class TemplateRegistryTests
   {
      // ── Resolve (character templates) ───────────────────────────────────────

      [Fact]
      public void Resolve_KnownName_ReturnsMatchingInstance()
      {
         Assert.Same(InfantryCaptainTemplate.Instance, TemplateRegistry.Resolve("infantry_captain"));
      }

      [Fact]
      public void Resolve_UnknownName_ReturnsNull()
      {
         Assert.Null(TemplateRegistry.Resolve("not_a_real_template"));
      }

      [Fact]
      public void Resolve_NullName_ReturnsNull()
      {
         Assert.Null(TemplateRegistry.Resolve(null));
      }

      [Fact]
      public void Register_NewTemplate_IsResolvableByName()
      {
         var dummy = new DummyCharacterTemplate();
         TemplateRegistry.Register(dummy);
         Assert.Same(dummy, TemplateRegistry.Resolve(dummy.Name));
      }

      // ── ResolveSlotTemplate ──────────────────────────────────────────────────

      [Fact]
      public void ResolveSlotTemplate_KnownName_ReturnsMatchingInstance()
      {
         Assert.Same(OneHandedWeaponTemplate.Instance, TemplateRegistry.ResolveSlotTemplate("one_handed_weapon"));
      }

      [Fact]
      public void ResolveSlotTemplate_UnknownName_ReturnsNull()
      {
         Assert.Null(TemplateRegistry.ResolveSlotTemplate("not_a_real_slot_template"));
      }

      [Fact]
      public void ResolveSlotTemplate_NullName_ReturnsNull()
      {
         Assert.Null(TemplateRegistry.ResolveSlotTemplate(null));
      }

      private class DummyCharacterTemplate : ICharacterTemplate
      {
         public string Name => "test_dummy_template";
         public string DisplayName => "Dummy";
         public bool DefaultEnabled => true;
         public IEnumerable<(EquipmentIndex Slot, ISlotTemplate Template)> Slots =>
            Array.Empty<(EquipmentIndex, ISlotTemplate)>();
      }
   }
}
