using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
   public Text levelText;  // 如果你使用的是普通的UI Text
    // public TextMeshProUGUI levelText; // 如果你使用的是TextMeshPro

    void Start()
    {
        UpdateLevelUI();
    }

    void UpdateLevelUI()
    {
        // 获取当前场景的名称并更新UI
        string sceneName = SceneManager.GetActiveScene().name;
        levelText.text = sceneName;
    }
}
