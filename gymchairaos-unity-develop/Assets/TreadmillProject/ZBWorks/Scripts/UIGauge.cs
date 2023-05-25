using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ZB;

namespace ZB
{
    public class UIGauge : MonoBehaviour
    {
        [SerializeField] RectTransform mask;
        [SerializeField] RectTransform body;

        [SerializeField] bool startAtZero;
        [SerializeField] float duration;
        [SerializeField] Ease ease;

        [Space(30)]
        [SerializeField] float testRatio;

        float length;

        public void ChangeRatio(float ratio)
        {
            if (ratio > 1)
                ratio = 1;
            else if (ratio < 0)
                ratio = 0;

            body.anchoredPosition = new Vector2(0, (ratio - 1) * length);
        }
        public void ChangeRatioWithTweening(float ratio)
        {
            if (ratio > 1)
                ratio = 1;
            else if (ratio < 0)
                ratio = 0;

            body.DOKill();
            body.DOAnchorPos(new Vector2(0, (ratio - 1) * length), duration).SetEase(ease);
        }

        void OnEnable()
        {
            if (startAtZero)
            {
                body.anchoredPosition = new Vector2(0, -length);
            }
        }
        void Awake()
        {
            length = mask.sizeDelta.y;
        }
    }
}