namespace Serjbal
{
    public class OnItemsUpdate : InventoryEvent
    {
        public InventoryModel Model { get; }
        public OnItemsUpdate(InventoryModel inventoryModel)
        {
            Model = inventoryModel;
        }
    }
}