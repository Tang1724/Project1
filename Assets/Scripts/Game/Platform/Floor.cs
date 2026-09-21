using MFramework.Event;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Transform targetPosition;
    public Transform startPosition;
    public float Mass = 8f;
    public float speed = 0.5f;
    [SerializeField] private float totalMass;
    [SerializeField] private bool OnTop;
    private float lastRequiredMass;
    private Rigidbody2D body;
    private Vector2 moveVelocity;
    private Collider2D platformCollider;
    private readonly List<Collider2D> supports = new List<Collider2D>();
    private readonly HashSet<Collider2D> visitedColliders = new HashSet<Collider2D>();
    private readonly HashSet<IMassProvider> countedProviders = new HashSet<IMassProvider>();
    private readonly List<ContactPoint2D> contacts = new List<ContactPoint2D>();

    public float TotalMass => totalMass;
    public float RequiredMass => Mass;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        if (body == null)
            body = gameObject.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Start()
    {
        lastRequiredMass = Mass;
        EventBus.Publish(new PlatformMassChangedEvent(this, totalMass, Mass));
    }

    private void FixedUpdate()
    {
        RecalculateMass();
        Vector2 target = OnTop && totalMass >= Mass ? targetPosition.position : startPosition.position;
        Vector2 remaining = target - body.position;
        // 向下启动和向上刹停的加速度低于重力，承重物体才能持续贴住台面。
        float acceleration = Mathf.Min(10f, Mathf.Abs(Physics2D.gravity.y) * 0.5f);
        float brakingSpeed = Mathf.Sqrt(2f * acceleration * remaining.magnitude);
        Vector2 desiredVelocity = remaining.normalized * Mathf.Min(speed, brakingSpeed);
        moveVelocity = Vector2.MoveTowards(moveVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);
        Vector2 movement = moveVelocity * Time.fixedDeltaTime;
        if (Vector2.Dot(movement, remaining) >= 0f && movement.sqrMagnitude >= remaining.sqrMagnitude)
        {
            movement = remaining;
            moveVelocity = Vector2.zero;
        }
        // 动态物体由物理接触承托，避免父子 Transform 位移与刚体解算同时搬动冰块。
        body.MovePosition(body.position + movement);
    }

    private void Update()
    {
        RecalculateMass();
    }

    public void RecalculateMass()
    {
        supports.Clear();
        visitedColliders.Clear();
        countedProviders.Clear();
        supports.Add(platformCollider);
        visitedColliders.Add(platformCollider);
        float mass = 0f;

        for (int index = 0; index < supports.Count; index++)
        {
            Collider2D support = supports[index];
            support.GetContacts(contacts);
            foreach (ContactPoint2D contact in contacts)
            {
                Vector2 normal = contact.otherCollider == support ? contact.normal : -contact.normal;
                if (normal.y > -0.5f)
                    continue;

                Collider2D above = contact.otherCollider == support ? contact.collider : contact.otherCollider;
                Rigidbody2D supportedBody = above.attachedRigidbody;
                if (!above.enabled || supportedBody == null || !supportedBody.simulated)
                    continue;

                // 沿向上的承托接触遍历；侧碰墙壁不算载重，跨两块底座的同一冰块只计一次。
                foreach (MonoBehaviour component in supportedBody.GetComponentsInChildren<MonoBehaviour>())
                {
                    IMassProvider provider = component as IMassProvider;
                    if (provider == null || !component.isActiveAndEnabled || !countedProviders.Add(provider))
                        continue;
                    mass += provider.CurrentMass;
                    foreach (Collider2D collider in component.GetComponents<Collider2D>())
                    {
                        if (collider.enabled && !collider.isTrigger && visitedColliders.Add(collider))
                            supports.Add(collider);
                    }
                }
            }
        }

        OnTop = countedProviders.Count > 0;
        if (totalMass == mass && lastRequiredMass == Mass)
            return;

        totalMass = mass;
        lastRequiredMass = Mass;
        EventBus.Publish(new PlatformMassChangedEvent(this, totalMass, Mass));
    }

    private void OnDrawGizmos()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Gizmos.color = Color.green;
        Bounds bounds = collider.bounds;
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y), new Vector3(bounds.max.x, bounds.max.y));
    }
}
