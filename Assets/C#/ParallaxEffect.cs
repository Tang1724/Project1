using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform characterTransform; // Reference to the character's Transform
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
        Vector3 characterViewportPos = Camera.main.WorldToViewportPoint(characterTransform.position);
        Vector3 moveAmount = new Vector3((characterViewportPos.x - 0.5f) * parallaxEffectMultiplier, (characterViewportPos.y - 0.5f) * parallaxEffectMultiplier, 0);
        
        transform.position = new Vector3(startPosition.x + moveAmount.x, startPosition.y + moveAmount.y, startZ);
    }
}
