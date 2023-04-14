using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ZB;


namespace ZB
{
    public class HpPoint : MonoBehaviour
    {
        [SerializeField] GameObject front;
        RectTransform rtf;
        Vector2 origianlSize;

        bool on = true;

        void Awake()
        {
            front.TryGetComponent(out rtf);
            origianlSize = rtf.sizeDelta;
        }

        public void Minus()
        {
            if (on)
            {
                rtf.DOSizeDelta(Vector2.zero, 0.5f).SetEase(Ease.OutQuart);
            }
            on = false;
        }

        public void Plus()
        {
            if (!on)
            {
                rtf.DOSizeDelta(origianlSize, 0.5f).SetEase(Ease.OutQuart);
            }
            on = true;
        }
    }
}