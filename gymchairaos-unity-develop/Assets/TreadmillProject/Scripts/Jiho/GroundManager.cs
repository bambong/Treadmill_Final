using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class GroundManager : MonoBehaviour
    {
        [SerializeField] private GameObject idleGround;
        [SerializeField] private GameObject hurdleSpawner;
        [SerializeField] private GameObject itemSpawner;

        private void Awake()
        {
            HurdleSpawnerTransform();
            ItemSpawnerTransform();
        }

        private void HurdleSpawnerTransform()
        {
            hurdleSpawner.transform.position = new Vector3(idleGround.transform.position.x, idleGround.transform.position.y, idleGround.transform.position.z);
        }

        private void ItemSpawnerTransform()
        {
            itemSpawner.transform.position = new Vector3(idleGround.transform.position.x, idleGround.transform.position.y + 1.1f, idleGround.transform.position.z);
        }

        private void MoveGround(GameObject ground_obj)
        {
            ground_obj.transform.position = new Vector3(ground_obj.transform.position.x, ground_obj.transform.position.y, idleGround.transform.position.z + 134.5f);
        }

        private void ChangeGround(GameObject ground_obj)
        {
            idleGround = ground_obj;
        }
        
        public void UseGround(GameObject ground_obj)
        {
            MoveGround(ground_obj);
            ChangeGround(ground_obj);
            HurdleSpawnerTransform();
            ItemSpawnerTransform();
        }
    }
}

