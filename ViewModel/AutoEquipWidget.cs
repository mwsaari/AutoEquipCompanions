using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace AutoEquipCompanions.ViewModel
{
    public class AutoEquipWidget : ButtonWidget
    {
        public AutoEquipWidget(UIContext context) : base(context)
        {
            WidthSizePolicy = SizePolicy.Fixed;
            HeightSizePolicy = SizePolicy.Fixed;
            SuggestedWidth = 30;
            SuggestedHeight = 30;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
