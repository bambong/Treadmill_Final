using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ZB.Balance
{
    public class TimeCount : MonoBehaviour
    {
        public int m_EarnedObjects { get => m_earnedObjects; }

        [SerializeField] private ObjectsScrolling m_objectScroll;
        [SerializeField] private ObjectPooling m_objectPool;
        [SerializeField] private Transform m_tf_spawnPosition;
        [SerializeField] private TextMeshProUGUI m_tmp;
        [SerializeField] private int m_earnedObjects;

        [ContextMenu("Start")]
        public void CountStart()
        {
            TimeObjectSpawnCycle_C = TimeObjectSpawnCycle();
            StartCoroutine(TimeObjectSpawnCycle_C);
        }
        public void CountStop()
        {
            StopCoroutine(TimeObjectSpawnCycle_C);
        }
        public void CountUp()
        {
            m_earnedObjects++;
            m_tmp.text = (m_earnedObjects * 30).ToString();
        }

        WaitForSeconds TimeObjectSpawnCycle_WFS = new WaitForSeconds(30);
        IEnumerator TimeObjectSpawnCycle_C;
        IEnumerator TimeObjectSpawnCycle()
        {
            m_earnedObjects = 0;
            while (true)
            {
                yield return TimeObjectSpawnCycle_WFS;

                //여기서 스폰
                m_objectScroll.InsertObj(m_objectPool.Spawn("TimeObject", m_tf_spawnPosition.position));
            }
        }
    }
}