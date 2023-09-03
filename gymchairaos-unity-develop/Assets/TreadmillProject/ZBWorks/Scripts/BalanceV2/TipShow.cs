using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipShow : MonoBehaviour
{
    [SerializeField] RectTransform rtf;

    [SerializeField] Vector2 pos_Appear;
    [SerializeField] Vector2 pos_Disappear;
    [SerializeField] bool activing;

    public void Active(bool active)
    {
        if (active)
        {
            activing = true;

            rtf.DOKill();
            rtf.DOAnchorPos(pos_Disappear, 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);
        }
        else
        {
            activing = false;

            rtf.DOKill();
            rtf.DOAnchorPos(pos_Appear, 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);
        }
    }

    public void OnBtnClick()
    {
        Active(!activing);
    }
}
