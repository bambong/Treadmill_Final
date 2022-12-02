using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class Hurdle_2 : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private int damage;
        [SerializeField] private GameObject firstPosition;

        private bool minus;

        private HurdleManager hurdleManager;

        private void OnEnable()
        {
            hurdleManager = FindObjectOfType<HurdleManager>();
            InitPosition();
        }

        private void InitPosition()
        {
            minus = false;
            transform.position = new Vector3(firstPosition.transform.position.x - 10, firstPosition.transform.position.y, firstPosition.transform.position.z);
        }

        private void Update()
        {
            if(transform.position.x >= 13)
            {
                minus = true;
            }
            else if(transform.position.x <= -13)
            {
                minus = false;
            }

            if (minus)
                HurdleMove(-1);
            else
                HurdleMove(1);

        }

        private void HurdleMove(int _num)
        {
            transform.position = new Vector3(transform.position.x + ((speed * Time.deltaTime) * _num), transform.position.y, firstPosition.transform.position.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                hurdleManager.Hurdle(damage);
                InitPosition();
            }
        }
    }
}

