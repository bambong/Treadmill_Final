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
        public float CurrentDistanceRecord { get { return currentDistanceRecord; } }

        [SerializeField] ObjectsScrolling scroll;
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] float nowRecord;
        float currentDistanceRecord;

        bool recording;

        [ContextMenu("RecordStart")]
        public void RecordStart()
        {
            if (!recording)
            {
                nowRecord = 0;
                recording = true;
            }

            if (record_C != null)
            {
                StopCoroutine(record_C);
            }
            record_C = recordC();
            StartCoroutine(record_C);
        }

        [ContextMenu("RecordPause")]
        public void RecordPause()
        {
            if (record_C != null)
            {
                StopCoroutine(record_C);
            }
        }

        [ContextMenu("RecordStop")]
        public void RecordStop()
        {
            currentDistanceRecord = nowRecord;

            nowRecord = 0;
            text.text = ((int)nowRecord).ToString() + "M";
            recording = false;
            if (record_C != null)
            {
                StopCoroutine(record_C);
            }
        }

        IEnumerator record_C;
        IEnumerator recordC()
        {
            while (true)
            {
                nowRecord += Time.deltaTime * scroll.ScrollSpeed;
                text.text = ((int)nowRecord).ToString() + "M";
                yield return null;
            }
        }
    }
}