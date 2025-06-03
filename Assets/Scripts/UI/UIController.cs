using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Utils;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class UIController : MonoBehaviour
{
    public CanvasGroup menu;

    public ApplicationUIController applicationUIController;
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

        // applicationUIController.backgroundRunningTypeDropdown.options
        CreateBackgroundRunningTypeDropdownOptions(applicationUIController.backgroundRunningTypeDropdown);
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

    private void CreateBackgroundRunningTypeDropdownOptions(TMP_Dropdown dropdown)
    {
        var type = typeof(BackgroundRunningType);
        var names = System.Enum.GetNames(type);
        // var values = System.Enum.GetValues(type);

        for (int i = 0; i < names.Length; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(names[i]));
        }
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

    public void TargetFrameRateSliderValueChanged(BaseEventData data)
    {
        // applicationUIController.targetFrameRateInputField.text = value.ToString();
        // applicationUIController.applicationSetting.SetTargetFrameRate(value);
        Debug.Log(applicationUIController.targetFrameRateSlider.value);

        PlayerSettingPref.Instance.ApplicationSettings.TargetFrameRate =
            (int)applicationUIController.targetFrameRateSlider.value;
    }

    public void TargetFrameRateInputFieldValueChanged(string value)
    {
        int targetFrameRate = Int32.Parse(value);
        applicationUIController.targetFrameRateSlider.value = targetFrameRate;

        PlayerSettingPref.Instance.ApplicationSettings.TargetFrameRate = targetFrameRate;
    }

    public void TargetFrameRateInputFieldEndEdit(string value)
    {
        int targetFrameRate = Int32.Parse(value);
        applicationUIController.targetFrameRateSlider.value = targetFrameRate;
        applicationUIController.applicationSetting.SetTargetFrameRate(targetFrameRate);

        PlayerSettingPref.Instance.ApplicationSettings.TargetFrameRate = targetFrameRate;
    }

    public void BackgroundRunningTypeDropdownValueChanged(int value)
    {
        // applicationUIController.applicationSetting.SetBackgroundRunningType(value);        
        var type = (BackgroundRunningType)value;
        applicationUIController.applicationSetting.SetBackgroundRunningType(type);
        
        PlayerSettingPref.Instance.ApplicationSettings.BackgroundRunningType = type;
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

    public float MenuCurrentTransparency
    {
        get => _menuCurrentTransparency;
        set => _menuCurrentTransparency = value;
    }
}

[Serializable]
public class ConstraintsUIController
{
    public Slider horizontalConstraintSlider;
    public Slider verticalConstraintSlider;
}

[Serializable]
public class ApplicationUIController
{
    public ApplicationSetting applicationSetting;

    public Slider targetFrameRateSlider;
    public TMP_InputField targetFrameRateInputField;
    public TMP_Dropdown backgroundRunningTypeDropdown;
}