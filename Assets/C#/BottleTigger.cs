using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleTigger : MonoBehaviour
{
    private Bottle bottle;

    void Start()
    {
        bottle = GetComponentInParent<Bottle>();
        if (bottle == null)
        {
            Debug.LogError("BottleTriggerHandler requires a Bottle component in the parent.");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        bottle.HandleTriggerEnter(collider);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        bottle.HandleTriggerStay(collider);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        bottle.HandleTriggerExit(collider);
    }
}