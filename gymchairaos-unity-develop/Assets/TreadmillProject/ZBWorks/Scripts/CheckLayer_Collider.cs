using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZB;

namespace ZB
{
    public class CheckLayer_Collider : MonoBehaviour
    {
        public bool Touching { get { return touching; } }

        [SerializeField] UnityEvent enterEvent;
        [SerializeField] UnityEvent stayEvent;
        [SerializeField] UnityEvent exitEvent;

        [SerializeField] int targetLayer;
        [SerializeField] bool touching;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == targetLayer)
            {
                touching = true;
                enterEvent.Invoke();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == targetLayer)
            {
                stayEvent.Invoke();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == targetLayer)
            {
                touching = false;
                exitEvent.Invoke();
            }
        }
    }
}