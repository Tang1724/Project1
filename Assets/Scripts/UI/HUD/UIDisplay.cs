using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplay : MonoBehaviour
{
     public GameObject uiElement; // 拖拽你的UI元素到这个变量上

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 确保你的玩家角色有一个标签叫"Player"
        {
            uiElement.SetActive(true); // 显示UI元素
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uiElement.SetActive(false); // 当玩家离开触发区域时隐藏UI元素
        }
    }

}
