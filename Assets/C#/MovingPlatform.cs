using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOlatformer : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 1.0f;
    private float startTime;
    private float journeyLength;
    private bool movingToB = true;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        if (movingToB)
        {
            transform.position = Vector3.Lerp(pointA.position, pointB.position, fractionOfJourney);
            if (transform.position == pointB.position)
            {
                movingToB = false;
                startTime = Time.time;  // Reset the time
            }
        }
        else
        {
            transform.position = Vector3.Lerp(pointB.position, pointA.position, fractionOfJourney);
            if (transform.position == pointA.position)
            {
                movingToB = true;
                startTime = Time.time;  // Reset the time
            }
        }
    }
}