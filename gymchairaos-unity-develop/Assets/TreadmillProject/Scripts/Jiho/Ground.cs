using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private GroundManager groundManager;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                groundManager.UseGround(this.gameObject);
            }
        }
    }
}


