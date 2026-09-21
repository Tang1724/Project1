using MFramework.Event;

public readonly struct PlayerStateChangedEvent : IEvent
{
    public PlayerState Player { get; }
    public PlayerState.State PreviousState { get; }
    public PlayerState.State CurrentState { get; }

    public PlayerStateChangedEvent(PlayerState player, PlayerState.State previousState, PlayerState.State currentState)
    {
        Player = player;
        PreviousState = previousState;
        CurrentState = currentState;
    }
}
