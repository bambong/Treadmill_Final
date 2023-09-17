using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZB.SimpleCamMachine
{
    public class Brain : MonoBehaviour
    {
        [SerializeField] Holder currentHolder;
        [SerializeField] float duration;
        [SerializeField] Ease ease;

        Vector3 resetPos;

        public void ResetFocus()
        {
            Move(resetPos);
        }
        public void FocusTo(Holder holder)
        {
            currentHolder = holder;
            Move(holder.FocusPos);
        }

        private void Move(Vector3 position)
        {
            Camera.main.transform.DOKill();
            Camera.main.transform.DOMove(position, duration).SetEase(ease);
        }
        private void Awake()
        {
            resetPos = Camera.main.transform.position;
        }
    }
}