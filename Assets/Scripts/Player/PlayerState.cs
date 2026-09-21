using System.Collections;
using System.Collections.Generic;
using MFramework.Event;
using UnityEngine;

public class PlayerState : MonoBehaviour, IMassProvider
{
    public enum State
    {
        Full,
        Empty,
        Fly
    }

    [SerializeField] private State currentState = State.Full;
    [SerializeField] private GameObject iceBlockPrefab;
    public float spawnDistance = 0.2f;
    public bool IceBlock = false;
    public float cooldown = 0.75f;
    public float flydown = 1f;
    public bool isWithinWater;
    private float lastEmptyTime;
    private readonly List<Collider2D> iceSpawnOverlaps = new List<Collider2D>();

    public State CurrentState => currentState;
    public bool IsFlying { get; private set; }
    public float CurrentMass => currentState == State.Full ? 5f : currentState == State.Empty ? 1f : 0f;

    private void Update()
    {
        if (isWithinWater && currentState == State.Empty && Time.time >= lastEmptyTime + cooldown)
            ChangeState(State.Full);
    }

    public void Release()
    {
        if (currentState == State.Full && IceBlock && !TryCreateIceBlock())
            return;

        if (currentState == State.Full || currentState == State.Fly)
        {
            ChangeState(State.Empty);
            EventBus.Publish(new SoundRequestedEvent("Pushsound"));
        }

        IsFlying = false;
    }

    public void BeginFlight()
    {
        if (currentState == State.Fly)
            IsFlying = true;
    }

    public void ChangeState(State newState)
    {
        if (currentState == newState)
            return;

        State previousState = currentState;
        currentState = newState;
        if (newState == State.Empty)
            lastEmptyTime = Time.time;
        if (newState != State.Fly)
            IsFlying = false;

        EventBus.Publish(new PlayerStateChangedEvent(this, previousState, newState));
    }

    public void CreateIceBlock()
    {
        TryCreateIceBlock();
    }

    private bool TryCreateIceBlock()
    {
        const float clearance = 0.02f;
        Physics2D.SyncTransforms();
        Bounds playerBounds = GetComponent<Collider2D>().bounds;
        BoxCollider2D iceCollider = iceBlockPrefab.GetComponent<BoxCollider2D>();
        Vector3 scale = iceBlockPrefab.transform.localScale;
        Vector2 iceSize = Vector2.Scale(iceCollider.size, new Vector2(Mathf.Abs(scale.x), Mathf.Abs(scale.y)));
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 center = new Vector2(
            playerBounds.center.x + direction * (playerBounds.extents.x + iceSize.x * 0.5f + Mathf.Max(clearance, spawnDistance)),
            playerBounds.min.y + iceSize.y * 0.5f + clearance);
        var filter = new ContactFilter2D();
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(iceBlockPrefab.layer));
        filter.useTriggers = false;
        Physics2D.OverlapBox(center, iceSize + Vector2.one * clearance, 0f, filter, iceSpawnOverlaps);
        // 生成空间被占用时保留水量，避免物理解算器把重叠的冰块和人物强行挤开。
        if (iceSpawnOverlaps.Count > 0)
            return false;

        Vector2 offset = Vector2.Scale(iceCollider.offset, new Vector2(scale.x, scale.y));
        Vector3 spawnPosition = new Vector3(center.x - offset.x, center.y - offset.y, transform.position.z);
        Instantiate(iceBlockPrefab, spawnPosition, Quaternion.identity);
        return true;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Water"))
        {
            if (currentState != State.Full)
            {
                ChangeState(State.Full);
                EventBus.Publish(new SoundRequestedEvent("Watersound"));
            }
            IsFlying = false;
        }

        if (collider.CompareTag("Purified water") && currentState != State.Fly)
        {
            ChangeState(State.Fly);
            EventBus.Publish(new SoundRequestedEvent("Watersound"));
        }

        if (collider.CompareTag("Empty"))
        {
            ChangeState(State.Empty);
            IsFlying = false;
            EventBus.Publish(new SoundRequestedEvent("Pushsound"));
        }

        if (collider.CompareTag("Cold"))
            IceBlock = true;

        if (collider.CompareTag("water1"))
        {
            isWithinWater = true;
            if (currentState == State.Empty && Time.time >= lastEmptyTime + cooldown)
                ChangeState(State.Full);
            if (currentState == State.Fly && IsFlying)
                StartCoroutine(ExecuteAfterDelay());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cold"))
            IceBlock = false;
        if (other.CompareTag("water1"))
            isWithinWater = false;
    }

    private IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        ChangeState(State.Full);
        IsFlying = false;
    }
}
