using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider volumeSlider;
    public Toggle muteToggle;

    public static float volume;
    public static bool muted;

    /*
     * TODO: Initiliaze the UI with data instead of componets
     */
    private void Start()
    {
        // volumeSlider.value = volume = audioSource.volume;
        // muteToggle.isOn = muted = audioSource.mute;
    }

    private void Update()
    {
        volume = volumeSlider.value;
        muted = muteToggle.isOn;
    }

}