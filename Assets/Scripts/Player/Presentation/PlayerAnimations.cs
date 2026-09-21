using MFramework.Event;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour, IEventReceiver<PlayerStateChangedEvent>, IEventReceiver<PlayerLevelChangedEvent>
{
    [SerializeField] private Animator visualAnimator;
    [SerializeField] private SpriteRenderer visualRenderer;
    [SerializeField] private SpriteRenderer stateMarker;
    [SerializeField] private ParticleSystem footDust;
    [SerializeField] private ParticleSystem stateParticles;
    [SerializeField] private GameObject fullWaterAura;
    [SerializeField] private GameObject flightAura;
    [SerializeField] private ParticleSystem flightStream;
    [SerializeField] private GameObject coldWaterAura;
    private Animator anim;
    private PlayerState playerState;
    private PlayerLevelControl levelControl;
    private PlayerMoveControl movement;
    private Rigidbody2D body;
    private bool wasGrounded;
    private float nextFootstep;
    private float exitTime;

    private void Awake()
    {
        anim = visualAnimator != null ? visualAnimator : GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();
        levelControl = GetComponent<PlayerLevelControl>();
        movement = GetComponent<PlayerMoveControl>();
        body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerStateChangedEvent>(this);
        EventBus.Subscribe<PlayerLevelChangedEvent>(this);
        ShowState();
        anim.SetBool("death", levelControl.death);
        anim.SetBool("Door1", levelControl.Door1);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerStateChangedEvent>(this);
        EventBus.Unsubscribe<PlayerLevelChangedEvent>(this);
        SetEffectVisible(fullWaterAura, false);
        SetEffectVisible(flightAura, false);
        SetEffectVisible(coldWaterAura, false);
        if (flightStream != null)
            flightStream.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void OnEvent(PlayerStateChangedEvent evt)
    {
        if (evt.Player == playerState)
        {
            ShowState();
            if (stateParticles != null)
                stateParticles.Emit(12);
        }
    }

    public void OnEvent(PlayerLevelChangedEvent evt)
    {
        if (evt.Player != levelControl)
            return;
        anim.SetBool("death", evt.IsDead);
        anim.SetBool("Door1", evt.HasReachedExit);
        RefreshStateEffects();
        if (evt.HasReachedExit)
            exitTime = Time.time;
    }

    private void LateUpdate()
    {
        RefreshStateEffects();
        if (visualAnimator == null)
            return;

        bool grounded = movement.isGrounded;
        anim.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        anim.SetFloat("VerticalSpeed", body.velocity.y);
        anim.SetBool("Grounded", grounded);
        if (!levelControl.IsStopped && Time.timeScale > 0f && footDust != null)
        {
            if (grounded && !wasGrounded)
                footDust.Emit(7);
            else if (grounded && Mathf.Abs(body.velocity.x) > 0.2f && Time.time >= nextFootstep)
            {
                footDust.Emit(2);
                nextFootstep = Time.time + 0.16f;
            }
        }
        wasGrounded = grounded;

        if (levelControl.Door1 && visualRenderer != null)
        {
            Color color = visualRenderer.color;
            color.a = 1f - Mathf.Clamp01((Time.time - exitTime) / 0.85f);
            visualRenderer.color = color;
            if (stateMarker != null)
                stateMarker.color = new Color(stateMarker.color.r, stateMarker.color.g, stateMarker.color.b, color.a);
        }
    }

    private void ShowState()
    {
        anim.SetFloat("Full", playerState.CurrentMass);
        anim.SetFloat("Fly", playerState.CurrentMass);
        Color tint = playerState.CurrentState == PlayerState.State.Full ? new Color(0.65f, 0.94f, 1f) :
                     playerState.CurrentState == PlayerState.State.Fly ? new Color(1f, 0.88f, 0.48f) : new Color(0.86f, 0.84f, 0.94f);
        if (visualRenderer != null)
            visualRenderer.color = tint;
        if (stateMarker != null)
            stateMarker.color = tint;
        if (stateParticles != null)
        {
            var main = stateParticles.main;
            main.startColor = tint;
        }
        RefreshStateEffects();
    }

    private void RefreshStateEffects()
    {
        bool visible = !levelControl.IsStopped;
        SetEffectVisible(fullWaterAura, visible && playerState.CurrentState == PlayerState.State.Full);
        SetEffectVisible(flightAura, visible && playerState.CurrentState == PlayerState.State.Fly);
        SetEffectVisible(coldWaterAura, visible && playerState.IceBlock && playerState.CurrentState == PlayerState.State.Full);
        if (flightStream == null)
            return;

        bool flying = visible && playerState.IsFlying;
        if (flying && !flightStream.isPlaying)
            flightStream.Play();
        else if (!flying && (flightStream.isPlaying || flightStream.particleCount > 0))
            flightStream.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private static void SetEffectVisible(GameObject effect, bool visible)
    {
        if (effect != null && effect.activeSelf != visible)
            effect.SetActive(visible);
    }
}
