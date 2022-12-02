using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlaneController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float standardTriggerDist = 150f;
    [SerializeField]
    private float moveDist = 200f;

    private float destinationPosZ;

    private void Awake()
    {
        destinationPosZ = transform.position.z;
        destinationPosZ += standardTriggerDist;
    }

    private void LateUpdate()
    {
        if(destinationPosZ <= target.position.z) 
        {
            
            var pos = transform.position;
            destinationPosZ += moveDist;
            pos.z += moveDist;
            transform.position = pos;
        }
    }
   
}
