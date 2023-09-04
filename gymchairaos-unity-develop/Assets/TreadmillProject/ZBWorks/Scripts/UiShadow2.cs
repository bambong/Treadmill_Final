using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class UiShadow2 : MonoBehaviour
{
    [SerializeField] Image image;

    Color nowColor;

    public void SetColor(Color color)
    {
        image.color = color;
        nowColor = color;
    }
    public void SetAlpha(float a, bool tween, float duration = 0, UnityAction onTweenEnded = null)
    {
        image.DOKill();
        Color targetColor = new Color(nowColor.r, nowColor.g, nowColor.b, a);
        if (tween)
        {
            if (onTweenEnded == null) 
                image.DOColor(targetColor, duration).SetUpdate(true);
            else
            {
                UnityEvent unityEvent = new UnityEvent();
                unityEvent.AddListener(onTweenEnded);
                image.DOColor(targetColor, duration).SetUpdate(true).OnComplete(unityEvent.Invoke);
            }
        }
        else
        {
            image.color = targetColor;
        }
    }
    public void SetActive(bool active)
    {
        image.gameObject.SetActive(active);
    }

    private void Awake()
    {
        nowColor = image.color;
    }
}
