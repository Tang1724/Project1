using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public string state = "FULL";

    public float CurrentMass { get { return currentMass; } }
    private float currentMass;
    private float speed;

    public float Speed { get { return speed; } }

    private PlayerMoveControl playermovecontrol;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Missing SpriteRenderer component!");
        }

        playermovecontrol = GetComponent<PlayerMoveControl>();
        if (playermovecontrol == null)
        {
            Debug.LogError("Missing PlayerMoveControl component!");
        }

        UpdateMass();
        UpdateColor();
        UpdateSpeed();
    }

    public void ChangeState()
    {
        if (state == "FULL")
        {
            state = "EMPTY";
            Debug.Log("State changed to: " + state);
            UpdateColor();
            UpdateMass();
            UpdateSpeed();
        }
        else
        {
            Debug.Log("State is already EMPTY, cannot change back to FULL.");
        }
    }

    public void ChangeState(string newState)
    {
        if (newState == "FULL" && state != "FULL")
        {
            Debug.Log("State changed back to FULL.");
        }
        state = newState;
        UpdateColor();
        UpdateMass();
        UpdateSpeed();
    }

    void UpdateColor()
    {
        if (state == "FULL")
        {
            spriteRenderer.color = Color.blue;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    void UpdateSpeed()
    {
        if (state == "FULL")
        {
            speed = playermovecontrol.fullspeed;
        }
        else
        {
            speed = playermovecontrol.emptyspeed;
        }
    }

    void UpdateMass()
    {
        currentMass = (state == "FULL") ? 5f : 1f;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bottle")
        {
            if (state == "FULL")
            {
                PushObject(collision.gameObject);
            }
            else
            {
                Debug.Log("Player is not strong enough to push the bottle.");
            }
        }
    }

    void PushObject(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(new Vector2(1, 0) * 100); // 推动力的方向和大小
        }
    }
}