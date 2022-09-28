using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI.BaseTypes;

namespace AutoEquipCompanions
{
    public static class Utilities
    {
        public static IEnumerable<Widget> GetWidgetDescendants(Widget widget)
        {
            return widget.Children.SelectMany(child => GetWidgetDescendants(child).Append(child));
        }
    }
}
