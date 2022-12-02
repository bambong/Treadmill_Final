using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class Hurdle : MonoBehaviour
    {
        [SerializeField] private int damage;
        private HurdleManager hurdleManager;
        private HurdleSpawner hurdleSpawner;
        private bool isMove = true;

        private void OnEnable()
        {
            hurdleManager = FindObjectOfType<HurdleManager>();
            hurdleSpawner = FindObjectOfType<HurdleSpawner>();
            StartCoroutine(Move());
        }

        private void IsMove()
        {
            isMove = false;
        }

        private IEnumerator Move()
        {
            while (isMove)
            {
                this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
                yield return new WaitForSeconds(0.01f);
            }
            StopCoroutine(Move());

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                hurdleManager.Hurdle(damage);
                hurdleSpawner.HurdleReset(this.gameObject);
                this.gameObject.SetActive(false);
            }

            if (other.CompareTag("DestroyItem"))
            {
                hurdleSpawner.HurdleReset(this.gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }

}
