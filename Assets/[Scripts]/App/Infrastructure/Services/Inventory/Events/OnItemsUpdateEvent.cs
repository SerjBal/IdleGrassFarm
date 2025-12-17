namespace Serjbal
{
    public class OnItemsUpdateEvent : InventoryEvent
    {
        public InventoryModel Model { get; }
        public OnItemsUpdateEvent(InventoryModel inventoryModel)
        {
            Model = inventoryModel;
        }
    }
}