using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{

    public Text massDisplayText;
    public Transform targetPosition; // 目标位置 PosB
    public Transform startPosition;  // 初始位置 PosA，平台的初始位置
    public float Mass = 8.0f;        // 触发平台移动的最小质量
    public float speed = 0.5f;       // 平台移动速度
    private HashSet<GameObject> trackedObjects = new HashSet<GameObject>(); // 使用 HashSet 防止重复
    public float totalMass = 0f;     // 当前平台上的总质量
    public bool OnTop = false;

    void Start()
    {
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {   
        // CheckPosition(collision);
        collision.collider.transform.SetParent(transform);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckPosition(collision);
        if (!trackedObjects.Contains(collision.gameObject))
        {
            trackedObjects.Add(collision.gameObject);
            RecalculateMass(); // 重新计算质量
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Bottle") || collision.collider.CompareTag("ICE"))
        {
            if (trackedObjects.Contains(collision.gameObject))
            {
                trackedObjects.Remove(collision.gameObject);
                RecalculateMass(); // 重新计算质量
            }
            OnTop = false; // 当玩家离开平台时标志设为假

            if (collision.collider.transform.parent == transform)
            {
                // 解除玩家的父子关系
                collision.collider.transform.SetParent(null);
            }
        }
    }

void CheckPosition(Collision2D collision)
{
    if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Bottle") || collision.collider.CompareTag("ICE"))
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // 计算平台的顶部
            float platformTop = transform.position.y + (transform.localScale.y * 0.5f);
            // 计算平台的左右边界
            float platformLeft = transform.position.x - (transform.localScale.x * 0.47f);
            float platformRight = transform.position.x + (transform.localScale.x * 0.47f);

            // 检查碰撞点是否在平台的顶部，并且在左右边界之内
            if (contact.point.y >= platformTop && contact.point.x >= platformLeft && contact.point.x <= platformRight)
            {
                OnTop = true;
                break; // 找到一个满足条件的点后即可退出循环
            }
        }
    }
}

    private void Update()
    {
        Vector2 targetPos;

        // 检查是否满足平台移动的条件
        if (OnTop && totalMass >= Mass)
        {
            // 设置目标位置为 targetPosition
            targetPos = targetPosition.position;
        }
        else
        {
            // 设置目标位置为 startPosition
            targetPos = startPosition.position;
        }

        // 计算从当前位置到目标位置的方向
        Vector2 moveDirection = (targetPos - (Vector2)transform.position).normalized;

        // 计算这一帧应当移动的距离
        float step = speed * Time.deltaTime;

        // 移动平台
        transform.position += (Vector3)(moveDirection * step);

        // 检查是否超过目标位置
        if (Vector2.Distance(transform.position, targetPos) < step)
        {
            transform.position = targetPos;
        }

        // 重新计算质量
        RecalculateMass();
    }

    public void RecalculateMass()
    {
        totalMass = 0f;
        foreach (var obj in trackedObjects)
        {
            if (obj.transform.parent == null || !trackedObjects.Contains(obj.transform.parent.gameObject))
            {
                totalMass += CalculateTotalMass(obj);
            }
        }
        Debug.Log($"Total Mass: {totalMass}");
    }

    private float CalculateTotalMass(GameObject obj)
    {
        float mass = 0f;

        // 检查 PlayerState 组件
        PlayerState playerState = obj.GetComponent<PlayerState>();
        if (playerState != null)
        {
            mass += playerState.CurrentMass;
        }

        // 检查 Bottle 组件
        Bottle bottle = obj.GetComponent<Bottle>();
        if (bottle != null)
        {
            mass += bottle.CurrentMass;
        }

        Ice ice = obj.GetComponent<Ice>();
        if(ice != null)
        {
            mass += ice.currentMass;
        }

        // 递归地检查所有子物体
        foreach (Transform child in obj.transform)
        {
            mass += CalculateTotalMass(child.gameObject);  // 递归调用自身
        }

        return mass;
    }

    void OnDrawGizmos()
    {
        // 设置 Gizmos 的颜色为半透明的蓝色
        Gizmos.color = new Color(0, 0, 1, 0.5f);

        // 计算平台的顶部和左右边界
        float platformTop = transform.position.y + (transform.localScale.y * 0.5f);
        float platformLeft = transform.position.x - (transform.localScale.x * 0.47f);
        float platformRight = transform.position.x + (transform.localScale.x * 0.47f);

        // 计算平台顶部的宽度
        float width = platformRight - platformLeft;
        
        // 绘制顶部的碰撞区域
        Gizmos.DrawCube(new Vector3(transform.position.x, platformTop, 0), new Vector3(width, 0.1f, 1));
    }
}