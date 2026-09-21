using MFramework.Event;

public readonly struct PlayerLevelChangedEvent : IEvent
{
    public PlayerLevelControl Player { get; }
    public bool IsDead { get; }
    public bool HasReachedExit { get; }

    public PlayerLevelChangedEvent(PlayerLevelControl player, bool isDead, bool hasReachedExit)
    {
        Player = player;
        IsDead = isDead;
        HasReachedExit = hasReachedExit;
    }
}
