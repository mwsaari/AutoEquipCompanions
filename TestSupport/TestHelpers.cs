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
         int difficulty = 0,
         int thrustDamage = 80,
         int swingDamage = 100,
         int missileSpeed = 0,
         int handling = 100,
         int value = 0)
      {
         var item = new ItemObject();
         item.Type = type;

         var wcd = new WeaponComponentData(item, weaponClass);
         wcd.Init(
            descriptionId, "", itemUsage,
            DamageTypes.Blunt, DamageTypes.Blunt,
            0, 100, 0f, 1f, 0.5f, handling,
            1f, 1f, 1,
            "", 100, missileSpeed,
            default(MatrixFrame), WeaponClass.Undefined,
            0f, 80, swingDamage, 60, thrustDamage,
            default(Vec3), WeaponComponentData.WeaponTiers.Tier1, 1);

         var wc = new WeaponComponent(item);
         wc.AddWeapon(wcd, null);
         SetProperty(item, "ItemComponent", wc);

         if (difficulty > 0)
            SetProperty(item, "Difficulty", difficulty);

         if (value > 0)
            SetProperty(item, "Value", value);

         return new EquipmentElement(item);
      }

      public static EquipmentElement MakeArmor(
         ItemObject.ItemTypeEnum type,
         int headArmor = 0,
         int bodyArmor = 0,
         int armArmor = 0,
         int legArmor = 0,
         ItemObject.ItemTiers? tier = null)
      {
         var item = new ItemObject();
         item.Type = type;

         var armor = new ArmorComponent(item);
         SetProperty(armor, "HeadArmor", headArmor);
         SetProperty(armor, "BodyArmor", bodyArmor);
         SetProperty(armor, "ArmArmor", armArmor);
         SetProperty(armor, "LegArmor", legArmor);
         SetProperty(item, "ItemComponent", armor);

         if (tier.HasValue)
            SetTier(item, tier.Value);

         return new EquipmentElement(item);
      }

      public static EquipmentElement MakeHorseHarness(int familyType = 0, ItemObject.ItemTiers? tier = null)
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.HorseHarness;

         var armor = new ArmorComponent(item);
         SetProperty(armor, "FamilyType", familyType);
         SetProperty(item, "ItemComponent", armor);

         if (tier.HasValue)
            SetTier(item, tier.Value);

         return new EquipmentElement(item);
      }

      public static EquipmentElement MakeHorse(
         int speed = 0,
         int maneuver = 0,
         int chargeDamage = 0,
         bool isCamel = false,
         int familyType = 0)
      {
         var item = new ItemObject();
         item.Type = ItemObject.ItemTypeEnum.Horse;

         var monster = new Monster();
         SetProperty(monster, "MonsterUsage", isCamel ? "camel" : "horse");
         SetProperty(monster, "FamilyType", familyType);

         var horse = new HorseComponent();
         SetProperty(horse, "Speed", speed);
         SetProperty(horse, "Maneuver", maneuver);
         SetProperty(horse, "ChargeDamage", chargeDamage);
         SetProperty(horse, "Monster", monster);

         SetProperty(item, "ItemComponent", horse);
         return new EquipmentElement(item);
      }

      // ItemObject.Tier is derived from a private Tierf/TierfOverride pair:
      //   Tierf = (TierfOverride >= 1) ? TierfOverride - 1 : <native item-value model, unavailable here>
      //   Tier  = Clamp(Round(Tierf), 0, 6) - 1
      // Setting TierfOverride to (tier + 2) lands Tier on the requested value without touching
      // Game.Current.BasicModels, which isn't set up in this harness.
      private static void SetTier(ItemObject item, ItemObject.ItemTiers tier)
      {
         SetProperty(item, "TierfOverride", (float)((int)tier + 2));
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
