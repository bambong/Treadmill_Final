using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;

namespace ZB
{
    public class ObstacleSpawnController : MonoBehaviour
    {
        [SerializeField] ObjectsScrolling objectsScrolling;
        [SerializeField] ObjectPooling obstaclePool;
        [SerializeField] ObstacleSet[] obstacleSet;

        [SerializeField] bool spawning;

        [ContextMenu("스폰시작")]
        public void SpawnCycleStart()
        {
            if (!spawning)
            {
                spawning = true;
                SpawnRandomObstacleSet(this.gameObject);
            }
        }
        [ContextMenu("스폰정지")]
        public void SpawnCycleStop()
        {
            if (spawning)
            {
                spawning = false;
                objectsScrolling.ResetFlexible();
            }
        }

        public void SpawnRandomObstacleSet(GameObject asdf)
        {
            Debug.Log(asdf.name);
            Debug.Log("AAAAAAAAAAAAAAAAAAA");
            ObstacleSet targetSet = obstacleSet[Random.Range(0, obstacleSet.Length)];
            Transform[] spawnedObjs;
            spawnedObjs = targetSet.Spawn();

            for (int i = 0; i < spawnedObjs.Length; i++)
            {
                objectsScrolling.InsertObj(spawnedObjs[i]);
            }
        }

        private void Awake()
        {
            for (int i = 0; i < obstacleSet.Length; i++)
            {
                obstacleSet[i].Init(obstaclePool);
            }
        }

        [System.Serializable]
        public class ObstacleSet
        {
            [SerializeField] Transform body;
            [SerializeField] ObstacleSpawnInfo[] obstacleSpawnInfos;
            ObjectPooling pool;

            public void Init(ObjectPooling pool)
            {
                this.pool = pool;

                obstacleSpawnInfos = new ObstacleSpawnInfo[body.childCount];
                for (int i = 0; i < body.childCount; i++)
                {
                    body.GetChild(i).TryGetComponent(out obstacleSpawnInfos[i]);
                }
            }

            public Transform[] Spawn()
            {
                Transform[] result = new Transform[obstacleSpawnInfos.Length];
                for (int i = 0; i < obstacleSpawnInfos.Length; i++)
                {
                    result[i] = pool.Spawn(obstacleSpawnInfos[i].TargetName, obstacleSpawnInfos[i].transform.position);
                }
                return result;
            }
        }
    }
}