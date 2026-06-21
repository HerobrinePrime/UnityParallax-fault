using System;
using TMPro;
using UnityEngine;

public class TimeDropDown : MonoBehaviour
{
    public TMP_Dropdown HDropdown;
    public TMP_Dropdown MDropdown;

    private void Start()
    {
        HDropdown.options.Clear();
        for (int i = 0; i < 24; i++)
        {
            HDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString("D2")));
        }
        MDropdown.options.Clear();
        for (int i = 0; i < 60; i++)
        {
            MDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString("D2")));
        }
    }

    public void SetTime(int hour, int minute)
    {
        HDropdown.value = hour;
        MDropdown.value = minute;
    }
}