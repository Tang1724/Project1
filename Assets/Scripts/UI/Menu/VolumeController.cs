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
        musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        soundEffectsVolumeSlider.value = AudioManager.instance.SoundVolume; // 假设所有音效音量相同，以第一个为准
        

    }

    private void OnEnable()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        soundEffectsVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    private void OnDisable()
    {
        musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        soundEffectsVolumeSlider.onValueChanged.RemoveListener(SetSoundVolume);
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