using System;
using System.Collections;
using Enum;
using UnityEngine;

public class TimeCalculator : MonoBehaviour
{
    private TimeCalculator _calculator;
    private DateTime now = default;
    public BGController bgController;
    public TimeUIController timeUIController;

    public DateTime Now
    {
        get
        {
            if (now == default)
            {
                now = DateTime.Now;
            }

            return now;
        }
    }

    public TimeCalculator Instance
    {
        get
        {
            if (_calculator == null)
            {
                _calculator = new TimeCalculator();
            }

            return _calculator;
        }
    }

    public bool forceTime = false;
    private bool _lastForceTime;
    public bool forceSeason = false;
    private bool _lastForceSeason;
    public Season forceCurrentSeason;
    public TimeOfDay forceCurrentTimeOfDay;
    public bool AutoCalculateSunPosition = false;
    public int dawnStartHour = 6;
    public int dawnStartMinute = 0;
    public int dayStartHour = 9;
    public int dayStartMinute = 0;
    public int duskStartHour = 16;
    public int duskStartMinute = 0;
    public int nightStartHour = 19;
    public int nightStartMinute = 0;
    public float transitionDurationMinute = 30;


    private TimeOfDay _timeOfDayBeforeTransition;
    private TimeOfDay _currentTimeOfDay;
    private Season _lastSeason;
    private Season _currentSeason;
    private bool _isInitialized = false;

    private void Start()
    {
        // Initialize settings from PlayerSettingPref

        _lastForceSeason = forceSeason;
        _lastForceTime = forceTime;

        _currentSeason = CheckSeason();
        StartCoroutine(CalculateTime());
    }

    private IEnumerator CalculateTime()
    {
        while (true)
        {
            now = DateTime.Now;
            // Debug.Log($"Current time: {now}");

            if (_isInitialized)
            {
                DetectTimeChange(now);
                // DetectSeasonChange(now);
            }
            else
            {
                InitTime(now);
                _isInitialized = true;
            }

            // if (forceSeason)
            // {
            //     Season newSeason = CheckSeason();
            //     if (newSeason != _currentSeason)
            //     {
            //         Debug.Log($"Season changed from {_currentSeason} to {newSeason}");
            //         _currentSeason = newSeason;
            //         bgController.Transition(_timeOfDayBeforeTransition, _currentTimeOfDay, _currentSeason,
            //             transitionDurationMinute);
            //     }
            // }

            float wait = 1 - (now.Millisecond / 1000f);
            yield return new WaitForSeconds(wait);
        }
    }

    private void DetectTimeChange(DateTime now)
    {
        TimeOfDay newTimeOfDay = CheckTimeOfDay(now);
        float _transitionDurationMinute = transitionDurationMinute;
        // _timeOfDayBeforeTransition = timeOfDay.TimeOfDayBeforeTransition;
        _timeOfDayBeforeTransition = _currentTimeOfDay;

        Season newSeason = CheckSeason();

        if (newTimeOfDay != _currentTimeOfDay || newSeason != _currentSeason || _lastForceTime != forceTime)
        {
            if (newTimeOfDay != _currentTimeOfDay)
            {
                Debug.Log($"Time of day changed from {_currentTimeOfDay} to {newTimeOfDay}");
                _currentTimeOfDay = newTimeOfDay;
            }

            if (newSeason != _currentSeason)
            {
                Debug.Log($"Season changed from {_currentSeason} to {newSeason}");
                _currentSeason = newSeason;
            }

            if (
                _lastForceSeason != forceSeason
                // || _lastForceTime != forceTime
            )
            {
                _lastForceSeason = forceSeason;
                // _lastForceTime = forceTime;

                // Debug.Log("Force Time or Season toggled, transition duration set to 0");
                Debug.Log("Force Season toggled, transition duration set to 0");
                _transitionDurationMinute = 0;
            }

            if (_lastForceTime != forceTime)
            {
                _lastForceTime = forceTime;
                //reinit
                Debug.Log("Force Time toggled, reinitializing time");
                InitTime(now);
                return;
            }

            //when forceTime is true and TimeOfDay is changed, duration should be 0;
            if (forceTime)
            {
                Debug.Log("Force Time is enabled, transition duration set to 0");
                _transitionDurationMinute = 0;
            }

            // Trigger time of day change event here
            bgController.Transition(_timeOfDayBeforeTransition, _currentTimeOfDay, _currentSeason,
                _transitionDurationMinute);
        }
    }

