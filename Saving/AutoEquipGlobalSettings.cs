using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace AutoEquipCompanions.Saving
{
    internal class AutoEquipGlobalSettings : AttributeGlobalSettings<AutoEquipGlobalSettings>
    {
        public override string Id => "AutoEquipMCMUI";
        public override string DisplayName => "Auto Equip Companions";

        [SettingPropertyBool("Use Locked Items", Order = 0, RequireRestart = false, HintText = "If you turn this off the auto equip will not equip locked items.")]
        [SettingPropertyGroup("General")]
        public bool SettingVariableName { get; set; } = true;
    }
}
