using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private void LateUpdate()
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - 7);
        }


    }
}



