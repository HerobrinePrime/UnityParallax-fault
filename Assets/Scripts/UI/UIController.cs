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

    #region Responsers

    public void ToggleReverse(bool reverse)
    {
        bgUIController.bgController.SetReversed(reverse);

        PlayerSettingPref.Instance.BGControllerSettings.Reverse = reverse;
    }

    public void ToggleUseParallax(bool useParallax)
    {
        bgUIController.bgController.SetUseParallax(useParallax);

        PlayerSettingPref.Instance.BGControllerSettings.UseParallax = useParallax;
    }

    public void ToggleUseFullBackground(bool useFullBackground)
    {
        bgUIController.bgController.SetUseFullBackground(useFullBackground);

        PlayerSettingPref.Instance.BGControllerSettings.UseFullBackground = useFullBackground;
    }

    public void ParallaxScaleSliderValueChanged(float value)
    {
        var value2 = Mathf.Round(bgUIController.parallaxScaleSlider.value * 100) / 100f;

        bgUIController.parallaxScaleInputField.SetTextWithoutNotify(value2.ToString());
    }

    public void ParallaxScaleSliderValueChanged(BaseEventData data)
    {
        var value = Mathf.Round(bgUIController.parallaxScaleSlider.value * 100) / 100f;
        bgUIController.parallaxScaleInputField.SetTextWithoutNotify(value.ToString());
        bgUIController.bgController.SetParallaxScale(value);

        PlayerSettingPref.Instance.BGControllerSettings.ParallaxScale = value;
    }

    public void ParallaxScaleInputFieldValueChanged(string value)
    {
        try
        {
            float parallaxScale = Mathf.Round(Mathf.Clamp(float.Parse(value),
                bgUIController.parallaxScaleSlider.minValue, bgUIController.parallaxScaleSlider.maxValue) * 100) / 100f;
            bgUIController.parallaxScaleSlider.SetValueWithoutNotify(parallaxScale);

            PlayerSettingPref.Instance.BGControllerSettings.ParallaxScale = parallaxScale;
        }
        catch (Exception e)
        {
        }
    }

    public void ParallaxScaleInputFieldEndEdit(string value)
    {
        try
        {
            float parallaxScale = Mathf.Round(Mathf.Clamp(float.Parse(value),
                bgUIController.parallaxScaleSlider.minValue, bgUIController.parallaxScaleSlider.maxValue) * 100) / 100f;
            bgUIController.parallaxScaleSlider.SetValueWithoutNotify(parallaxScale);
            bgUIController.bgController.SetParallaxScale(parallaxScale);

            bgUIController.parallaxScaleInputField.SetTextWithoutNotify(parallaxScale.ToString());

            PlayerSettingPref.Instance.BGControllerSettings.ParallaxScale = parallaxScale;
        }
        catch (Exception e)
        {
        }
    }

    public void BasicScaleSliderValueChanged(float value)
    {
        var value2 = Mathf.Round(value * 100) / 100f;
        bgUIController.basicScaleText.SetText(value2.ToString());
    }
    public void BasicScaleSliderValueChanged(BaseEventData data)
    {
        Debug.Log("BasicScaleSliderValueChanged");
        var value = Mathf.Round(bgUIController.basicScaleSlider.value * 100) / 100f;
        bgUIController.basicScaleText.SetText(value.ToString());
        
        bgUIController.bgController.SetBasicScale(value);

        PlayerSettingPref.Instance.BGControllerSettings.BasicScale = value;
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

    public void TargetFrameRateSliderValueChanged(float value)
    {
        applicationUIController.targetFrameRateText.SetText(value.ToString());
    }

    public void TargetFrameRateSliderValueChanged(BaseEventData data)
    {
        var targetFrameRate = (int)applicationUIController.targetFrameRateSlider.value;
        applicationUIController.targetFrameRateText.SetText(targetFrameRate.ToString());
        applicationUIController.applicationSetting.SetTargetFrameRate(targetFrameRate);

        PlayerSettingPref.Instance.ApplicationSettings.TargetFrameRate = targetFrameRate;
    }

    public void BackgroundRunningTypeDropdownValueChanged(int value)
    {
        var type = (BackgroundRunningType)value;
        applicationUIController.applicationSetting.SetBackgroundRunningType(type);

        PlayerSettingPref.Instance.ApplicationSettings.BackgroundRunningType = type;
    }

    #endregion

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
        applicationUIController.targetFrameRateText.SetText(applicationSettings.TargetFrameRate.ToString());
        // applicationUIController.targetFrameRateInputField.SetTextWithoutNotify(applicationSettings.TargetFrameRate.ToString());
        applicationUIController.backgroundRunningTypeDropdown.SetValueWithoutNotify(
            (int)applicationSettings.BackgroundRunningType);
        applicationUIController.backgroundRunningTypeDropdown.RefreshShownValue();

        bgUIController.reverseToggle.SetIsOnWithoutNotify(bgControllerSettings.Reverse);
        // Debug.Log("Setting useParallaxToggle");
        bgUIController.useParallaxToggle.SetIsOnWithoutNotify(bgControllerSettings.UseParallax);
        // Debug.Log("Result: " +  bgUIController.useParallaxToggle.isOn);
        bgUIController.useFullBGToggle.SetIsOnWithoutNotify(bgControllerSettings.UseFullBackground);
        bgUIController.parallaxScaleSlider.SetValueWithoutNotify(bgControllerSettings.ParallaxScale);
        bgUIController.parallaxScaleInputField.SetTextWithoutNotify(bgControllerSettings.ParallaxScale.ToString());
        bgUIController.basicScaleSlider.SetValueWithoutNotify(bgControllerSettings.BasicScale);
        bgUIController.basicScaleText.SetText(bgControllerSettings.BasicScale.ToString());
        bgUIController.dampingSlider.SetValueWithoutNotify(bgControllerSettings.Damping);
        bgUIController.dampingText.SetText(bgControllerSettings.Damping.ToString());

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
    public Toggle useParallaxToggle;
    public Slider parallaxScaleSlider;
    public Toggle useFullBGToggle;
    public Slider basicScaleSlider;
    public TMP_Text basicScaleText;
    public Slider dampingSlider;
    public TMP_Text dampingText;
    public TMP_InputField parallaxScaleInputField;
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

    // public TMP_InputField targetFrameRateInputField;
    public TMP_Text targetFrameRateText;
    public TMP_Dropdown backgroundRunningTypeDropdown;
}