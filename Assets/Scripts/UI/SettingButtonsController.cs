using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingButtonsController : MonoBehaviour
{
    public static SettingButtonsController Instance;
    public static List<SettingButtonController> _allButtons = new List<SettingButtonController>();
    public float colorChangingDuration = 0.1f;
    public float settingMenuChangingDuration = 0.3f;

    public float containerWidth = 416f;
    public float audioMenuContainerHeight = 440;
    public float audioMenuContentHeight = 389;
    
    public RectTransform Container;
    public RectTransform bg;
    public RectTransform background;
    public RectTransform border;
    // public RectTransform seperator;
    // public RectTransform SettingButtons;
    [FormerlySerializedAs("settingMenuContainer")]
    public RectTransform Contents;
    public RectTransform ScrollViewAudio;
    
    
    

    private TweenerCore<Vector2, Vector2, VectorOptions> settingMenuChangingTween;
    // public HorizontalLayoutGroup settingMenuContainer;

    private void Awake()
    {
        Instance = this;
    }

    // private void Start()
    // {
    //     Debug.Log(container.rect.width);
    // }

    public void TransitionToSetting(SettingType settingType)
    {
        if (settingMenuChangingTween != null || settingMenuChangingTween.IsActive())
        {
            settingMenuChangingTween.Kill();
        }

        // settingMenuContainer.position = new Vector3(-containerWidth * (int)settingType, settingMenuContainer.position.y, settingMenuContainer.position.z);
        // settingMenuContainer.padding.left = -(int)(containerWidth * (int)settingType);
        Contents.anchoredPosition = new Vector2(-containerWidth * (int)settingType,
            Contents.anchoredPosition.y);
        // settingMenuChangingTween = DOTween.To(() => settingMenuContainer.anchoredPosition,
        //     x => settingMenuContainer.anchoredPosition = x,
        //     new Vector2(-containerWidth * (int)settingType, settingMenuContainer.anchoredPosition.y),
        //     settingMenuChangingDuration);
    }
}