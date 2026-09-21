using MFramework.Event;

public readonly struct BottleStateChangedEvent : IEvent
{
    public Bottle Bottle { get; }
    public Bottle.BottleState State { get; }

    public BottleStateChangedEvent(Bottle bottle, Bottle.BottleState state)
    {
        Bottle = bottle;
        State = state;
    }
}
