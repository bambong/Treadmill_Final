using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private poolInfo[] poolInfos;

        private void Awake()
        {
            for (int i = 0; i < poolInfos.Length; i++)
            {
                poolInfos[i].Init(transform);
            }
        }

        [System.Serializable]
        public class poolInfo
        {
            [SerializeField] private string m_name;
            [SerializeField] private Transform m_originalObj;
            [SerializeField] private Transform[] m_pool;
            [SerializeField] private int maxIndex;
            [SerializeField] private int index;

            public Transform Spawn(Vector3 position)
            {
                index = (index + 1) % m_pool.Length;
                Transform target = m_pool[index];

                target.position = position;

                return target;
            }
            
            public void Init(Transform originalParent)
            {
                m_pool = new Transform[maxIndex];
                for (int i = 0; i < m_pool.Length; i++)
                {
                    Transform newTf = Instantiate(m_originalObj);
                    newTf.parent = originalParent;
                    newTf.gameObject.SetActive(false);
                }
            }
        }
    }
}