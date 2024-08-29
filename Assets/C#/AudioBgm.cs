using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBgm : MonoBehaviour
{
// 使用单例模式确保只有一个实例
    public static AudioBgm instance;

    void Awake()
    {
        // 检查是否已经有一个实例存在
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 防止在加载新场景时被销毁
        }
        else
        {
            Destroy(gameObject); // 如果已存在实例，销毁新对象
        }
    }
}
