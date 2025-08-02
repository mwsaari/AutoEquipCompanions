using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System;

namespace AutoEquipCompanions.Model.Saving
{
    internal class AutoEquipGlobalSettings : AttributeGlobalSettings<AutoEquipGlobalSettings>
    {
        public override string Id => "AutoEquipMCMUI";
        public override string DisplayName => "Auto Equip Companions";

        [SettingPropertyBool("Ignore Locked Items", Order = 0, RequireRestart = false, HintText = "If you turn this on the auto equip will not equip locked items.\nTurning this on will disable the ability to auto-equip by pressing the button in the inventory screen.")]
        [SettingPropertyGroup("General")]
        public bool CanAutoEquipIgnoreLockedItems { get; set; } = true;

        //[SettingPropertyBool("Save AutoEquip Settings on Inventory Cancel", Order = 1, RequireRestart = false, HintText = "By default hitting cancel will follow the M&B logic of saving nothing,\nwith this setting though you can always save changes to AutoEquip even if you hit cancel.")]
        //[SettingPropertyGroup("General")]
        //public bool SaveAutoEquipSettingsOnInventoryCancel { get; set; } = false;

        [SettingPropertyButton("Configure Presets", Content = "Create preset", Order = 99, RequireRestart = false)]
        [SettingPropertyGroup("Presets")]
        public Action ConfigurePresets { get; set; } = (() =>
        {
        });
    }
}
