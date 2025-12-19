namespace Serjbal.Infrastructure.Services
{
    public class OnLevelUpdateEvent : GameEvent
    {
        public int Value { get; }

        public OnLevelUpdateEvent(int level)
        {
            Value = level;
        }
    }
}