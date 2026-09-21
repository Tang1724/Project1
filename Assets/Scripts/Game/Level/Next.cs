using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next : MonoBehaviour
{


        void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) // 确认触碰对象是玩家
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        LevelFlow.LoadNextLevel();
    }
}
