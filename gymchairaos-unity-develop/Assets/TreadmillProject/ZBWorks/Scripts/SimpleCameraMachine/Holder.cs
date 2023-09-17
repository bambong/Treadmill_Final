using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.SimpleCamMachine
{
    public class Holder : MonoBehaviour
    {
        public Vector3 FocusPos { get => tf_focus.position; }

        [SerializeField] Brain brain;
        [SerializeField] Transform tf_focus;
        [SerializeField] bool resetZone;

        public void FocusToThis()
        {
            if (resetZone) brain.ResetFocus();
            else brain.FocusTo(this);
        }
    }
}