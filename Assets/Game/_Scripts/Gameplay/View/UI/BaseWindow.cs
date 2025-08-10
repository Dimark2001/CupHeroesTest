using System;
using UnityEngine;
using DG.Tweening;

public class BaseWindow : MonoBehaviour
{
    public event Action OnHide;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _fadeDuration = 0.3f;

    private void Awake()
    {
        _canvasGroup ??= GetComponent<CanvasGroup>();
    }

    public virtual async void Show(bool instant = false)
    {
        if (instant)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            return;
        }

        await _canvasGroup.DOFade(1f, _fadeDuration)
            .OnStart(() =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            })
            .OnComplete(() =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }).ToUniTask();
        
    }

    public virtual async void Hide(bool instant = false)
    {
        if (instant)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            OnHide?.Invoke();
            return;
        }

        await _canvasGroup.DOFade(0f, _fadeDuration)
            .OnStart(() =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }).ToUniTask();
        OnHide?.Invoke();
    }

    public void Toggle(bool show)
    {
        if (show) Show();
        else Hide();
    }
}