using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuButtonController : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public Animator animator;
    public bool open = false;
    public float duration = 0.5f;

    public CanvasGroup menu;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        // _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter()
    {
        // Debug.Log("OnPointerEnter");
        _canvasGroup.DOFade(1, 0.2f);
    }

    public void OnPointerExit()
    {
        // Debug.Log("OnPointerExit");
        _canvasGroup.DOFade(0.3f, 0.2f);
    }

    public void OnPointerClick()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        open = !open;
        ToggleMenu();
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(OpenMenu(open));
    }

    private bool _isAnimating = false;
    private Coroutine _coroutine = null;

    private IEnumerator OpenMenu(bool open)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float process = time / duration;
            float currentValue = animationCurve.Evaluate(open ? process : 1 - process);
            animator.SetFloat("time", currentValue);
            yield return null;
        }

        _isAnimating = false;
    }

    void ToggleMenu()
    {
        if (open)
        {
            // menu.DOScale(1, 0.5f);
            menu.gameObject.SetActive(true);
            menu.DOFade(1, duration);
        }
        else
        {
            // menu.DOScale(0, 0.5f);
            menu.DOFade(0, duration).OnComplete(() => { menu.gameObject.SetActive(false); });
        }
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (open)
        {
            // menu.localScale = new Vector3(1, 1, 1);
            menu.alpha = 1;
            animator.SetFloat("time", 1);
        }
        else
        {
            // menu.localScale = new Vector3(0, 0, 0);
            menu.alpha = 0;
            animator.SetFloat("time", 0);
        }
    }

#endif
}