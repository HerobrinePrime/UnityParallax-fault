using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enum;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingButtonController : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    // private static List<SettingButtonController> _allButtons = new List<SettingButtonController>();

    public bool Selected = false;
    public SettingType settingType;
    private bool _isSelected = false;


    private TweenerCore<Color, Color, ColorOptions> bgTween;
    private TweenerCore<Color, Color, ColorOptions> borderTween;
    private TweenerCore<Color, Color, ColorOptions> textTween;
    private TweenerCore<Color, Color, ColorOptions> iconTween;

    private Image _bgImage;
    private Image _borderImage;
    private TMP_Text _text;
    private Image _iconImage;

    private Color _defaultBgColor;
    private Color _defaultBorderColor;
    private Color _defaultTextColor;
    private Color _defaultIconColor;
    public Color SelectedBgColor;
    public Color SelectedBorderColor;
    public Color SelectedTextColor;
    public Color SelectedIconColor;

    private void Start()
    {
        _bgImage = transform.Find("bg").GetComponent<Image>();
        _borderImage = transform.Find("border").GetComponent<Image>();
        _text = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        _iconImage = transform.Find("Icon").GetComponent<Image>();

        _defaultBgColor = _bgImage.color;
        _defaultBorderColor = _borderImage.color;
        _defaultTextColor = _text.color;
        _defaultIconColor = _iconImage.color;

        // Debug.Log($"Default colors: {_defaultBgColor}, {_defaultBorderColor}, {_defaultTextColor}, {_defaultIconColor}");
        SettingButtonsController._allButtons.Add(this);

        if (Selected)
        {
            OnPointerEnter(null);
            _isSelected = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("OnPointerEnter");
        // _bgImage.color = SelectedBgColor;
        // _borderImage.color = SelectedBorderColor;
        // _text.color = SelectedTextColor;
        // _iconImage.color = SelectedIconColor;]
        ClearTweens();
        bgTween = DOTween.To(() => _bgImage.color, x => _bgImage.color = x,
            SelectedBgColor, SettingButtonsController.Instance.colorChangingDuration);
        borderTween = DOTween.To(() => _borderImage.color, x => _borderImage.color = x,
            SelectedBorderColor, SettingButtonsController.Instance.colorChangingDuration);
        textTween = DOTween.To(() => _text.color, x => _text.color = x,
            SelectedTextColor, SettingButtonsController.Instance.colorChangingDuration);
        iconTween = DOTween.To(() => _iconImage.color, x => _iconImage.color = x,
            SelectedIconColor, SettingButtonsController.Instance.colorChangingDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("OnPointerExit");
        if (!_isSelected)
        {
            Deselect();
        }
    }

    public void OnClick()
    {
        _isSelected = true;

        SettingButtonsController.Instance.TransitionToSetting(this.settingType);

        foreach (var button in SettingButtonsController._allButtons)
        {
            if (button != this)
            {
                button.Deselect();
            }
        }
    }

    private void Deselect()
    {
        // Debug.Log("OnDeselect");
        _isSelected = false;
        // _bgImage.color = _defaultBgColor;
        // _borderImage.color = _defaultBorderColor;
        // _text.color = _defaultTextColor;
        // _iconImage.color = _defaultIconColor;

        ClearTweens();

        bgTween = DOTween.To(() => _bgImage.color, x => _bgImage.color = x,
            _defaultBgColor, SettingButtonsController.Instance.colorChangingDuration);
        borderTween = DOTween.To(() => _borderImage.color, x => _borderImage.color = x,
            _defaultBorderColor, SettingButtonsController.Instance.colorChangingDuration);
        textTween = DOTween.To(() => _text.color, x => _text.color = x,
            _defaultTextColor, SettingButtonsController.Instance.colorChangingDuration);
        iconTween = DOTween.To(() => _iconImage.color, x => _iconImage.color = x,
            _defaultIconColor, SettingButtonsController.Instance.colorChangingDuration);
    }

    private void ClearTweens()
    {
        bgTween?.Kill();
        borderTween?.Kill();
        textTween?.Kill();
        iconTween?.Kill();
    }
}