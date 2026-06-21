using System;
using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class TimeUIController : MonoBehaviour
{
    public Toggle forceTimeToggle;
    public TMP_Dropdown TODDropdown;
    public Toggle forceSeasonToggle;
    public TMP_Dropdown seasonDropdown;
    public Toggle autoCalculateSunPositionToggle;
    public StartTime startTime;
    public TMP_InputField transitionDurationInputField;

    private void Start()
    {
        CreateSeasonTODOptions();
    }

    private void CreateSeasonTODOptions()
    {
        foreach (var s in System.Enum.GetNames(typeof(Season)))
        {
            seasonDropdown.options.Add(new TMP_Dropdown.OptionData(s));
        }

        foreach (var t in System.Enum.GetNames(typeof(TimeOfDay)))
        {
            TODDropdown.options.Add(new TMP_Dropdown.OptionData(t));
        }
    }

    private void InitFromSettings()
    {
        var timeSettings = PlayerSettingPref.Instance.TimeSettings;
        
        
    }

    [Serializable]
    public class StartTime
    {
        public TimeDropDown dawnStartTime;
        public TimeDropDown dayStartTime;
        public TimeDropDown duskStartTime;
        public TimeDropDown nightStartTime;
    }
}