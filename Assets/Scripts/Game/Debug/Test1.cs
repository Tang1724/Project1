using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{

    void Start(){

        Debug.Log("Drawing Gizmos for object: " + gameObject.name);
        Debug.Log("Object Position: " + transform.position);
        Debug.Log("Gizmos Color: " + Gizmos.color);

    }
    void OnDrawGizmos()
    {
        // 设置 Gizmo 颜色为青色
        Gizmos.color = Color.cyan;

        // 在 GameObject 的位置绘制一个小球形 Gizmo
        Gizmos.DrawSphere(transform.position, 10f);

        // 从 GameObject 的位置向右绘制一条长度为 1 的线
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right);

        // 输出调试信息到控制台
        Debug.Log("Drawing Gizmos for object: " + gameObject.name);
        Debug.Log("Object Position: " + transform.position);
        Debug.Log("Gizmos Color: " + Gizmos.color);
    }
}
