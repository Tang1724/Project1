using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider musicVolumeSlider; // 对应背景音乐滑块
    public Slider soundEffectsVolumeSlider; // 对应音效滑块

    void Start()
    {
        // 初始化滑块位置
        musicVolumeSlider.value = AudioManager.instance.backgroundMusic.volume;
        soundEffectsVolumeSlider.value = AudioManager.instance.sounds[0].volume; // 假设所有音效音量相同，以第一个为准
        
        // 添加滑块事件监听器
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        soundEffectsVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    void SetMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }

    void SetSoundVolume(float volume)
    {
        AudioManager.instance.SetSoundVolume(volume);
    }
}