using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MusicButtonController : PrototypeButtonController
{
    private static MusicButtonController selectedButton;

    protected TweenerCore<Color, Color, ColorOptions> numberTween;
    protected TMP_Text _number;
    
    // private bool selected = false;
    private bool unused = false;

    protected void Start()
    {
        base.Start();
        
        _number = transform.Find("number").GetComponent<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        
        numberTween = DOTween.To(() => _number.color, x => _number.color = x,
            SelectedTextColor, SettingButtonsController.Instance.colorChangingDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        
        numberTween = DOTween.To(() => _number.color, x => _number.color = x,
            _defaultTextColor, SettingButtonsController.Instance.colorChangingDuration);
    }

    public void onClick()
    {
        if (unused)
            return;

        if (selectedButton == this)
            return;

        /**
         * TODO:
         */

        selectedButton = this;
    }
}