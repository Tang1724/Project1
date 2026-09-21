using UnityEngine;
using MFramework.Event;

public class InputSystemController : MonoBehaviour
{
    private static bool CanPlay => GameModeManager.Instance == null || GameModeManager.Instance.CurrentMode == GameMode.Playing;

    public static float Movement => CanPlay ? Input.GetAxisRaw("Horizontal") : 0f;
    public static bool JumpPressed => CanPlay && Input.GetButtonDown("Jump");
    public static bool ReleasePressed => CanPlay && Input.GetKeyDown(KeyCode.E);
    public static bool RestartPressed => CanPlay && Input.GetKeyDown(KeyCode.R);
    public static bool PreviousLevelPressed => CanPlay && Input.GetKeyDown(KeyCode.Alpha1);
    public static bool NextLevelPressed => CanPlay && Input.GetKeyDown(KeyCode.Alpha2);
    public static bool PausePressed => Input.GetKeyDown(KeyCode.Escape);
    public static Vector3 PointerPosition => Input.mousePosition;

    private void Update()
    {
        if (PausePressed && GameModeManager.Instance != null)
        {
            GameMode nextMode = GameModeManager.Instance.CurrentMode == GameMode.Pause ? GameMode.Playing : GameMode.Pause;
            EventBus.Publish(new GameModeChangeRequestedEvent(nextMode));
        }
    }
}
