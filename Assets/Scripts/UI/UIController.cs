using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public CanvasGroup menu;
    public AudioSource audioSource;
    public Slider volumeSlider;
    public Toggle muteToggle;
    public float muteToggleDduration = 0.5f;
    public Slider transparencySlider;
    private float _menuTtransparency = 1f;

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
        volume = volumeSlider.value;
        muted = muteToggle.isOn;
    }

    public void SetTransparency(float transparency)
    {
        menu.alpha = _menuTtransparency = transparency;
    }

    public TweenerCore<float, float, FloatOptions> ToggleMenu(bool show)
    {
        return menu.DOFade(show ? _menuTtransparency : 0, muteToggleDduration);
    }
    
}