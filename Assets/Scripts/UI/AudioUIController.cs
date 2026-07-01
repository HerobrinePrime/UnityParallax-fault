using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class AudioUIController : MonoBehaviour
{
    private static AudioUIController _instance;

    public static AudioUIController Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new Exception("TimeUIController instance is not set!");
            }

            return _instance;
        }
    }

    public static float volume;
    public static bool muted;

    public AudioSystemController AudioSystemController;

    public AudioSource audioSource;
    public Slider volumeSlider;

    public Toggle muteToggle;
    public float muteToggleDduration = 0.5f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            throw new Exception("AudioUIController instance already exists!");
        }

        _instance = this;

        InitFromSettings();
    }


    private void Update()
    {
        //update volume and mute state for audioTester
        volume = volumeSlider.value;
        muted = muteToggle.isOn;
    }

    public void InitFromSettings()
    {
        var audioSettings = PlayerSettingPref.Instance.AudioSettings;
        audioSource.volume = audioSettings.Volume;
       audioSource.mute = audioSettings.Muted;
        
        volumeSlider.SetValueWithoutNotify(audioSettings.Volume);
        muteToggle.SetIsOnWithoutNotify(audioSettings.Muted);
    }
    
    public void VolumeSliderValueChanged(float value)
    {
        audioSource.volume = value;

        // PlayerSettingPref.Instance.OtherSettings.Volume = value;
        PlayerSettingPref.Instance.AudioSettings.Volume = value;
    }
    
    public void ToggleMute(bool mute)
    {
        audioSource.mute = mute;

        // PlayerSettingPref.Instance.OtherSettings.Muted = mute;
        PlayerSettingPref.Instance.AudioSettings.Muted = mute;
    }
    
    [Obsolete]
    public AudioUIController GetMetaSettings()
    {
        return  null;
    }
}