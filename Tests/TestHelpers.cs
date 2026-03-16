using System;
using System.Reflection;
using System.Runtime.Serialization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace AutoEquipCompanions.Test
{
   internal static class Helpers
   {
      static Helpers()
      {
         InitGameContext();
      }

      // Bootstrap a minimal Game.Current so DefaultSkills.OneHanded etc. don't throw.
      private static void InitGameContext()
      {
         // Create a Game shell without running its constructor (which needs GameType, etc.)
         var fakeGame = (Game)FormatterServices.GetUninitializedObject(typeof(Game));
         typeof(Game)
            .GetField("_current", BindingFlags.Static | BindingFlags.NonPublic)
            .SetValue(null, fakeGame);

         // Create a DefaultSkills shell and wire up only the skills we need
         var ds = typeof(DefaultSkills);
         var fakeSkills = (DefaultSkills)FormatterServices.GetUninitializedObject(ds);
         foreach (var (field, id) in new[]
         {
            ("_skillOneHanded", "OneHanded"),
            ("_skillTwoHanded", "TwoHanded"),
            ("_skillPolearm",   "Polearm"),
            ("_skillBow",       "Bow"),
            ("_skillCrossbow",  "Crossbow"),
            ("_skillThrowing",  "Throwing"),
         })
         {
            ds.GetField(field, BindingFlags.NonPublic | BindingFlags.Instance)
              .SetValue(fakeSkills, new SkillObject(id));
         }

         // Attach to the fake game
         typeof(Game)
            .GetProperty("DefaultSkills", BindingFlags.Public | BindingFlags.Instance)
            .SetValue(fakeGame, fakeSkills);

         // Hero..ctor calls MBRandom which uses Game.Current.RandomGenerator
         typeof(Game)
            .GetProperty("RandomGenerator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(fakeGame, new MBFastRandom());
      }

      public static EquipmentElement MakeWeapon(
         ItemObject.ItemTypeEnum type,
         WeaponClass weaponClass = WeaponClass.Undefined,
         string itemUsage = "",
         string descriptionId = "",
         int difficulty = 0)
      {
         var item = new ItemObject();
         item.Type = type;

         var wcd = new WeaponComponentData(item, weaponClass);
         wcd.Init(
            descriptionId, "", itemUsage,
            DamageTypes.Blunt, DamageTypes.Blunt,
            0, 100, 0f, 1f, 0.5f, 100,
            1f, 1f, 1,
            "", 100, 0,
            default(MatrixFrame), WeaponClass.Undefined,
            0f, 80, 100, 60, 80,
            default(Vec3), WeaponComponentData.WeaponTiers.Tier1, 1);

         var wc = new WeaponComponent(item);
         wc.AddWeapon(wcd, null);
         SetProperty(item, "ItemComponent", wc);

         if (difficulty > 0)
            SetProperty(item, "Difficulty", difficulty);

         return new EquipmentElement(item);
      }

      public static EquipmentElement MakeArmor(ItemObject.ItemTypeEnum type)
      {
         var item = new ItemObject();
         item.Type = type;
         SetProperty(item, "ItemComponent", new ArmorComponent(item));
         return new EquipmentElement(item);
      }

      public static Hero MakeHero(
         int oneHandedSkill = 0,
         int twoHandedSkill = 0,
         int polearmSkill = 0,
         EquipmentElement slot0 = default,
         EquipmentElement slot1 = default,
         EquipmentElement slot2 = default,
         EquipmentElement slot3 = default,
         EquipmentElement horse = default)
      {
         var hero = new Hero();
         var eq = new Equipment();
         eq[EquipmentIndex.Weapon0] = slot0;
         eq[EquipmentIndex.Weapon1] = slot1;
         eq[EquipmentIndex.Weapon2] = slot2;
         eq[EquipmentIndex.Weapon3] = slot3;
         eq[EquipmentIndex.Horse] = horse;
         SetProperty(hero, "_battleEquipment", eq);

         if (oneHandedSkill > 0) hero.SetSkillValue(DefaultSkills.OneHanded, oneHandedSkill);
         if (twoHandedSkill > 0) hero.SetSkillValue(DefaultSkills.TwoHanded, twoHandedSkill);
         if (polearmSkill > 0) hero.SetSkillValue(DefaultSkills.Polearm, polearmSkill);

         return hero;
      }

      private static void SetProperty(object obj, string name, object value)
      {
         var type = obj.GetType();

         var prop = type.GetProperty(name,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
         if (prop?.CanWrite == true)
         {
            prop.SetValue(obj, value);
            return;
         }

         foreach (var fieldName in new[] { name, $"<{name}>k__BackingField", $"_{name}" })
         {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
               field.SetValue(obj, value);
               return;
            }
         }

         throw new InvalidOperationException($"Cannot set '{name}' on {type.Name}");
      }
   }
}
