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

    public BackgroundController backgroundController;
    public AudioController audioController;
    public MenuTransparencyController menuTransparencyController;

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
        volume = audioController.volumeSlider.value;
        muted = audioController.muteToggle.isOn;
    }

    public TweenerCore<float, float, FloatOptions> ToggleMenu(bool show)
    {
        return menu.DOFade(show ? menuTransparencyController.MenuCurrentTransparency : 0,
            audioController.muteToggleDduration);
    }

    public void ToggleReverse(bool reverse)
    {
        backgroundController.bgController.SetReversed(reverse);
        PlayerSettingPref.Instance.Reverse = reverse;
    }

    public void VolumeSliderValueChanged(float value)
    {
        audioController.audioSource.volume = value;
        PlayerSettingPref.Instance.Volume = value;
    }

    public void ToggleMute(bool mute)
    {
        audioController.audioSource.mute = mute;
        PlayerSettingPref.Instance.Muted = mute;
    }

    public void SetTransparency(float transparency)
    {
        menu.alpha = menuTransparencyController.MenuCurrentTransparency = transparency;
        PlayerSettingPref.Instance.MenuTransparency = transparency;
    }

    public void SetHorizontalConstraint(float value)
    {
        backgroundController.bgController.SetHorizontalConstraint(value);
        PlayerSettingPref.Instance.XConstraint = value;
    }

    public void SetVerticalConstraint(float value)
    {
        backgroundController.bgController.SetVerticalConstraint(value);
        PlayerSettingPref.Instance.YConstraint = value;
    }

    /*
     * TODO: Initialize UI and game data from settings
     */
    public void InitFromSettings()
    {
        
    }
}

[Serializable]
public class BackgroundController
{
    public BGController bgController;
    public Toggle reverseToggle;
    public ConstraintsController constraintsController;
}

[Serializable]
public class AudioController
{
    public AudioSource audioSource;
    public Slider volumeSlider;

    public Toggle muteToggle;
    public float muteToggleDduration = 0.5f;
}

[Serializable]
public class MenuTransparencyController
{
    public Slider transparencySlider;
    private float _menuCurrentTransparency = 1f;
    public float MenuCurrentTransparency { get => _menuCurrentTransparency; set => _menuCurrentTransparency = value; }
}

[Serializable]
public class ConstraintsController
{
    public Slider horizontalConstraintSlider;
    public Slider verticalConstraintSlider;
}