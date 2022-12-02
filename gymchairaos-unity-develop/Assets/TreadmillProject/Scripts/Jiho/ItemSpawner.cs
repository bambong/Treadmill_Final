using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class ItemSpawner : MonoBehaviour
    {
        private int count = 0;
        private bool isItem;
        [SerializeField] private GameObject[] items;

        private void Awake()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].transform.position = this.transform.position;
                items[i].SetActive(false);
            }
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                if(isItem)
                {
                    if (count > items.Length - 1)
                    {
                        count = 0;
                    }
                    else
                    {
                        SetActiveItem(items[count]);
                        count++;
                    }
                }
                yield return new WaitForSeconds(10f);
            }
        }

        private void SetActiveItem(GameObject _obj)
        {
            int random = Random.Range(-8, 7);
            _obj.transform.position = new Vector3(random, transform.position.y, transform.position.z);
            _obj.SetActive(true);
        }

        private void ItemTransform(GameObject _obj)
        {
            _obj.transform.position = this.transform.position;
        }

        public void IsItem(bool _bool)
        {
            isItem = _bool;
        }

        public void ItemReset(GameObject _obj)
        {
            ItemTransform(_obj);
        }
    }
}


