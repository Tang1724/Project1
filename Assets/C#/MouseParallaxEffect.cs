using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseParallaxEffect : MonoBehaviour
{
    public float parallaxEffectMultiplier;
    private Vector3 startPosition;
    private float startZ;

    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z; // Keep the original Z position constant
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToViewportPoint(mousePos); // Convert mouse position to viewport space
        Vector3 moveAmount = new Vector3((mousePos.x - 0.5f) * parallaxEffectMultiplier, (mousePos.y - 0.5f) * parallaxEffectMultiplier, 0);
        
        transform.position = new Vector3(startPosition.x + moveAmount.x, startPosition.y + moveAmount.y, startZ);
    }
}