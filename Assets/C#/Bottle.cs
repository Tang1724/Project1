// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Bottle : MonoBehaviour
// {    
//     public enum BottleState
//     {
//         Empty,
//         Full,
//         Fly
//     }

//     private Rigidbody2D rb;
//     private PlayerState.State previousState;
//     public BottleTigger bottleTigger;
//     private Collider2D coll;
//     private SpriteRenderer spriteRenderer;
//     public bool PlayerinBottle = false;
//     public BottleState state = BottleState.Empty;
//     public PlayerState playerState;
//     public float BottleFlyForce = 5f;

//     public float CurrentMass { get { return currentMass; } }
//     private float currentMass;
//     public float totalMass = 0f;
//     public PhysicsMaterial2D BottleEmpty;
//     public PhysicsMaterial2D BottleFull;

//     void Start()
//     {
//         playerState = GetComponent<PlayerState>();
//         if (playerState == null)
//         {
//             playerState = FindObjectOfType<PlayerState>();
//             if (playerState == null)
//             {
//                 Debug.LogError("No PlayerState component found in the scene.");
//                 return;
//             }
//         }

//         spriteRenderer = GetComponent<SpriteRenderer>();
//         rb = GetComponent<Rigidbody2D>();
//         coll = GetComponent<Collider2D>();
//         bottleTigger = GetComponent<BottleTigger>();

//         if (spriteRenderer == null || rb == null || coll == null)
//         {
//             Debug.LogError("Bottle component missing an essential component: SpriteRenderer, Rigidbody2D, or Collider2D.");
//             return;
//         }

//         playerState.OnStateChange += HandlePlayerStateChange;
//         UpdatePhysicsProperties();
//         UpdateColor();
//     }

//     void Update()
//     {
//         if (state == BottleState.Fly)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, BottleFlyForce);
//         }
//     }

//     private void HandlePlayerStateChange()
//     {
//         ChangeState();
//     }
    
//     public void UpdateColor()
//     {
//         spriteRenderer.color = state == BottleState.Full ? Color.green :
//                                 state == BottleState.Empty ? Color.white :
//                                                             Color.yellow;
//     }

//     public void UpdatePhysicsProperties()
//     {
//         if (state == BottleState.Full)
//         {
//             currentMass = 5f;
//         }
//         if (state == BottleState.Empty)
//         {
//             currentMass = 1f;
//         }
//         if (state == BottleState.Fly)
//         {
//             currentMass = 0f;
//         }
//     }

    // private void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Bottle")
    //     {
    //         Vector3 globalPosition = collider.transform.position;
    //         Quaternion globalRotation = collider.transform.rotation;

    //         collider.transform.SetParent(transform);
    //         collider.transform.position = globalPosition;
    //         collider.transform.rotation = globalRotation;
    //     }
    // }


    // void OnTriggerStay2D(Collider2D collider)
    // {
    //     if (collider.gameObject.tag == "Player")
    //     {
    //         PlayerinBottle = true;
    //     }

    //     if(playerState.currentState == PlayerState.State.Full || playerState.currentState == PlayerState.State.Fly){
    //         coll.sharedMaterial = BottleFull;
    //     }else{
    //         coll.sharedMaterial = BottleEmpty;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collider)
    // {
    //     if (collider.gameObject.tag == "Player")
    //     {
    //         PlayerinBottle = false;
    //     }

    //     if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Bottle")
    //     {
    //         collider.transform.SetParent(null);
    //     }
    // }

//     public void ChangeState()
//     {
//         if (PlayerinBottle)
//         {
//             if (playerState.currentState == PlayerState.State.Empty)
//             {
//                 switch (previousState)
//                 {
//                     case PlayerState.State.Full:
//                         state = BottleState.Full;
//                         UpdatePhysicsProperties();
//                         UpdateColor();
//                         break;

//                     case PlayerState.State.Fly:
//                         state = BottleState.Fly;
//                         UpdatePhysicsProperties();
//                         UpdateColor();
//                         break;
//                 }
//             }
//         }
//         previousState = playerState.currentState;
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{    
    public enum BottleState
    {
        Empty,
        Full,
        Fly
    }

    private Rigidbody2D rb;
    private PlayerState.State previousState;
    public BottleTigger bottleTigger;
    private Collider2D coll;
    private SpriteRenderer spriteRenderer;
    public bool PlayerinBottle = false;
    public BottleState state = BottleState.Empty;
    public PlayerState playerState;
    public float BottleFlyForce = 5f;

    public float CurrentMass { get { return currentMass; } }
    private float currentMass;
    public float totalMass = 0f;
    public PhysicsMaterial2D BottleEmpty;
    public PhysicsMaterial2D BottleFull;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        if (playerState == null)
        {
            playerState = FindObjectOfType<PlayerState>();
            if (playerState == null)
            {
                Debug.LogError("No PlayerState component found in the scene.");
                return;
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        bottleTigger = GetComponent<BottleTigger>();

        if (spriteRenderer == null || rb == null || coll == null)
        {
            Debug.LogError("Bottle component missing an essential component: SpriteRenderer, Rigidbody2D, or Collider2D.");
            return;
        }

        playerState.OnStateChange += HandlePlayerStateChange;
        UpdatePhysicsProperties();
        UpdateColor();
    }

    void Update()
    {
        if (state == BottleState.Fly)
        {
            rb.velocity = new Vector2(rb.velocity.x, BottleFlyForce);
        }
    }

    private void HandlePlayerStateChange()
    {
        ChangeState();
    }
    
    public void UpdateColor()
    {
        spriteRenderer.color = state == BottleState.Full ? Color.green :
                                state == BottleState.Empty ? Color.white :
                                                            Color.yellow;
    }

    public void UpdatePhysicsProperties()
    {
        if (state == BottleState.Full)
        {
            currentMass = 5f;
        }
        if (state == BottleState.Empty)
        {
            currentMass = 1f;
        }
        if (state == BottleState.Fly)
        {
            currentMass = 0f;
        }
    }

    public void ChangeState()
    {
        if (PlayerinBottle)
        {
            if (playerState.currentState == PlayerState.State.Empty)
            {
                switch (previousState)
                {
                    case PlayerState.State.Full:
                        state = BottleState.Full;
                        UpdatePhysicsProperties();
                        UpdateColor();
                        break;

                    case PlayerState.State.Fly:
                        state = BottleState.Fly;
                        UpdatePhysicsProperties();
                        UpdateColor();
                        break;
                }
            }
        }
        previousState = playerState.currentState;
    }

    // Trigger handling methods for BottleTriggerHandler to call
    public void HandleTriggerEnter(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Bottle")
        {
            Vector3 globalPosition = collider.transform.position;
            Quaternion globalRotation = collider.transform.rotation;

            collider.transform.SetParent(transform);
            collider.transform.position = globalPosition;
            collider.transform.rotation = globalRotation;
        }
    }

    public void HandleTriggerStay(Collider2D collider)
    {
         if (collider.gameObject.tag == "Player")
        {
            PlayerinBottle = true;
        }

        if(playerState.currentState == PlayerState.State.Full || playerState.currentState == PlayerState.State.Fly){
            coll.sharedMaterial = BottleFull;
        }else{
            coll.sharedMaterial = BottleEmpty;
        }
    }

    public void HandleTriggerExit(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerinBottle = false;
        }

        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Bottle")
        {
            collider.transform.SetParent(null);
        }
    }
}