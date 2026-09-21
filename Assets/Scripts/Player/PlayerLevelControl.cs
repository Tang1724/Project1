using System.Collections;
using MFramework.Event;
using UnityEngine;

public class PlayerLevelControl : MonoBehaviour
{
    public bool death { get; private set; }
    public bool Door1 { get; private set; }
    public bool IsStopped => death || Door1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (IsStopped)
            return;

        if (InputSystemController.PreviousLevelPressed)
            LevelFlow.LoadPreviousLevel();
        else if (InputSystemController.NextLevelPressed)
            LevelFlow.LoadNextLevel();
        else if (InputSystemController.RestartPressed)
            LevelFlow.RestartGame();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("End"))
            EventBus.Publish(new SoundRequestedEvent("Firesound"));
        if (collider.CompareTag("Trap"))
            EventBus.Publish(new SoundRequestedEvent("Deadsound"));
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (IsStopped)
            return;

        if (collider.CompareTag("Trap"))
            death = true;
        else if (collider.CompareTag("End"))
            Door1 = true;
        else
            return;

        EventBus.Publish(new PlayerLevelChangedEvent(this, death, Door1));
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        StartCoroutine(CompleteAfterDelay());
    }

    private IEnumerator CompleteAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        if (death)
            LevelFlow.RestartGame();
        else
            LevelFlow.LoadNextLevel();
    }
}
