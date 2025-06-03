using System;
using DefaultNamespace.Utils;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public CanvasGroup menu;

    public BGUIController bgUIController;
    public AudioUIController audioUIController;
    public MenuTransparencyUIController menuTransparencyUIController;

    public static float volume;
    public static bool muted;

    private static UIController _instance;

    public static UIController Instance => _instance;

    /*
     * TODO: Initiliaze the UI with data instead of componets
     */
    private void Start()
    {
        // volumeSlider.value = volume = audioSource.volume;
        // muteToggle.isOn = muted = audioSource.mute;
        _instance = this;
    }

    private void Update()
    {
        //update volume and mute state for audioTester
        volume = audioUIController.volumeSlider.value;
        muted = audioUIController.muteToggle.isOn;
    }

    public TweenerCore<float, float, FloatOptions> ToggleMenu(bool show)
    {
        return menu.DOFade(show ? menuTransparencyUIController.MenuCurrentTransparency : 0,
            audioUIController.muteToggleDduration);
    }

    public void ToggleReverse(bool reverse)
    {
        bgUIController.bgController.SetReversed(reverse);
        PlayerSettingPref.Instance.BGControllerSettings.Reverse = reverse;
    }

    public void ParallaxScaleSliderValueChanged(float value)
    {
        bgUIController.bgController.SetParallaxScale(value);
        PlayerSettingPref.Instance.BGControllerSettings.ParallaxScale = value;
    }

    public void VolumeSliderValueChanged(float value)
    {
        audioUIController.audioSource.volume = value;
        PlayerSettingPref.Instance.OtherSettings.Volume = value;
    }

    public void ToggleMute(bool mute)
    {
        audioUIController.audioSource.mute = mute;
        PlayerSettingPref.Instance.OtherSettings.Muted = mute;
    }

    public void SetTransparency(float transparency)
    {
        menu.alpha = menuTransparencyUIController.MenuCurrentTransparency = transparency;
        PlayerSettingPref.Instance.OtherSettings.MenuTransparency = transparency;
    }

    public void SetHorizontalConstraint(float value)
    {
        bgUIController.bgController.SetHorizontalConstraint(value);
        PlayerSettingPref.Instance.BGControllerSettings.XConstraint = value;
    }

    public void SetVerticalConstraint(float value)
    {
        bgUIController.bgController.SetVerticalConstraint(value);
        PlayerSettingPref.Instance.BGControllerSettings.YConstraint = value;
    }

    /*
     * TODO: Initialize UI and game data from settings
     */
    public void InitFromSettings()
    {
        
    }
    
    /*
     * TODO:
     */
    public void GetMetaSettings()
    {
        
    }
}

[Serializable]
public class BGUIController
{
    public BGController bgController;
    public Toggle reverseToggle;
    public Slider parallaxScaleSlider;
    public ConstraintsUIController constraintsUIController;
}

[Serializable]
public class AudioUIController
{
    public AudioSource audioSource;
    public Slider volumeSlider;

    public Toggle muteToggle;
    public float muteToggleDduration = 0.5f;
}

[Serializable]
public class MenuTransparencyUIController
{
    public Slider transparencySlider;
    private float _menuCurrentTransparency = 1f;
    public float MenuCurrentTransparency { get => _menuCurrentTransparency; set => _menuCurrentTransparency = value; }
}

[Serializable]
public class ConstraintsUIController
{
    public Slider horizontalConstraintSlider;
    public Slider verticalConstraintSlider;
}