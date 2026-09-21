using MFramework.Event;
using UnityEngine;

[RequireComponent(typeof(GameModeManager), typeof(InputSystemController))]
public class MenuControl : MonoBehaviour, IEventReceiver<GameModeChangedEvent>
{
    public GameObject pauseMenuUI;
    private GameModeManager gameMode;

    private void Awake()
    {
        gameMode = GetComponent<GameModeManager>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GameModeChangedEvent>(this);
    }

    private void Start()
    {
        pauseMenuUI.SetActive(gameMode.CurrentMode == GameMode.Pause);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameModeChangedEvent>(this);
    }

    public void OnEvent(GameModeChangedEvent evt)
    {
        pauseMenuUI.SetActive(evt.Mode == GameMode.Pause);
    }

    public void Resume() { gameMode.ChangeMode(GameMode.Playing); }
    public void Pause() { gameMode.ChangeMode(GameMode.Pause); }
    public void RestartGame() { LevelFlow.RestartGame(); }
    public void LoadMainMenu() { LevelFlow.LoadMainMenu(); }
    public void QuitGame() { LevelFlow.QuitGame(); }
}
