using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiShadow : MonoBehaviour
{
    [SerializeField] Image shadowImg;
    [SerializeField] bool active;

    [Space]
    [SerializeField] float duration;

    Color originalColor;

    private void Awake()
    {
        originalColor = shadowImg.color;
        shadowImg.gameObject.SetActive(true);
        shadowImg.color = Color.clear;
    }

    public void Active(bool active)
    {
        //È°¼ºÈ­
        if (!this.active && active)
        {
            this.active = active;
            shadowImg.DOKill();

            shadowImg.DOColor(originalColor, duration).SetEase(Ease.OutQuart).SetUpdate(true);
        }
        else if (this.active && !active)
        {
            this.active = active;
            shadowImg.DOKill();

            shadowImg.DOColor(Color.clear, duration).SetEase(Ease.OutQuart).SetUpdate(true);
        }
    }
}
