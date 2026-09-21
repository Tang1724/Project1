using MFramework.Event;
using UnityEngine;

public class Bottle : MonoBehaviour, IMassProvider, IEventReceiver<PlayerStateChangedEvent>
{
    public enum BottleState
    {
        Empty,
        Full,
        Fly
    }

    [SerializeField] private BottleState state = BottleState.Empty;
    [SerializeField] private bool PlayerinBottle;
    public PlayerState playerState;
    public float BottleFlyForce = 5f;
    public PhysicsMaterial2D BottleEmpty;
    public PhysicsMaterial2D BottleFull;
    private Rigidbody2D rb;
    private Collider2D coll;
    private Collider2D[] bodyColliders;
    private int[] originalForceReceiveLayers;
    private int playerLayerMask;

    public BottleState CurrentState => state;
    public float CurrentMass => state == BottleState.Full ? 5f : state == BottleState.Empty ? 1f : 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        bodyColliders = GetComponents<Collider2D>();
        originalForceReceiveLayers = new int[bodyColliders.Length];
        for (int i = 0; i < bodyColliders.Length; i++)
            originalForceReceiveLayers[i] = bodyColliders[i].forceReceiveLayers;
        playerLayerMask = LayerMask.GetMask("Player");
        if (playerState == null || !playerState.gameObject.scene.IsValid())
            playerState = FindObjectOfType<PlayerState>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerStateChangedEvent>(this);
        UpdatePushPermission();
    }

    private void Start()
    {
        EventBus.Publish(new BottleStateChangedEvent(this, state));
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerStateChangedEvent>(this);
        for (int i = 0; i < bodyColliders.Length; i++)
            bodyColliders[i].forceReceiveLayers = originalForceReceiveLayers[i];
    }

    private void Update()
    {
        if (state == BottleState.Fly)
            rb.velocity = new Vector2(rb.velocity.x, BottleFlyForce);
    }

    public void OnEvent(PlayerStateChangedEvent evt)
    {
        if (evt.Player != playerState)
            return;

        UpdatePushPermission();
        if (!PlayerinBottle || evt.CurrentState != PlayerState.State.Empty)
            return;

        if (evt.PreviousState == PlayerState.State.Full)
            ChangeState(BottleState.Full);
        else if (evt.PreviousState == PlayerState.State.Fly)
            ChangeState(BottleState.Fly);
    }

    private void ChangeState(BottleState newState)
    {
        if (state == newState)
            return;
        state = newState;
        EventBus.Publish(new BottleStateChangedEvent(this, state));
    }

    public void HandleTriggerEnter(Collider2D collider)
    {
        if (!isActiveAndEnabled || !collider.gameObject.activeInHierarchy)
            return;
        if (collider.CompareTag("Player") || collider.CompareTag("Bottle"))
            collider.transform.SetParent(transform, true);
    }

    public void HandleTriggerStay(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerinBottle = true;
            playerState = collider.GetComponent<PlayerState>();
            UpdatePushPermission();
        }
    }

    private void UpdatePushPermission()
    {
        bool canPush = playerState != null && playerState.CurrentState == PlayerState.State.Full;
        coll.sharedMaterial = canPush ? BottleFull : BottleEmpty;
        // 非满水时只屏蔽玩家传来的碰撞力，保留瓶子的重力和其他物体的作用力。
        for (int i = 0; i < bodyColliders.Length; i++)
            bodyColliders[i].forceReceiveLayers = canPush
                ? originalForceReceiveLayers[i]
                : originalForceReceiveLayers[i] & ~playerLayerMask;
    }

    public void HandleTriggerExit(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            PlayerinBottle = false;

        // 停用父物体也会触发退出回调，此时 Unity 不允许修改其子物体层级。
        if (!gameObject.activeInHierarchy || !collider.gameObject.activeInHierarchy)
            return;
        if ((collider.CompareTag("Player") || collider.CompareTag("Bottle")) && collider.transform.parent == transform)
            collider.transform.SetParent(null);
    }
}
