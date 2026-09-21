using MFramework.Event;

public readonly struct SoundRequestedEvent : IEvent
{
    public string Name { get; }

    public SoundRequestedEvent(string name)
    {
        Name = name;
    }
}
