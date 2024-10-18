using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.backgroundMusic.UnPause();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 恢复正常时间流速
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.backgroundMusic.Pause();
    }

    public void RestartGame()
    {
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume(); // 确保时间恢复正常
    }

    public void LoadMainMenu()
    {
        // 加载主菜单场景，假设主菜单的场景名为 "MainMenu"
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.backgroundMusic.Stop();
        SceneManager.LoadScene("MainScene");

        Resume(); // 确保时间恢复正常
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
