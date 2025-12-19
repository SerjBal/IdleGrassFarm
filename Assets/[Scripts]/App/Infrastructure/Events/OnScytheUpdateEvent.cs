namespace Serjbal
{
    public class OnScytheUpdateEvent: GameEvent
    {
        public ScytheModel Model { get; }
        public OnScytheUpdateEvent(ScytheModel scytheModel)
        {
            Model = scytheModel;
        }
    }
}