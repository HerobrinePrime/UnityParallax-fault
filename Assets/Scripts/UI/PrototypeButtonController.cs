using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PrototypeButtonController : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    protected TweenerCore<Color, Color, ColorOptions> bgTween;
    protected TweenerCore<Color, Color, ColorOptions> borderTween;
    protected TweenerCore<Color, Color, ColorOptions> textTween;
    protected TweenerCore<Color, Color, ColorOptions> iconTween;


    protected Image _bgImage;
    protected Image _borderImage;
    protected TMP_Text _text;
    protected Image _iconImage;

    protected Color _defaultBgColor;
    protected Color _defaultBorderColor;
    protected Color _defaultTextColor;
    protected Color _defaultIconColor;
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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

    protected void ClearTweens()
    {
        bgTween?.Kill();
        borderTween?.Kill();
        textTween?.Kill();
        iconTween?.Kill();
    }
}