using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace AutoEquipCompanions.ViewModel
{
    public class AutoEquipWidget : ButtonWidget
    {
        public AutoEquipWidget(UIContext context) : base(context)
        {
            Brush = context.BrushFactory.GetBrush("Inventory.AutoEquipCompanions.UpgradeButton");
            WidthSizePolicy = SizePolicy.Fixed;
            HeightSizePolicy = SizePolicy.Fixed;
            SuggestedWidth = 30;
            SuggestedHeight = 30;
            VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
