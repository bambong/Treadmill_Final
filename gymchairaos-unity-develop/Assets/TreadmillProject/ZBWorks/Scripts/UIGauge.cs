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
            Debug.Log(new Vector2(0, (ratio - 1) * length));
        }

        [ContextMenu("TestChangeRatio")]
        public void TestChangeRatio()
        {
            ChangeRatio(testRatio);
        }
        [ContextMenu("TestChangeRatioWithTweening")]
        public void TestChangeRatioWithTweening()
        {
            ChangeRatioWithTweening(testRatio);
        }

        void Awake()
        {
            length = mask.sizeDelta.y;
        }
    }
}