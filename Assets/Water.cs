using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public string state = "EMPTY";

    public float CurrentMass { get { return currentMass; } }
    private float currentMass;
    private bool prevStateWasFull = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Missing SpriteRenderer component!");
        }

        UpdateMass();
        UpdateColor();
    }

    public void ChangeState()
    {
        if (state == "EMPTY")
        {
            state = "FULL";
            Debug.Log("State changed to: " + state);
            UpdateColor();
            UpdateMass();
        }
        else
        {
            Debug.Log("State is already EMPTY, cannot change back to FULL.");
        }
    }

    public void ChangeState(string newState)
    {
        if (newState == "EMPTY" && state != "EMPTY")
        {
            Debug.Log("State changed back to EMPTY.");
        }
        state = newState;
        UpdateColor();
        UpdateMass();
    }

    void UpdateColor()
    {
        if (state == "EMPTY")
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.green;
        }
    }

    void UpdateMass()
    {
        currentMass = (state == "FULL") ? 5f : 1f;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        CheckAndChangeState(collider);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        CheckAndChangeState(collider);
        if (collider.gameObject.tag == "Water")
        {
            Debug.Log("Touched water. Attempting to change state to FULL.");
            ChangeState("FULL");
        }
        else
        {
            Debug.Log("Touched object not tagged as Water: " + collider.gameObject.tag);
        }
    }

    private void CheckAndChangeState(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerState player = collider.gameObject.GetComponent<PlayerState>();
            if (player != null)
            {
                bool currentPlayerStateIsFull = player.state == "FULL";
                
                // 如果玩家之前的状态是FULL，并且当前状态不是FULL
                if (prevStateWasFull && !currentPlayerStateIsFull && this.state == "EMPTY")
                {
                    Debug.Log("Player state changed from FULL to EMPTY. Changing Water state to FULL.");
                    ChangeState("FULL");
                }

                // 更新前一个状态记录
                prevStateWasFull = currentPlayerStateIsFull;
            }
        }
        else
        {
            Debug.Log("Touched object not tagged as Player: " + collider.gameObject.tag);
        }
    }

    
}