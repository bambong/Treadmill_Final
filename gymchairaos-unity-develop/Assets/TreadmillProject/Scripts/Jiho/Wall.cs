using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho 
{
    public class Wall : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                other.transform.position = new Vector3(0, other.transform.position.y, other.transform.position.z);
            }
        }
    }
}


