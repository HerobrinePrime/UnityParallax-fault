using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enum;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonsController : MonoBehaviour
{
    public static SettingButtonsController Instance;
    public static List<SettingButtonController> _allButtons = new List<SettingButtonController>();
    public float colorChangingDuration = 0.1f;
    public float settingMenuChangingDuration = 0.3f;

    public float containerWidth = 416f;

    public RectTransform settingMenuContainer;

    private TweenerCore<Vector2, Vector2, VectorOptions> settingMenuChangingTween;
    // public HorizontalLayoutGroup settingMenuContainer;

    private void Awake()
    {
        Instance = this;
    }

    public void TransitionToSetting(SettingType settingType)
    {
        if (settingMenuChangingTween != null || settingMenuChangingTween.IsActive())
        {
            settingMenuChangingTween.Kill();
        }

        // settingMenuContainer.position = new Vector3(-containerWidth * (int)settingType, settingMenuContainer.position.y, settingMenuContainer.position.z);
        // settingMenuContainer.padding.left = -(int)(containerWidth * (int)settingType);
        // settingMenuContainer.anchoredPosition = new Vector2(-containerWidth * (int)settingType, settingMenuContainer.anchoredPosition.y);
        settingMenuChangingTween = DOTween.To(() => settingMenuContainer.anchoredPosition,
            x => settingMenuContainer.anchoredPosition = x,
            new Vector2(-containerWidth * (int)settingType, settingMenuContainer.anchoredPosition.y),
            settingMenuChangingDuration);
    }
}