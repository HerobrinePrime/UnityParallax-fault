using System;
using System.Collections.Generic;
using DefaultNamespace;
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
using Debug = UnityEngine.Debug;

public class UIController : MonoBehaviour
{
    public CanvasGroup menu;

    public ApplicationUIController applicationUIController;
    public BGUIController bgUIController;
    public AudioUIController audioUIController;
    public MenuTransparencyUIController menuTransparencyUIController;

    public static float volume;
    public static bool muted;

    public static UIController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new Exception("An instance of UIController already exists!");
        }

        Instance = this;
    }

    private void Start()
    {
        // volumeSlider.value = volume = audioSource.volume;
        // muteToggle.isOn = muted = audioSource.mute;

        // applicationUIController.backgroundRunningTypeDropdown.options
        CreateBackgroundRunningTypeDropdownOptions(applicationUIController.backgroundRunningTypeDropdown);

        InitFromSettings();
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
        Debug.Log("Toggle reverse");
        bgUIController.bgController.SetReversed(reverse);
        PlayerSettingPref.Instance.BGControllerSettings.Reverse = reverse;
    }

    public void ParallaxScaleSliderValueChanged(BaseEventData data)
    {
        Debug.Log("Parallax Scale Slider Value Changed");
        var value = bgUIController.parallaxScaleSlider.value;
        bgUIController.bgController.SetParallaxScale(value);
        PlayerSettingPref.Instance.BGControllerSettings.ParallaxScale = value;
    }

    public void VolumeSliderValueChanged(float value)
    {
        Debug.Log("Volume Slider Value Changed");
        audioUIController.audioSource.volume = value;
        PlayerSettingPref.Instance.OtherSettings.Volume = value;
    }

    public void ToggleMute(bool mute)
    {
        Debug.Log("Toggle mute");
        audioUIController.audioSource.mute = mute;
        PlayerSettingPref.Instance.OtherSettings.Muted = mute;
    }

    // bool _isFirseTime = true;
    public void SetTransparency(float transparency)
    {
        // if (_isFirseTime)
        // {
        //     _isFirseTime = false;
        //     return;
        // }

        Debug.Log("Set transparency");
        menu.alpha = menuTransparencyUIController.MenuCurrentTransparency = transparency;
        PlayerSettingPref.Instance.OtherSettings.MenuTransparency = transparency;
    }

    public void SetHorizontalConstraint(float value)
    {
        Debug.Log("Set horizontal constraint");
        bgUIController.bgController.SetHorizontalConstraint(value);
        PlayerSettingPref.Instance.BGControllerSettings.XConstraint = value;
    }

    public void SetVerticalConstraint(float value)
    {
        Debug.Log("Set vertical constraint");
        bgUIController.bgController.SetVerticalConstraint(value);
        PlayerSettingPref.Instance.BGControllerSettings.YConstraint = value;
    }

    public void TargetFrameRateSliderValueChanged(float value)
    {
        applicationUIController.targetFrameRateInputField.text = value.ToString();
    }

    public void TargetFrameRateSliderValueChanged(BaseEventData data)
    {
        Debug.Log("TargetFrameRate Slider Value Changed");
        // applicationUIController.targetFrameRateInputField.text = value.ToString();
        // applicationUIController.applicationSetting.SetTargetFrameRate(value);
        // Debug.Log(applicationUIController.targetFrameRateSlider.value);
        var targetFrameRate = (int)applicationUIController.targetFrameRateSlider.value;
        applicationUIController.targetFrameRateInputField.text = targetFrameRate.ToString();
        applicationUIController.applicationSetting.SetTargetFrameRate(targetFrameRate);

        PlayerSettingPref.Instance.ApplicationSettings.TargetFrameRate = targetFrameRate;
    }

    public void TargetFrameRateInputFieldValueChanged(string value)
    {
        Debug.Log("TargetFrameRate Input Field Value Changed");
        int targetFrameRate = Int32.Parse(value);
        applicationUIController.targetFrameRateSlider.value = targetFrameRate;

        PlayerSettingPref.Instance.ApplicationSettings.TargetFrameRate = targetFrameRate;
    }

    public void TargetFrameRateInputFieldEndEdit(string value)
    {
        Debug.Log("TargetFrameRate Input Field End Edit");
        int targetFrameRate = Int32.Parse(value);
        applicationUIController.targetFrameRateSlider.value = targetFrameRate;
        applicationUIController.applicationSetting.SetTargetFrameRate(targetFrameRate);

        PlayerSettingPref.Instance.ApplicationSettings.TargetFrameRate = targetFrameRate;
    }

    public void BackgroundRunningTypeDropdownValueChanged(int value)
    {
        // applicationUIController.applicationSetting.SetBackgroundRunningType(value);        
        Debug.Log(value);
        var type = (BackgroundRunningType)value;
        Debug.Log(type);
        applicationUIController.applicationSetting.SetBackgroundRunningType(type);

        PlayerSettingPref.Instance.ApplicationSettings.BackgroundRunningType = type;
    }

    public void InitFromSettings()
    {
        //init other settings
        var otherSettings = PlayerSettingPref.Instance.OtherSettings;
        audioUIController.audioSource.volume = otherSettings.Volume;
        audioUIController.audioSource.mute = otherSettings.Muted;
        menuTransparencyUIController.MenuCurrentTransparency = otherSettings.MenuTransparency;

        var bgControllerSettings = PlayerSettingPref.Instance.BGControllerSettings;
        var applicationSettings = PlayerSettingPref.Instance.ApplicationSettings;

        //read ui data
        applicationUIController.targetFrameRateSlider.SetValueWithoutNotify(applicationSettings.TargetFrameRate);
        applicationUIController.targetFrameRateInputField.SetTextWithoutNotify(applicationSettings.TargetFrameRate.ToString());
        applicationUIController.backgroundRunningTypeDropdown.SetValueWithoutNotify((int)applicationSettings.BackgroundRunningType);
        applicationUIController.backgroundRunningTypeDropdown.RefreshShownValue();

        bgUIController.reverseToggle.SetIsOnWithoutNotify(bgControllerSettings.Reverse);
        bgUIController.parallaxScaleSlider.SetValueWithoutNotify(bgControllerSettings.ParallaxScale);

        audioUIController.volumeSlider.SetValueWithoutNotify(otherSettings.Volume);
        audioUIController.muteToggle.SetIsOnWithoutNotify(otherSettings.Muted);

        menuTransparencyUIController.transparencySlider.SetValueWithoutNotify(otherSettings.MenuTransparency);

        bgUIController.constraintsUIController.horizontalConstraintSlider.SetValueWithoutNotify(bgControllerSettings
            .XConstraint);
        bgUIController.constraintsUIController.verticalConstraintSlider.SetValueWithoutNotify(bgControllerSettings
            .YConstraint);
    }


    public OtherSettings GetMetaSettings()
    {
        return new OtherSettings(
            audioUIController.audioSource.volume,
            audioUIController.audioSource.mute,
            menuTransparencyUIController.MenuCurrentTransparency
        );
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