using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引入 UI 命名空间以使用 UI 组件

public class MenuController : MonoBehaviour
{
    public GameObject settingsPanel; // 在 Inspector 中分配
    public Slider volumeSlider; // 在 Inspector 中分配

    void Start()
    {
        settingsPanel.SetActive(false); // 开始时隐藏设置面板
        volumeSlider.onValueChanged.AddListener(SetVolume); // 添加滑块事件监听
    }

    public void StartGame()
    {
        // 加载游戏主场景
        SceneManager.LoadScene("Test1.1");
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.backgroundMusic.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    // 显示或隐藏设置面板
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
