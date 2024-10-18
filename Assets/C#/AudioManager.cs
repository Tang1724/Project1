using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] sounds; // 音效资源数组
    public AudioSource backgroundMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 确保音效管理器在加载新场景时不会被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlaySound(string name)
    {
        foreach (AudioSource sound in sounds)
        {
            if (sound.clip.name == name)
            {
                sound.Play();
                return;
            }
        }
        Debug.LogWarning("Sound name not found: " + name);
    }

    public void PauseMusic()
    {
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
        }
    }

    // 恢复背景音乐播放
    public void ResumeMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.UnPause();
        }
    }

        // 停止背景音乐
    public void StopMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
    }

    // 开始播放背景音乐
    public void StartMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

        // 设置背景音乐的音量
    public void SetMusicVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = Mathf.Clamp(volume, 0, 1); // 限制音量值在0到1之间
        }
    }

    // 设置所有音效的音量
    public void SetSoundVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1); // 限制音量值在0到1之间
        foreach (AudioSource sound in sounds)
        {
            if (sound != null)
            {
                sound.volume = volume;
            }
        }
    }

}
