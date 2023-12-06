using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ZB;

namespace ZB
{
    public class DistanceRecord : MonoBehaviour
    {
        public float NowRecord { get { return nowRecord; } }

        [SerializeField] TextMeshProUGUI tmp;
        [SerializeField] private float nowRecord = 0;

        public void UpdateText(float value, string text)
        {
            nowRecord = value;
            tmp.text = $"{text} M";
        }
    }
}