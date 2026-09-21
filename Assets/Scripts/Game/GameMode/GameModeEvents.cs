using MFramework.Event;

public readonly struct GameModeChangeRequestedEvent : IEvent
{
    public GameMode Mode { get; }

    public GameModeChangeRequestedEvent(GameMode mode)
    {
        Mode = mode;
    }
}

public readonly struct GameModeChangedEvent : IEvent
{
    public GameMode Mode { get; }

    public GameModeChangedEvent(GameMode mode)
    {
        Mode = mode;
    }
}
