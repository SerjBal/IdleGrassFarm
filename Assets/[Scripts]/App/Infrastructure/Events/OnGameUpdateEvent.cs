namespace Serjbal
{
    public class OnGameUpdateEvent : GameEvent
    {
        public InventoryModel Model { get; }
        public OnGameUpdateEvent(InventoryModel inventoryModel)
        {
            Model = inventoryModel;
        }
    }
}