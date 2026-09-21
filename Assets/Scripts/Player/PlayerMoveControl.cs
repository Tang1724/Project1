using MFramework.Event;
using UnityEngine;

[RequireComponent(typeof(PlayerState), typeof(PlayerLevelControl), typeof(Rigidbody2D))]
public class PlayerMoveControl : MonoBehaviour
{
    [SerializeField] private PlayerDefinitionSo definition;
    private Rigidbody2D rb;
    private PlayerState playerState;
    private PlayerLevelControl levelControl;
    public Vector3 colliderOffset;
    public LayerMask groundLayer;
    public float groundLength;
    public bool isGrounded = true;
    private int jumpCount;
    public bool isJump;
    private bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        levelControl = GetComponent<PlayerLevelControl>();
        if (definition == null)
        {
            Debug.LogError("PlayerMoveControl requires a PlayerDefinitionSo.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (levelControl.IsStopped)
            return;

        if (InputSystemController.JumpPressed && isGrounded)
            jumpPressed = true;

        if (InputSystemController.ReleasePressed)
            playerState.Release();

        if (InputSystemController.JumpPressed)
            playerState.BeginFlight();

        if (playerState.IsFlying)
            rb.velocity = new Vector2(rb.velocity.x, definition.FlyForce);
    }

    private void FixedUpdate()
    {
        if (levelControl.IsStopped)
            return;

        GroundMovement();
        Jumps();
    }

    private void GroundMovement()
    {
        float horizontalMove = InputSystemController.Movement;
        Vector3 scale = transform.localScale;
        rb.velocity = new Vector2(horizontalMove * definition.GetSpeed(playerState.CurrentState), rb.velocity.y);
        if (horizontalMove != 0)
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(horizontalMove);
        transform.localScale = scale;
    }

    private void Jumps()
    {
        if (jumpPressed)
        {
            if (isGrounded || (jumpCount > 0 && isJump))
            {
                rb.velocity = new Vector2(rb.velocity.x, definition.GetJumpForce(playerState.CurrentState));
                jumpCount--;
            }

            jumpPressed = false;
            EventBus.Publish(new SoundRequestedEvent("jumpsound"));
        }

        isGrounded = IsOnGround();
    }

    private bool IsOnGround()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector3(transform.position.x - colliderOffset.x, transform.position.y - colliderOffset.y, transform.position.z), Vector2.down, groundLength, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x + colliderOffset.x, transform.position.y - colliderOffset.y, transform.position.z), Vector2.down, groundLength, groundLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - colliderOffset.y, transform.position.z), Vector2.down, groundLength, groundLayer);
        return hit1 || hit2 || hit3;
    }

    private void OnDrawGizmos()
    {
    
    Gizmos.color = Color.red;
    Vector3 start1 = new Vector3(transform.position.x-colliderOffset.x,transform.position.y - colliderOffset.y,transform.position.z);
    
    Vector3 end1 = new Vector3(transform.position.x-colliderOffset.x,transform.position.y - colliderOffset.y - groundLength,transform.position.z);
    
    Vector3 start2 = new Vector3(transform.position.x+colliderOffset.x,transform.position.y - colliderOffset.y,transform.position.z);
    
    Vector3 end2 = new Vector3(transform.position.x+colliderOffset.x,transform.position.y - colliderOffset.y - groundLength,transform.position.z);
    
    Vector3 start3 = new Vector3(transform.position.x,transform.position.y - colliderOffset.y,transform.position.z);
    
    Vector3 end3 = new Vector3(transform.position.x,transform.position.y - colliderOffset.y - groundLength,transform.position.z);



    
    RaycastHit2D hit1 = Physics2D.Raycast(start1, Vector2.down, groundLength, groundLayer);
    
    RaycastHit2D hit2 = Physics2D.Raycast(start2, Vector2.down, groundLength, groundLayer);

    RaycastHit2D hit3 = Physics2D.Raycast(start3, Vector2.down, groundLength, groundLayer);
    
    if (hit1.collider != null)
    {
        Gizmos.DrawLine(start1, hit1.point);
    }
    else
    {
        Gizmos.DrawLine(start1, end1);
    }

    if (hit2.collider != null)
    {
        Gizmos.DrawLine(start2, hit2.point);
    }
    else
    {
        Gizmos.DrawLine(start2, end2);
    }
    
    if (hit3.collider != null)
    {
        Gizmos.DrawLine(start3, hit3.point);
    }
    else
    {
        Gizmos.DrawLine(start3, end3);
    }
}

}
