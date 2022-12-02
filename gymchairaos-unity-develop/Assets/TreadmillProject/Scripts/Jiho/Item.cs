using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private int count = 10;

        private ItemManager itemManager;
        private ItemSpawner itemSpawner;
        private bool isMove = true;

        private void OnEnable()
        {
            itemManager = FindObjectOfType<ItemManager>();
            itemSpawner = FindObjectOfType<ItemSpawner>();
            StartCoroutine(ItemMove());
        }

        private void IsMove()
        {
            isMove = false;
        }

        private IEnumerator ItemMove()
        {
            while (isMove)
            {
                this.transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z - 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            StopCoroutine(ItemMove());

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                itemManager.GetItem(count);
                itemSpawner.ItemReset(this.gameObject);
                this.gameObject.SetActive(false);
            }

            if (other.CompareTag("DestroyItem"))
            {
                itemSpawner.ItemReset(this.gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }
}

