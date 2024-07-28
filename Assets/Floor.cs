using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Transform targetPosition; // PosB 的位置
    public Transform startPosition;  // PosA 的位置，平台的初始位置

    public float Mass = 8.0f;        // 触发平台移动的最小质量
    public float speed = 0.5f;       // 平台移动的速度

    private HashSet<GameObject> trackedObjects = new HashSet<GameObject>(); // 使用 HashSet 防止重复

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Bottle"))
        {
            // 只有当对象不在 trackedObjects 中时才添加
            trackedObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Bottle"))
        {
            trackedObjects.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        float totalMass = 0f;
        foreach (var obj in trackedObjects)
        {
            var playerState = obj.GetComponent<PlayerState>();
            var bottle = obj.GetComponent<Bottle>();
            if (playerState != null)
            {
                totalMass += playerState.CurrentMass;
            }
            if (bottle != null)
            {
                totalMass += bottle.CurrentMass;
            }
            Debug.Log(+ totalMass);
        }

        if (totalMass >= Mass)
        {
            // 平滑移动平台到目标位置B
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
        }
        else
        {
            // 平滑移动平台回到初始位置A
            transform.position = Vector2.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
        }
    }
}
