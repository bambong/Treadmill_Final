using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZB.Balance
{
    public class TrainerShower : MonoBehaviour
    {
        [SerializeField] private Transform m_tf_trainer;

        private Vector3 m_p3_upPosition;
        private Vector3 m_p3_downPosition;

        public void TrainerUp()
        {
            m_tf_trainer.DOMove(m_p3_upPosition, 1);
        }

        public void TrainerDown()
        {
            m_tf_trainer.DOMove(m_p3_downPosition, 1);
        }

        private void Awake()
        {
            m_p3_upPosition = m_tf_trainer.transform.position;
            m_p3_downPosition = m_p3_upPosition + new Vector3(0, 0, -5);
        }
    }
}