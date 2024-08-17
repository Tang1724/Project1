using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float currentMass = 1f; // 你可以根据需要使用这个变量

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞的对象是否具有标签 "Empty"
        if (collision.gameObject.tag == "Empty")
        {
            StartCoroutine(DisableAndDestroy()); // 启动协程处理延迟销毁
        }
    }

    IEnumerator DisableAndDestroy()
    {
        // GetComponent<Collider2D>().enabled = false; // 禁用碰撞器
        yield return new WaitForSeconds(3); // 等待三秒
        Destroy(gameObject); // 销毁对象
    }
}