    // private void DetectSeasonChange(DateTime now)
    // {
    //     Season newSeason = CheckSeason();
    //     if (newSeason != _currentSeason)
    //     {
    //         Debug.Log($"Season changed from {_currentSeason} to {newSeason}");
    //         _currentSeason = newSeason;
    //         // Trigger season change event here
    //         bgController.Transition(_timeOfDayBeforeTransition, _currentTimeOfDay, _currentSeason,
    //             transitionDurationMinute);
    //     }
    // }
    //

    private void InitTime(DateTime now)
    {
        // _currentTimeOfDay = CheckTimeOfDay(now);
        // bgController.Transition(_currentTimeOfDay, _currentSeason, transitionDurationMinute);

        float durationMinute = 0;
        float transitionStartValue = 0;
        float diffHour = 0;
        float diffMinute = 0;
        TimeOfDay timeOfDayBeforeTransition;

        // _currentTimeOfDay = CheckTimeOfDay(now);
        if (forceSeason)
        {
            _currentSeason = forceCurrentSeason;
        }
        else
        {
            _currentSeason = CheckSeason();
        }

        if (forceTime)
        {
            timeOfDayBeforeTransition = _currentTimeOfDay;
            _currentTimeOfDay = forceCurrentTimeOfDay;
        }
        else
        {
            // // if (now.Hour >= dawnStartHour && now.Hour < dayStartHour)
            // if (
            //     (now.Hour == dawnStartHour && now.Minute >= dawnStartMinute) ||
            //     (now.Hour > dawnStartHour && now.Hour < dayStartHour)
            // )
            // {
            //     _currentTimeOfDay = TimeOfDay.Evening;
            //     // _timeOfDayBeforeTransition = TimeOfDay.Night;
            //     timeOfDayBeforeTransition = TimeOfDay.Night;
            //     diffHour = now.Hour - dawnStartHour;
            // }
            // // else if (now.Hour >= dayStartHour && now.Hour < duskStartHour)
            // else if (
            //     (now.Hour == dayStartHour && now.Minute >= dayStartMinute) ||
            //     (now.Hour > dayStartHour && now.Hour < duskStartHour)
            // )
            // {
            //     _currentTimeOfDay = TimeOfDay.Day;
            //     timeOfDayBeforeTransition = TimeOfDay.Evening;
            //     diffHour = now.Hour - dayStartHour;
            // }
            // // else if (now.Hour >= duskStartHour && now.Hour < nightStartHour)
            // else if (
            //     (now.Hour == duskStartHour && now.Minute >= duskStartMinute) ||
            //     (now.Hour > duskStartHour && now.Hour < nightStartHour)
            // )
            // {
            //     _currentTimeOfDay = TimeOfDay.Evening;
            //     timeOfDayBeforeTransition = TimeOfDay.Day;
            //     diffHour = now.Hour - duskStartHour;
            // }
            // else
            // {
            //     _currentTimeOfDay = TimeOfDay.Night;
            //     timeOfDayBeforeTransition = TimeOfDay.Evening;
            //     if (now.Hour >= nightStartHour)
            //     {
            //         diffHour = now.Hour - nightStartHour;
            //     }
            //     else
            //     {
            //         diffHour = now.Hour + (24 - nightStartHour);
            //     }
            // }

            int current = ToMinutes(now.Hour, now.Minute);

            int dawnStart = ToMinutes(dawnStartHour, dawnStartMinute);
            int dayStart = ToMinutes(dayStartHour, dayStartMinute);
            int duskStart = ToMinutes(duskStartHour, duskStartMinute);
            int nightStart = ToMinutes(nightStartHour, nightStartMinute);

            if (current >= dawnStart && current < dayStart)
            {
                _currentTimeOfDay = TimeOfDay.Evening;
                timeOfDayBeforeTransition = TimeOfDay.Night;
                diffMinute = current - dawnStart;
            }
            else if (current >= dayStart && current < duskStart)
            {
                _currentTimeOfDay = TimeOfDay.Day;
                timeOfDayBeforeTransition = TimeOfDay.Evening;
                diffMinute = current - dayStart;
            }
            else if (current >= duskStart && current < nightStart)
            {
                _currentTimeOfDay = TimeOfDay.Evening;
                timeOfDayBeforeTransition = TimeOfDay.Day;
                diffMinute = current - duskStart;
            }
            else
            {
                _currentTimeOfDay = TimeOfDay.Night;
                timeOfDayBeforeTransition = TimeOfDay.Evening;
                if (current >= nightStart)
                {
                    diffMinute = current - nightStart;
                }
                else
                {
                    diffMinute = current + (24 * 60 - nightStart);
                }
            }

            // if (diffHour * 60 + now.Minute < transitionDurationMinute)
            // {
            //     // transitionStartValue = (float)now.Minute / transitionDurationMinute;
            //     // duration = transitionDurationMinute - now.Minute;
            //     transitionStartValue = (diffHour * 60 + now.Minute) / transitionDurationMinute;
            //     // Debug.Log("Transition Start Value: " + transitionStartValue);
            //     durationMinute = transitionDurationMinute - (diffHour * 60 + now.Minute);
            // }

            if (diffMinute < transitionDurationMinute)
            {
                transitionStartValue = diffMinute / transitionDurationMinute;
                durationMinute = transitionDurationMinute - diffMinute;
            }
        }


        // if (now.Minute < transitionDurationMinute)


        Debug.Log("durationMinute: " + durationMinute);
        bgController.Transition(timeOfDayBeforeTransition, _currentTimeOfDay, _currentSeason, durationMinute,
            transitionStartValue);


        Debug.Log($"Initial time of day: {_currentTimeOfDay}");
    }

