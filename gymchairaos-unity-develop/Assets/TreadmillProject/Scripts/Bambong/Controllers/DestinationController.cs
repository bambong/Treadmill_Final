using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class DestinationController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player")) 
            {
                GameSceneManager.Instance.SetStateGameClear();

            }
            else if (other.gameObject.CompareTag("AI"))
            {
                GameSceneManager.Instance.SetStateGameOver();

            }
        }
    }

}
