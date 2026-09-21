using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间以使用 UI 组件

public class MenuController : MonoBehaviour
{
    public GameObject settingsPanel; // 在 Inspector 中分配
    public Slider volumeSlider; // 在 Inspector 中分配

    void Start()
    {
        settingsPanel.SetActive(false); // 开始时隐藏设置面板
    }

    private void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }

    public void StartGame()
    {
        // 加载游戏主场景
        LevelFlow.StartGame();
    }

    public void QuitGame()
    {
        LevelFlow.QuitGame();
    }

    public void SetVolume(float volume)
    {
        AudioManager.SetMasterVolume(volume);
    }

    // 显示或隐藏设置面板
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

}
