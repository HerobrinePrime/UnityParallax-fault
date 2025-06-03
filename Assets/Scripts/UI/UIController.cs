using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
        volume = audioController.volumeSlider.value;
        muted = audioController.muteToggle.isOn;
    }

    public TweenerCore<float, float, FloatOptions> ToggleMenu(bool show)
    {
        return menu.DOFade(show ? menuTransparencyController.GetTransparency() : 0,
            audioController.muteToggleDduration);
    }

    public void ToggleReverse(bool reverse)
    {
        // BGController
    }

    public void VolumeSliderValueChanged(float value)
    {
    }

    public void ToggleMute(bool mute)
    {
    }

    public void SetTransparency(float transparency)
    {
        menu.alpha = transparency;
        menuTransparencyController.SetTransparency(transparency);
    }

    public void SetHorizontalConstraint(float value)
    {
    }

    public void SetVerticalConstraint(float value)
    {
    }

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
    private float _menuTtransparency = 1f;

    public void SetTransparency(float transparency)
    {
        transparencySlider.value = _menuTtransparency = transparency;
    }

    public float GetTransparency()
    {
        return _menuTtransparency;
    }
}

[Serializable]
public class ConstraintsController
{
    public Slider horizontalConstraintSlider;
    public Slider verticalConstraintSlider;
}