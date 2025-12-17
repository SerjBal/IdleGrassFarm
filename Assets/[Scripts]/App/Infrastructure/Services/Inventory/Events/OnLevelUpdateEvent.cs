namespace Serjbal.Infrastructure.Services
{
    public class OnLevelUpdateEvent : InventoryEvent
    {
        public int Value { get; }

        public OnLevelUpdateEvent(int diff)
        {
            Value = diff;
        }
    }
}