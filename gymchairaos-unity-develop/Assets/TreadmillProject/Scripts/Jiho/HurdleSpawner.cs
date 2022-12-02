using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace jiho
{
    public class HurdleSpawner : MonoBehaviour
    {
        private int count = 0;
        private bool isHurdle;
        [SerializeField] private GameObject[] hurdles;
        

        private void Awake()
        {
            for(int i = 0; i < hurdles.Length; i++)
            {
                hurdles[i].transform.position = this.transform.position;
                hurdles[i].SetActive(false);
            }
            StartCoroutine(Spawn());
        }
        
        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(1f);
            while(true)
            {
                if(isHurdle)
                {
                    if (count > hurdles.Length - 1)
                    {
                        count = 0;
                    }
                    else
                    {
                        SetActiveHurdle(hurdles[count]);
                        count++;
                    }
                }
                yield return new WaitForSeconds(2.5f);
            }
        }

        private void SetActiveHurdle(GameObject _obj)
        {
            int random = Random.Range(-8, 7);
            _obj.transform.position = new Vector3(random, transform.position.y, transform.position.z);
            _obj.SetActive(true);
        }

        private void HurdleTransform(GameObject _obj)
        {
            _obj.transform.position = this.transform.position;
        }

        public void HurdleReset(GameObject _obj)
        {
            HurdleTransform(_obj);
        }

        public void IsHurdle(bool _bool)
        {
            isHurdle = _bool;
        }
    }
}

