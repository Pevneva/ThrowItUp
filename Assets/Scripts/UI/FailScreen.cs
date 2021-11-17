using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class FailScreen : MonoBehaviour
{
    private Image _failImage;
    private RectTransform _rectTransformFailed;
    private void Start()
    {
        _failImage = GetComponent<Image>();
        _rectTransformFailed = GetComponent<RectTransform>();
        MoveFailImage();
        // var rectSize = _rectTransformFailed.sizeDelta;
        // _rectTransformFailed.DOSizeDelta(rectSize * 0.5f, 0);
    }

    private void MoveFailImage()
    {
        Debug.Log("sizeDelta.x = " + GetComponent<RectTransform>().sizeDelta.x);
        Debug.Log("sizeDelta.y = " + GetComponent<RectTransform>().sizeDelta.y);
        var rectSize = _rectTransformFailed.sizeDelta;
        Sequence seq = DOTween.Sequence();
        // seq.Append(_rectTransformFailed.DOSizeDelta(rectSize * 0.5f, 0));
        seq.Append(_rectTransformFailed.DOSizeDelta(rectSize * 6f, 0.70f).SetEase(Ease.Linear));
        seq.Append(_rectTransformFailed.DOSizeDelta(rectSize * 3.1f, 0.30f).SetEase(Ease.Linear));
    }
}

