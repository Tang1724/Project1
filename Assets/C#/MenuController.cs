using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
     public void StartGame()
    {
        // 确保此处的"MainScene"是你游戏主场景的名称
        SceneManager.LoadScene("Test1.1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