    private Season CheckSeason()
    {
        if (forceSeason)
        {
            return forceCurrentSeason;
        }

        DateTime now = DateTime.Now;
        if (now.Month >= 3 && now.Month <= 5)
        {
            return Season.Spring;
        }
        else if (now.Month >= 6 && now.Month <= 8)
        {
            return Season.Summer;
        }
        else if (now.Month >= 9 && now.Month <= 11)
        {
            return Season.Autumn;
        }
        else
        {
            return Season.Winter;
        }
    }

    private struct TimeOfDayStruct
    {
        public TimeOfDay TimeOfDayBeforeTransition;
        public TimeOfDay CurrentTimeOfDay;
    }

    private TimeOfDay CheckTimeOfDay(DateTime now)
    {
        if (forceTime)
        {
            // TimeOfDay timeOfDayBeforeTransition;
            // switch (forceCurrentTimeOfDay)
            // {
            //     case TimeOfDay.Day:
            //         timeOfDayBeforeTransition = TimeOfDay.Evening;
            //         break;
            //     case TimeOfDay.Evening:
            //         timeOfDayBeforeTransition = TimeOfDay.Day;
            //         break;
            //     case TimeOfDay.Night:
            //         timeOfDayBeforeTransition = TimeOfDay.Evening;
            //         break;
            //     default:
            //         timeOfDayBeforeTransition = TimeOfDay.Day;
            //         break;
            // }

            // return new TimeOfDayStruct
            // {
            //     TimeOfDayBeforeTransition = _currentTimeOfDay,
            //     CurrentTimeOfDay = forceCurrentTimeOfDay
            // };
            return forceCurrentTimeOfDay;
        }

        int current = ToMinutes(now.Hour, now.Minute);

        int dawnStart = ToMinutes(dawnStartHour, dawnStartMinute);
        int dayStart = ToMinutes(dayStartHour, dayStartMinute);
        int duskStart = ToMinutes(duskStartHour, duskStartMinute);
        int nightStart = ToMinutes(nightStartHour, nightStartMinute);

        // if (now.Hour >= dawnStartHour && now.Hour < dayStartHour)
        if (current >= dawnStart && current < dayStart)
        {
            return TimeOfDay.Evening;
            // return new TimeOfDayStruct
            // {
            //     TimeOfDayBeforeTransition = TimeOfDay.Night,
            //     CurrentTimeOfDay = TimeOfDay.Evening
            // };
        }
        // else if (now.Hour >= dayStartHour && now.Hour < duskStartHour)
        else if (current >= dayStart && current < duskStart)
        {
            return TimeOfDay.Day;
            // return new TimeOfDayStruct
            // {
            //     TimeOfDayBeforeTransition = TimeOfDay.Evening,
            //     CurrentTimeOfDay = TimeOfDay.Day
            // };
        }
        // else if (now.Hour >= duskStartHour && now.Hour < nightStartHour)
        else if (current >= duskStart && current < nightStart)
        {
            return TimeOfDay.Evening;
            // return new TimeOfDayStruct
            // {
            //     TimeOfDayBeforeTransition = TimeOfDay.Day,
            //     CurrentTimeOfDay = TimeOfDay.Evening
            // };
        }
        else
        {
            return TimeOfDay.Night;
            // return new TimeOfDayStruct
            // {
            //     TimeOfDayBeforeTransition = TimeOfDay.Evening,
            //     CurrentTimeOfDay = TimeOfDay.Night
            // };
        }
    }

