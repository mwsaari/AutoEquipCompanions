using SandBox.GauntletUI;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory;

namespace AutoEquipCompanions.ViewModel
{
    public class AutoEquipOverlayViewModel : TaleWorlds.Library.ViewModel
    {
        private static InventoryEquippedItemSlotWidget[] _equipmentSlotWidgets = new InventoryEquippedItemSlotWidget[(int)EquipmentIndex.NumEquipmentSetSlots];
        private int _bodySlot;
        private int bodySlot;
        private int bodySlot;

        public AutoEquipOverlayViewModel(GauntletInventoryScreen inventoryScreen)
        {
            foreach (var slot in GetEquipmentSlotWidgets(inventoryScreen))
            {
                _equipmentSlotWidgets[slot.EquipmentIndex] = slot;
                slot.PropertyChanged += OnSlotPropertyChanged;
            }
        }

        public int BodySlot { get => bodySlot; set { bodySlot = value; OnPropertyChanged(); } }
        public int HorseSlot { get => bodySlot; set { bodySlot = value; OnPropertyChanged(); } }
        public int Weapon1Slot { get => bodySlot; set { bodySlot = value; OnPropertyChanged(); } }

        public static IReadOnlyList<InventoryEquippedItemSlotWidget> EquipmentSlotWidgets => _equipmentSlotWidgets;

        private void OnSlotPropertyChanged(PropertyOwnerObject obj, string propertyName, object value)
        {
            if (obj is InventoryEquippedItemSlotWidget slot && propertyName == nameof(InventoryEquippedItemSlotWidget.TargetEquipmentIndex))
            {
                _equipmentSlotWidgets[slot.TargetEquipmentIndex] = slot;
            }
        }

        private IEnumerable<InventoryEquippedItemSlotWidget> GetEquipmentSlotWidgets(GauntletInventoryScreen inventoryScreen)
        {
            var inventoryRootWidget = FindInventoryScreenRootWidget(inventoryScreen);
            return Utilities.GetWidgetDescendants(inventoryRootWidget).OfType<InventoryEquippedItemSlotWidget>();
        }

        private Widget FindInventoryScreenRootWidget(GauntletInventoryScreen inventoryScreen)
        {
            // This is written as a loop for safety. But really inventory screen will be first layer, and inventoryVM will be first view.
            return inventoryScreen.Layers
                .OfType<GauntletLayer>()
                .SelectMany(gauntletLayer => gauntletLayer._moviesAndDatasources
                    .Where(views => views.Item2 is SPInventoryVM)
                    .Select(views => (views.Item1.RootWidget)))
                    .First();
        }
    }
}
