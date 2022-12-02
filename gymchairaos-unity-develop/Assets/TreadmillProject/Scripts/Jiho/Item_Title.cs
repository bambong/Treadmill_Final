using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class Item_Title : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;

        private void OnEnable()
        {
            StartCoroutine(Rotate());
        }

        private void OnDisable()
        {
            StopCoroutine(Rotate());
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                this.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
                yield return null;
            }

        }
    }
}

