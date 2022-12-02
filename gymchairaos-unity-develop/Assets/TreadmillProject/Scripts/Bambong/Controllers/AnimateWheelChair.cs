using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class AnimateWheelChair : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private float moveSpeed = 1f;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                animator.SetFloat("MoveSpeed",moveSpeed);
                animator.Play("Move");
            }
        }
    }
}
