using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZB.Balance
{
    public class TrainerShower : MonoBehaviour
    {
        [SerializeField] private DefeatConditionChecker m_defeatChecker;
        [SerializeField] private Transform m_tf_trainer;
        [SerializeField] private float m_minSpeed;

        private Vector3 m_p3_upPosition;
        private Vector3 m_p3_downPosition;

        public void TrainerUp()
        {
            m_tf_trainer.DOKill();
            m_tf_trainer.DOMove(m_p3_upPosition, 1);
        }

        public void TrainerDown()
        {
            m_tf_trainer.DOKill();
            m_tf_trainer.DOMove(m_p3_downPosition, 1);
        }

        public void CheckStart()
        {
            CheckCycle_C = CheckCycle();
            StartCoroutine(CheckCycle_C);
        }

        public void CheckStop()
        {
            StopCoroutine(CheckCycle_C);
        }

        private void Awake()
        {
            m_p3_upPosition = m_tf_trainer.transform.position;
            m_p3_downPosition = m_p3_upPosition + new Vector3(0, 0, -5);
        }
        WaitForSeconds CheckCycle_WFS = new WaitForSeconds(2f);
        IEnumerator CheckCycle_C;
        IEnumerator CheckCycle()
        {
            TrainerDown();
            float waitT = 0;

            yield return CheckCycle_WFS;

            while (true)
            {
                //�ּ� �ӵ��� ���� ����
                if (TokenInputManager.Instance.CurSpeed < m_minSpeed)
                {
                    //���󰡱� ����
                    if (waitT == 0)
                    {
                        TrainerUp();
                    }

                    waitT += Time.deltaTime;
                }
                else
                {
                    //���󰡱� ����
                    if (waitT > 0)
                    {
                        waitT = 0;
                        TrainerDown();
                    }
                }

                //���ӿ��� ����
                if (waitT >= 3)
                {
                    m_defeatChecker.Defeat();
                }

                yield return true;
            }
        }
    }
}