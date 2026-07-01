using System;
using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class TimeUIController : MonoBehaviour
{
    //Instance
    private static TimeUIController _instance;

    public static TimeUIController Instance
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

    public TimeCalculator TimeCalculator;

    public Toggle forceTimeToggle;
    public TMP_Dropdown TODDropdown;
    public Toggle forceSeasonToggle;
    public TMP_Dropdown seasonDropdown;
    public Toggle autoCalculateSunPositionToggle;
    public StartTime startTime;
    public TMP_InputField transitionDurationInputField;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            throw new Exception("TimeUIController instance already exists!");
            // Application.Quit();
        }

        _instance = this;

        CreateSeasonTODOptions();
        startTime.dawnStartTime.InitTimeOptions();
        startTime.dayStartTime.InitTimeOptions();
        startTime.duskStartTime.InitTimeOptions();
        startTime.nightStartTime.InitTimeOptions();
        
        InitFromSettings();
    }

    private void Start()
    {
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

    public void InitFromSettings()
    {
        var timeSettings = PlayerSettingPref.Instance.TimeSettings;

        TimeCalculator.forceTime = timeSettings.ForceTime;
        TimeCalculator.forceSeason = timeSettings.ForceSeason;
        TimeCalculator.forceCurrentSeason = timeSettings.Season;
        TimeCalculator.forceCurrentTimeOfDay = timeSettings.TimeOfDay;
        TimeCalculator.AutoCalculateSunPosition = timeSettings.AutoCalculateSunPosition;
        TimeCalculator.dawnStartHour = timeSettings.DawnStartHour;
        TimeCalculator.dawnStartMinute = timeSettings.DawnStartMinute;
        TimeCalculator.dayStartHour = timeSettings.DayStartHour;
        TimeCalculator.dayStartMinute = timeSettings.DayStartMinute;
        TimeCalculator.duskStartHour = timeSettings.DuskStartHour;
        TimeCalculator.duskStartMinute = timeSettings.DuskStartMinute;
        TimeCalculator.nightStartHour = timeSettings.NightStartHour;
        TimeCalculator.nightStartMinute = timeSettings.NightStartMinute;
        TimeCalculator.transitionDurationMinute = timeSettings.TransitionDuration;

        Debug.Log(
            $"Init TimeUIController from settings: ForceTime={timeSettings.ForceTime}, ForceSeason={timeSettings.ForceSeason}, TimeOfDay={timeSettings.TimeOfDay}, Season={timeSettings.Season}, AutoCalculateSunPosition={timeSettings.AutoCalculateSunPosition}, DawnStartTime={timeSettings.DawnStartHour}:{timeSettings.DawnStartMinute}, DayStartTime={timeSettings.DayStartHour}:{timeSettings.DayStartMinute}, DuskStartTime={timeSettings.DuskStartHour}:{timeSettings.DuskStartMinute}, NightStartTime={timeSettings.NightStartHour}:{timeSettings.NightStartMinute}, TransitionDuration={timeSettings.TransitionDuration}");

        forceTimeToggle.SetIsOnWithoutNotify(timeSettings.ForceTime);
        // forceTimeToggle.isOn = timeSettings.ForceTime;
        forceSeasonToggle.SetIsOnWithoutNotify(timeSettings.ForceSeason);
        // forceSeasonToggle.isOn = timeSettings.ForceSeason;
        TODDropdown.SetValueWithoutNotify((int)timeSettings.TimeOfDay);
        seasonDropdown.SetValueWithoutNotify((int)timeSettings.Season);
        autoCalculateSunPositionToggle.SetIsOnWithoutNotify(timeSettings.AutoCalculateSunPosition);
        // autoCalculateSunPositionToggle.isOn = timeSettings.AutoCalculateSunPosition;
        startTime.dawnStartTime.SetTimeWithoutNotify(timeSettings.DawnStartHour, timeSettings.DawnStartMinute);
        startTime.dayStartTime.SetTimeWithoutNotify(timeSettings.DayStartHour, timeSettings.DayStartMinute);
        startTime.duskStartTime.SetTimeWithoutNotify(timeSettings.DuskStartHour, timeSettings.DuskStartMinute);
        startTime.nightStartTime.SetTimeWithoutNotify(timeSettings.NightStartHour, timeSettings.NightStartMinute);
        transitionDurationInputField.SetTextWithoutNotify(timeSettings.TransitionDuration.ToString());

        InitHiddenUI(timeSettings.ForceTime, timeSettings.ForceSeason, timeSettings.AutoCalculateSunPosition);
    }

    public void InitHiddenUI(bool forceTime, bool forceSeason, bool autoCalculateSunPosition)
    {
        TODDropdown.transform.parent.gameObject.SetActive(forceTime);
        seasonDropdown.transform.parent.gameObject.SetActive(forceSeason);
        autoCalculateSunPositionToggle.GetComponent<StartTimeToggle>().ToggleStartTime(autoCalculateSunPosition);
    }

    [Obsolete]
    public TimeSettings GetMetaSettings()
    {
        return new TimeSettings(
            TimeCalculator.forceTime,
            TimeCalculator.forceSeason,
            TimeCalculator.forceCurrentSeason,
            TimeCalculator.forceCurrentTimeOfDay,
            TimeCalculator.AutoCalculateSunPosition,
            TimeCalculator.dawnStartHour,
            TimeCalculator.dawnStartMinute,
            TimeCalculator.dayStartHour,
            TimeCalculator.dayStartMinute,
            TimeCalculator.duskStartHour,
            TimeCalculator.duskStartMinute,
            TimeCalculator.nightStartHour,
            TimeCalculator.nightStartMinute,
            TimeCalculator.transitionDurationMinute
        );
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