using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        Full,
        Empty,
        Fly
    }


    public State currentState = State.Full;
    private SpriteRenderer spriteRenderer;
    private PlayerMoveControl playerMoveControl;
    public event Action OnStateChange;

    [SerializeField]
    private GameObject iceBlockPrefab;
    public float spawnDistance = 0.2f;
    public bool IceBlock = false;
    public float cooldown = 0.75f; // 3秒的冷却时间
    private float lastEmptyTime; // 记录最后一次转换到EMPTY状态的时间

    public float flydown = 1f;
    private float lastFlyTime;
    public bool isWithinWater; // 是否处于water1标签内
    public float CurrentMass => currentState == State.Full ? 5f :
                                currentState == State.Empty ? 1f :
                                                             0f;
    public float Speed => currentState == State.Full ? playerMoveControl.fullspeed :
                          currentState == State.Empty ? playerMoveControl.emptyspeed :
                                                       playerMoveControl.flyspeed;


    public float JumpForce => currentState == State.Full ? playerMoveControl.jumpFullForce :
                              currentState == State.Empty ? playerMoveControl.jumpEmptyForce :
                                                            playerMoveControl.FlyForce;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMoveControl = GetComponent<PlayerMoveControl>();

        if (spriteRenderer == null)
        {
            Debug.LogError("[PlayerState] Missing SpriteRenderer component!");
        }

        if (playerMoveControl == null)
        {
            Debug.LogError("[PlayerState] Missing PlayerMoveControl component!");
        }

        // UpdateStateProperties();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentState == State.Full && isWithinWater)
        {
            ChangeState(State.Empty);
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateChange?.Invoke();
            Debug.Log($"[PlayerState] State changed to: {currentState}");
            // UpdateStateProperties();
            if (newState == State.Empty)
            {
                lastEmptyTime = Time.time; // 记录转换到EMPTY的时间
            }
             if (newState == State.Fly)
            {
                lastFlyTime = Time.time; // 记录转换到EMPTY的时间
            }
        }
        else
        {
            Debug.Log($"[PlayerState] State is already {currentState}, no change performed.");
        }
    }

    public  void CreateIceBlock(){
        Vector3 forwardDirection = transform.localScale.x > 0 ? transform.right : -transform.right;

        // 根据朝向计算生成位置
        Vector3 spawnPosition = transform.position + forwardDirection * spawnDistance;

        // 使用计算后的位置生成冰块
        Instantiate(iceBlockPrefab, spawnPosition, Quaternion.identity);
    }
    void OnTriggerStay2D(Collider2D collider)
    {   
            if (collider.gameObject.tag == "Water")
            {
                Debug.Log("[PlayerState] Touched water. Attempting to change state to Full.");
                ChangeState(State.Full);
                playerMoveControl.StartFly = false;

            }
            
            if (collider.gameObject.tag == "Purified water"){
                ChangeState(State.Fly);

            }
            
            if(collider.gameObject.tag == "Empty"){
                Debug.Log("[PlayerState] Touched water. Attempting to change state to Full.");
                ChangeState(State.Empty);
                playerMoveControl.StartFly = false;
                AudioManager.instance.PlaySound("Pushsound");
            }

            if(collider.gameObject.tag == "Cold"){
                IceBlock = true;

            }

            if (collider.gameObject.tag == "water1")
            {
                isWithinWater = true; // 进入水区
                if (currentState == State.Empty && Time.time >= lastEmptyTime + cooldown)
                {
                    ChangeState(State.Full); 
                }

            if (currentState == State.Fly)
            {
                if (playerMoveControl.StartFly == true)
                {
                    StartCoroutine(ExecuteAfterDelay());
                }
            }
            }
        }  
    

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cold"))
        {
            IceBlock = false;}

         if (other.gameObject.tag == "water1")
        {
            isWithinWater = false; // 离开水区
        }
        }

    private IEnumerator ChangeStateWithDelay(State newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }

    IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // 等待两秒
        ChangeState(State.Full); // 延迟两秒后执行的内容
        playerMoveControl.StartFly = false;
    }

}