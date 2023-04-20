using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class DestinationController : MonoBehaviour
    {
        private bool isClear = false;
        private void OnTriggerEnter(Collider other)
        {
            if (isClear) 
            {
                return;
            }
            if(other.gameObject.CompareTag("Player")) 
            {
                GameSceneManager.Instance.SetStateGameClear();
                isClear = true;
            }
            else if (other.gameObject.CompareTag("AI"))
            {
                GameSceneManager.Instance.SetStateGameOver();
                isClear = true;
            }
        }
    }

}