    private int ToMinutes(int hour, int minute)
    {
        return hour * 60 + minute;
    }

    public void ToggleForceTime()
    {
        forceTime = !forceTime;
    }

    public void ToggleForceSeason()
    {
        forceSeason = !forceSeason;
    }

    public void ToggleAutoCalculateSunPosition()
    {
        AutoCalculateSunPosition = !AutoCalculateSunPosition;
    }

    public void SetTOD(int value)
    {
        var timeOfDay = (TimeOfDay)value;
        forceCurrentTimeOfDay = timeOfDay;
    }

    public void SetSeason(int value)
    {
        var season = (Season)value;
        forceCurrentSeason = season;
    }

    public void OnDawnHChanged(int value)
    {
        dawnStartHour = value;
        OnSettingsChanged();
    }

    public void OnDawnMChanged(int value)
    {
        dawnStartMinute = value;
        OnSettingsChanged();
    }

    public void OnDayHChanged(int value)
    {
        dayStartHour = value;
        OnSettingsChanged();
    }

    public void OnDayMChanged(int value)
    {
        dayStartMinute = value;
        OnSettingsChanged();
    }

    public void OnDuskHChanged(int value)
    {
        duskStartHour = value;
        OnSettingsChanged();
    }

    public void OnDuskMChanged(int value)
    {
        duskStartMinute = value;
        OnSettingsChanged();
    }

    public void OnNightHChanged(int value)
    {
        nightStartHour = value;
        OnSettingsChanged();
    }

    public void OnNightMChanged(int value)
    {
        nightStartMinute = value;
        OnSettingsChanged();
    }

    public void OnTDFEndEdit(string value)
    {
        if (value == "")
        {
            // Debug.Log("Transition Duration Minute changed: " + "0");
            transitionDurationMinute = 0;
            timeUIController.transitionDurationInputField.text = "0";
            OnSettingsChanged();
            return;
        }

        if (float.TryParse(value, out float result))
        {
            if (transitionDurationMinute != result)
            {
                transitionDurationMinute = result;
                // Debug.Log("Transition Duration Minute changed: " + result);
                OnSettingsChanged();
            }
        }
    }

    public void OnSettingsChanged()
    {
        Debug.Log("Setting changed, reinitializing time");
        InitTime(DateTime.Now);
    }
}