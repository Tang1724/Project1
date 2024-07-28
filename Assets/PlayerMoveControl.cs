using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerState playerState;

    public AnimationCurve jumpCurve = new AnimationCurve(
        new Keyframe(0, 0, 0, Mathf.Tan(Mathf.Deg2Rad * 75)),  // 起始点，切线向上斜率大
        new Keyframe(0.25f, 0.5f, 0, 0),  // 上升最高点，切线水平
        new Keyframe(0.5f, 0, Mathf.Tan(Mathf.Deg2Rad * -75), 0)  // 下降结束点，切线向下斜率大
    );

    [Header("移动参数")]
    public float fullspeed = 4f;
    public float emptyspeed =7f; 
    private float xVelocity;

    [Header("跳跃参数")]
    public float jumpFullForce = 24.2f;
    public float jumpEmptyForce = 52.2f;
    public float jumpDuration = 0.4f;  // 跳跃动作的总持续时间

    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        Debug.Log("Is Grounded: " + isGrounded);
        Debug.Log("Current State: " + playerState.state);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            StartCoroutine(ApplyJumpCurve());
            isGrounded = false;  // 确保在空中时不会重新触发跳跃
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerState.ChangeState();
        }
    }

    private void FixedUpdate()
    {
        GroundMovement();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Bottle")
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Water")
        {
            playerState.ChangeState("FULL");
        }
    }

    void GroundMovement()
    {
        xVelocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xVelocity * playerState.Speed, rb.velocity.y);
        FilpDirection();
    }

    void FilpDirection()
    {
        Vector3 scale = transform.localScale;
        if (xVelocity < 0)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else if (xVelocity > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    IEnumerator ApplyJumpCurve()
    {
        float timer = 0;
        float initialJumpForce = (playerState.state == "FULL") ? jumpFullForce : jumpEmptyForce;

        while (timer < jumpDuration)
        {
            float proportionalTime = timer / jumpDuration; // 计算时间比例
            float curveValue = jumpCurve.Evaluate(proportionalTime); // 从曲线获取当前力度比例
            float currentForce = curveValue * initialJumpForce;
            rb.velocity = new Vector2(rb.velocity.x, currentForce);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}