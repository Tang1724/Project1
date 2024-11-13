using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMoveControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerState playerState;
    private Collider2D coll;
    private Animator anim;
    private Bottle bottle;
    public float fullspeed = 4f;
    public float emptyspeed = 6f; 
    public float flyspeed = 6f;
    public float jumpFullForce = 5f;
    public float jumpEmptyForce = 8f;
    public float FlyForce = 5f;
    public Vector3 colliderOffset;
    public LayerMask groundLayer;
    public float groundLength;
    public bool isGrounded = true;
    private int jumpCount;
    public bool isJump;
    bool jumpPressed;
    public bool StartFly = false;
    public bool death = false;
    public bool Door1 = false;

    // // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();
        bottle = GetComponent<Bottle>();
    }

    // Update is called once per frame
    void Update()
    {
        if(death){{
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
        }
        
            return;}

        if(Door1){{
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
        }
        
            return;}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadPreviousLevel();
        }

        // 按键2转移到下一关
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadNextLevel();
        }
        if(Input.GetButtonDown("Jump") && isGrounded){
            jumpPressed = true; 
        }

        
        if(playerState.currentState == PlayerState.State.Full){
            if(playerState.IceBlock == true)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    playerState.CreateIceBlock();
                }
            }
        }

       if (Input.GetKeyDown(KeyCode.E))
    {
        if (playerState.currentState == PlayerState.State.Full || playerState.currentState == PlayerState.State.Fly)
        {
            playerState.ChangeState(PlayerState.State.Empty);
            AudioManager.instance.PlaySound("Pushsound");
        }
        StartFly = false;
    }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // 获取当前活动的场景名字
            string sceneName = SceneManager.GetActiveScene().name;
            // 重载当前场景
            SceneManager.LoadScene(sceneName);
        }

        if (playerState.currentState == PlayerState.State.Fly)
        {
            if(Input.GetButtonDown("Jump")){
                StartFly = true;

            }
         }

        if (StartFly)
        {
            // 在飞行状态下且飞行已开始，自动持续向上移动
            rb.velocity = new Vector2(rb.velocity.x, FlyForce);
        }

        
    }


    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("End"))
        {
            AudioManager.instance.PlaySound("Firesound");
        }

        if (collider.gameObject.CompareTag("Trap"))
        {
            AudioManager.instance.PlaySound("Deadsound");
        }

        if (collider.gameObject.tag == "Water")
        {

            AudioManager.instance.PlaySound("Watersound");
        }
            
        if (collider.gameObject.tag == "Purified water")
        {
            AudioManager.instance.PlaySound("Watersound");
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trap"))
        {
            death = true;
            StartCoroutine(ReloadCurrentSceneAfterDelay(1f));
        }

        if (collider.gameObject.CompareTag("End"))
        {
            Door1 = true;
            StartCoroutine(LoadNextLevelAfterDelay(1f));
  
        }
    }


        IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的延迟时间

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // 确保关卡索引不超过已存在的场景数
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // 加载下一关卡
        }
        else
        {
            Debug.Log("No more levels!"); // 如果没有更多关卡，输出提示
        }
    }

    

    IEnumerator ReloadCurrentSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的延迟时间
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName); // 重载当前场景
    }

    private void FixedUpdate()
    {
        if (death)
            return;
        if (Door1)
            return;
        GroundMovement();
        Jumps();
    }

    void GroundMovement(){
        float horizontalMove = Input.GetAxisRaw("Horizontal");
         Vector3 scale = transform.localScale; // 获取当前的缩放值
        rb.velocity = new Vector2(horizontalMove * playerState.Speed, rb.velocity.y);

        if(horizontalMove != 0)
        {
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(horizontalMove);
        }
        transform.localScale = scale;
    }

    void Jumps(){
        if (jumpPressed)
    {
        if (isGrounded || (jumpCount > 0 && isJump))
        {
            rb.velocity = new Vector2(rb.velocity.x, playerState.JumpForce);
            jumpCount--;
        }
        // 无论是否跳跃成功，都重置跳跃按键状态
        jumpPressed = false;
        AudioManager.instance.PlaySound("jumpsound");
    }

        isGrounded = IsOnGround();
    }

    bool IsOnGround()
    {
    RaycastHit2D hit1 = Physics2D.Raycast(new Vector3(transform.position.x - colliderOffset.x, transform.position.y - colliderOffset.y, transform.position.z), Vector2.down, groundLength, groundLayer);
    RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x + colliderOffset.x, transform.position.y - colliderOffset.y, transform.position.z), Vector2.down, groundLength, groundLayer);
    RaycastHit2D hit3 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - colliderOffset.y, transform.position.z), Vector2.down, groundLength, groundLayer);

    if (hit1 || hit2 || hit3)
    {
        return true;
    }

    return false;
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

 private void LoadPreviousLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = Mathf.Max(currentSceneIndex - 1, 0); // 确保索引不会小于0
        SceneManager.LoadScene(previousSceneIndex);
    }

    // 转移到下一关
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) // 确保索引不会超出已有的关卡数
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("已经是最后一关了！");
        }
    }

}