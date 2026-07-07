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

    public int containerWidth = 416;
    public int defaultMenuContainerHeight = 220;
    public int defaultMenuContentHeight = 169;
    public int audioMenuContainerHeight = 440;
    public int audioMenuContentHeight = 389;
    public int audioMenuWidth = 500;

    public RectTransform Container;
    public RectTransform bg;
    public RectTransform background;
    public RectTransform border;

    public RectTransform seperator;
    public RectTransform SettingButtons;
    
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

        if (settingType == SettingType.Audio)
        {
            // Container.rect.height = audioMenuContainerHeight;
            Container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, audioMenuContainerHeight);
            bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, audioMenuContainerHeight);
            background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, audioMenuContainerHeight);
            border.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, audioMenuContainerHeight);

            Contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, audioMenuContentHeight);
            ScrollViewAudio.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, audioMenuContentHeight);
            
            Container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
            bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
            background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
            border.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
            // seperator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
            seperator.gameObject.SetActive(false);
            SettingButtons.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
            Contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
            ScrollViewAudio.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, audioMenuWidth);
        }
        else
        {
            Container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultMenuContainerHeight);
            bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultMenuContainerHeight);
            background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultMenuContainerHeight);
            border.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultMenuContainerHeight);

            Contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultMenuContentHeight);
            ScrollViewAudio.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultMenuContentHeight);
            
            
            Container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
            bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
            background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
            border.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
            // seperator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
            seperator.gameObject.SetActive(true);
            SettingButtons.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
            Contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
            ScrollViewAudio.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
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