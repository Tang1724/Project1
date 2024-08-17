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
    public float spawnDistance = 0.5f;
    public bool IceBlock = false;
    public bool Cap = false;
    public bool CapChange = false;

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

        UpdateStateProperties();
    }

    public void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateChange?.Invoke();
            Debug.Log($"[PlayerState] State changed to: {currentState}");
            UpdateStateProperties();
        }
        else
        {
            Debug.Log($"[PlayerState] State is already {currentState}, no change performed.");
        }
    }

    private void UpdateStateProperties()
    {
        UpdateColor();
        // Since mass, speed, and force properties are computed, no need to call update methods
    }

    private void UpdateColor()
    {
        spriteRenderer.color = currentState == State.Full ? Color.blue :
                               currentState == State.Empty ? Color.white:
                               currentState == State.Fly ? Color.red:
                               Color.black;
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
            }

            if(collider.gameObject.tag == "Cold"){
                IceBlock = true;

            }
        }  
    

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cold"))
        {
            IceBlock = false;}
        }
}