using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZB;

namespace ZB
{
    public class CheckLayer_Trigger : MonoBehaviour
    {
        public enum CheckLayerBy { idToLayerReader, originalLayer }

        public bool Touching { get { return touching; } }

        public Transform LastTouchedTransform { get => lastTouchedTransform; }

        [SerializeField] CheckLayerBy checkLayerBy;

        [SerializeField] UnityEvent enterEvent;
        [SerializeField] UnityEvent stayEvent;
        [SerializeField] UnityEvent exitEvent;

        [SerializeField] string targetID;
        [SerializeField] int targetLayer;
        [SerializeField] bool touching;

        [SerializeField] Transform lastTouchedTransform;

        private void OnTriggerEnter(Collider other)
        {
            int target = -1;
            if (checkLayerBy == CheckLayerBy.idToLayerReader)
                target = IdToLayerReader.Instance.IdToLayer(targetID);
            else if (checkLayerBy == CheckLayerBy.originalLayer)
                target = targetLayer;

            if (other.gameObject.layer == target)
            {
                lastTouchedTransform = other.transform;
                touching = true;
                enterEvent.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            int target = -1;
            if (checkLayerBy == CheckLayerBy.idToLayerReader)
                target = IdToLayerReader.Instance.IdToLayer(targetID);
            else if (checkLayerBy == CheckLayerBy.originalLayer)
                target = targetLayer;

            if (other.gameObject.layer == target)
            {
                stayEvent.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == targetLayer)
            {
                touching = false;
                exitEvent.Invoke();
            }
        }
    }
}