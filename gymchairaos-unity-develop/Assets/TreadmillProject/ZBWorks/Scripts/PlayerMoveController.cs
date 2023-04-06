using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;

namespace ZB
{
    public class PlayerMoveController : MonoBehaviour
    {
        Rigidbody rb;

        [SerializeField] float moveSpeed;

        private void Awake()
        {
            TryGetComponent(out rb);
        }
        public void MoveInput(float Horizontal)
        {
            rb.velocity = new Vector2(moveSpeed * Horizontal, 0);
        }
    }
}
