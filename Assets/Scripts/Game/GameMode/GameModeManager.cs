using MFramework;
using MFramework.Event;
using UnityEngine;

public class GameModeManager : Singleton<GameModeManager>, IEventReceiver<GameModeChangeRequestedEvent>
{
    public GameMode CurrentMode { get; private set; } = GameMode.Playing;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
            Time.timeScale = 1f;
    }

    private void Start()
    {
        if (Instance == this)
            EventBus.Publish(new GameModeChangedEvent(CurrentMode));
    }

    private void OnEnable()
    {
        if (Instance == this)
            EventBus.Subscribe<GameModeChangeRequestedEvent>(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameModeChangeRequestedEvent>(this);
    }

    public void OnEvent(GameModeChangeRequestedEvent evt)
    {
        ChangeMode(evt.Mode);
    }

    public void ChangeMode(GameMode mode)
    {
        if (Instance != this || CurrentMode == mode)
            return;

        CurrentMode = mode;
        Time.timeScale = mode == GameMode.Pause ? 0f : 1f;
        EventBus.Publish(new GameModeChangedEvent(mode));
    }
}
