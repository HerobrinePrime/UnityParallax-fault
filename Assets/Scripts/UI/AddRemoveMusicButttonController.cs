using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddRemoveMusicButttonController : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    private TweenerCore<Color, Color, ColorOptions> bgTween;
    private TweenerCore<Color, Color, ColorOptions> iconTween;

    private Image _bgImage;
    private Image _iconImage;

    private Color _defaultBgColor;
    private Color _defaultIconColor;
    public Color SelectedBgColor;
    public Color SelectedIconColor;

    private void Start()
    {
        _bgImage = transform.Find("bg").GetComponent<Image>();
        _iconImage = transform.Find("icon").GetComponent<Image>();

        _defaultBgColor = _bgImage.color;
        _defaultIconColor = _iconImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ClearTweens();
        bgTween = DOTween.To(() => _bgImage.color, x => _bgImage.color = x,
            SelectedBgColor, SettingButtonsController.Instance.colorChangingDuration);
        iconTween = DOTween.To(() => _iconImage.color, x => _iconImage.color = x,
            SelectedIconColor, SettingButtonsController.Instance.colorChangingDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ClearTweens();
        bgTween = DOTween.To(() => _bgImage.color, x => _bgImage.color = x,
            _defaultBgColor, SettingButtonsController.Instance.colorChangingDuration);
        iconTween = DOTween.To(() => _iconImage.color, x => _iconImage.color = x,
            _defaultIconColor, SettingButtonsController.Instance.colorChangingDuration);
    }

    protected void ClearTweens()
    {
        bgTween?.Kill();
        iconTween?.Kill();
    }
}