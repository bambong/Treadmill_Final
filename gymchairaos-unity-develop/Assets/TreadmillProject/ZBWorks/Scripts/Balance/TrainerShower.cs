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
            bool sfxPlayed = false;

            yield return CheckCycle_WFS;

            while (true)
            {
                //최소 속도를 넘지 못함
                if (TokenInputManager.Instance.CurSpeed < m_minSpeed)
                {
                    //따라가기 시작
                    if (waitT == 0)
                    {
                        TrainerUp();
                        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect(Random.Range(0, 2) == 0 ?
                            "sfx_whistle1" : "sfx_whistle2");
                    }

                    waitT += Time.deltaTime;
                    if (!sfxPlayed && waitT > 2)
                    {
                        sfxPlayed = false;
                        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect(Random.Range(0, 2) == 0 ?
                            "sfx_whistle1" : "sfx_whistle2");
                    }
                }
                else
                {
                    //따라가기 종료
                    if (waitT > 0)
                    {
                        sfxPlayed = false;
                        waitT = 0;
                        TrainerDown();
                    }
                }

                //게임오버 판정
                if (waitT >= 3)
                {
                    Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("sfx_stop");
                    m_defeatChecker.Defeat();
                }

                yield return true;
            }
        }
    }
}