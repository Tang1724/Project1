using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
   public Text levelText;
    void Start()
    {
        UpdateLevelUI();
    }

    void UpdateLevelUI()
    {
        // 获取当前场景的名称并更新UI
        string sceneName = LevelFlow.CurrentLevelName;
        levelText.text = sceneName;
    }
}
