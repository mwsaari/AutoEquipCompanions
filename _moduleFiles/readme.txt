# Auto Equip Companions

:: REQUIREMENTS ::
Mount & Blade II: Bannerlord v1.3.15+

:: DESCRIPTION ::
Automatically equips companions with the best available gear when closing the inventory screen.
Per-character settings allow you to control which equipment slots are managed for each hero.

:: GAME SETTINGS (game_settings.json) ::
  CanAutoEquipLocked        Allow auto equip to replace locked items (default: true)
  DebugEnabled              Write item debug info to log on inventory open (default: false)
  BastardSwordsAreOneHanded Treat bastard swords as valid for one-handed slots (default: true)
  UIVersion                 0 = no UI (auto-equips all heroes with default template on inventory close)
                            1 = toggle overlay with per-slot enable/disable buttons (default)
                            2 = template overlay with per-character template selection and per-slot toggles

:: FEATURES ::
- Auto-equips companions on inventory close
- Manual trigger button in the inventory screen (UI versions 1 and 2)
- Per-character template selection (Default, Infantry Captain, Cavalry Captain, etc.)
- Per-character toggle to enable/disable auto equip
- Per-slot toggles for armor, weapons, horse, and harness
- Settings are saved between sessions
- Configurable behavior via game_settings.json
- No-UI mode (UIVersion 0) for hands-off auto-equipping with default templates

:: USAGE ::
Open the inventory screen. A button will appear in the top-right area.
- Left click: manually trigger auto equip
- Right click: toggle the settings panel

In the settings panel you can select a template for the current character, enable/disable auto equip
per character, and toggle individual equipment slots.

With UIVersion 0, no UI is shown. All heroes are automatically equipped using the default template
when the inventory screen is closed.

:: TEMPLATES ::
Templates control what type of gear each slot looks for when auto-equipping.

  Default
    Armor:   Best available of any armor type for each slot
    Horse:   Best available mount and harness
    Weapons: Matches the same weapon type already in each slot

  Infantry Captain
    Armor:   Best available armor in all slots
    Horse:   Empty (dismounted)
    Weapon0: One-handed weapon (includes bastard swords if enabled)
    Weapon1: Shield
    Weapon2: Polearm
    Weapon3: Thrown weapon

  Cavalry Captain
    Armor:   Best available armor in all slots
    Horse:   Best available mount and harness
    Weapon0: Cavalry/lance weapon
    Weapon1: One-handed weapon
    Weapon2: Shield
    Weapon3: Thrown weapon

  Horse Archer
    Armor:   Light armor in all slots
    Horse:   Fast mount and light harness
    Weapon0: Bow
    Weapon1: Arrows
    Weapon2: One-handed weapon
    Weapon3: Arrows (extra quiver)

  Bow Captain
    Armor:   Medium armor in all slots
    Horse:   Empty (dismounted)
    Weapon0: Bow
    Weapon1: Arrows
    Weapon2: Shield
    Weapon3: One-handed weapon

  Crossbow Captain
    Armor:   Medium armor in all slots
    Horse:   Empty (dismounted)
    Weapon0: Crossbow
    Weapon1: Bolts
    Weapon2: Shield
    Weapon3: One-handed weapon

:: KNOWN ISSUES ::
None at the moment. Templates is a full rewrite though so be safe with save data.
