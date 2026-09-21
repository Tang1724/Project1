using MFramework.Event;

public readonly struct PlatformMassChangedEvent : IEvent
{
    public Floor Platform { get; }
    public float TotalMass { get; }
    public float RequiredMass { get; }

    public PlatformMassChangedEvent(Floor platform, float totalMass, float requiredMass)
    {
        Platform = platform;
        TotalMass = totalMass;
        RequiredMass = requiredMass;
    }
}
