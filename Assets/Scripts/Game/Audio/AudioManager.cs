using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework;
using MFramework.Event;

public class AudioManager : Singleton<AudioManager>, IEventReceiver<SoundRequestedEvent>, IEventReceiver<GameModeChangedEvent>
{
    public static AudioManager instance => Instance;

    public AudioSource[] sounds; // 音效资源数组
    public AudioSource backgroundMusic;
    public float MusicVolume => backgroundMusic != null ? backgroundMusic.volume : 0f;
    public float SoundVolume => sounds.Length > 0 && sounds[0] != null ? sounds[0].volume : 0f;

    public static void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
            DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (Instance != this)
            return;
        EventBus.Subscribe<SoundRequestedEvent>(this);
        EventBus.Subscribe<GameModeChangedEvent>(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<SoundRequestedEvent>(this);
        EventBus.Unsubscribe<GameModeChangedEvent>(this);
    }

    public void OnEvent(SoundRequestedEvent evt)
    {
        PlaySound(evt.Name);
    }

    public void OnEvent(GameModeChangedEvent evt)
    {
        if (evt.Mode == GameMode.Pause)
            PauseMusic();
        else
            ResumeMusic();
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
