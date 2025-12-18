namespace Serjbal
{
    public class OnInventoryUpdateEvent : InventoryEvent
    {
        public InventoryModel Model { get; }
        public OnInventoryUpdateEvent(InventoryModel inventoryModel)
        {
            Model = inventoryModel;
        }
    }
}