using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;


namespace ZB
{
    public class ObjectPooling : MonoBehaviour
    {
        [SerializeField] ObjectType[] objectTypes;

        public Transform Spawn(string name, Vector3 position)
        {
            for (int i = 0; i < objectTypes.Length; i++)
            {
                if (objectTypes[i].Name == name)
                {
                    return objectTypes[i].Spawn(position);
                }
            }

            Debug.LogError("오브젝트 풀 -> 스폰 실패");
            return null;
        }

        private void Awake()
        {
            for (int i = 0; i < objectTypes.Length; i++)
            {
                objectTypes[i].Init(transform);
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Spawn("Test", Vector3.zero);
            }
        }

        [System.Serializable]
        public class ObjectType
        {
            public string Name { get { return name; } }

            [SerializeField] string name;
            [SerializeField] Transform original;
            [SerializeField] int initSpawnCount;
            Transform parentTf;
            int index;
            List<Transform> pool;

            public void Init(Transform parentTf)
            {
                this.parentTf = parentTf;

                pool = new List<Transform>();
                for (int i = 0; i < initSpawnCount; i++)
                {
                    pool.Add(Instantiate(original));
                    pool[i].gameObject.SetActive(false);
                    pool[i].parent = parentTf;
                }
            }

            public Transform Spawn(Vector3 position)
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    index = (index + 1) % pool.Count;
                    if (!pool[index].gameObject.activeSelf)
                    {
                        pool[index].gameObject.SetActive(true);
                        pool[index].position = position;
                        return pool[index];
                    }
                }

                pool.Add(Instantiate(original));
                index = pool.Count - 1;
                pool[index].gameObject.SetActive(true);
                pool[index].position = position;
                pool[index].parent = parentTf;
                return pool[index];
            }
        }
    }
}