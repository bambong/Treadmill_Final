using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ZB.Balance
{
    public class DistanceCount : MonoBehaviour
    {
        [SerializeField] WheelChairGuage m_wheelChairGuage;
        [SerializeField] TextMeshProUGUI m_tmp_distanceShow;

        public float m_NowDistance { get => m_nowDistance; }
        [SerializeField] private float m_nowDistance;

        [ContextMenu("Start")]
        public void CountStart()
        {
            CountCycle_C = CountCycle();
            StartCoroutine(CountCycle_C);
        }

        [ContextMenu("Stop")]
        public void CountStop()
        {
            StopCoroutine(CountCycle_C);
        }

        IEnumerator CountCycle_C;
        IEnumerator CountCycle()
        {
            m_nowDistance = 0;
            while (true)
            {
                m_nowDistance += m_wheelChairGuage.m_NowSpeed;
                m_tmp_distanceShow.text = ((int)m_nowDistance).ToString();
                yield return null;
            }
        }
    }
}