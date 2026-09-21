using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞的对象是否具有标签 "EMPTY"
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
            // 如果是，销毁这个冰块对象
            Destroy(gameObject);
        }
    }

}
