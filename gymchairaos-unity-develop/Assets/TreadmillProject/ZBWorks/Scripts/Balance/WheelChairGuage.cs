using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Balance
{
    public class WheelChairGuage : MonoBehaviour
    {
        [SerializeField] PlayerSlant m_playerSlant;
        [SerializeField] Animation m_ani;
        [SerializeField] private DefeatConditionChecker m_defeatChecker;
        [SerializeField] private UIGauge m_uiGuage_left;
        [SerializeField] private UIGauge m_uiGuage_Right;
        [SerializeField] private UIGauge m_uiGuage_hp;

        public float m_NowSpeed { get => TokenInputManager.Instance.CurSpeed; }
        public float m_leftRpm { get => TokenInputManager.Instance.Save_left_rpm; }
        public float m_rightRpm { get => TokenInputManager.Instance.Save_right_rpm; }

        [SerializeField] private float m_guageMax;
        [SerializeField] private float m_hp;

        [ContextMenu("Start")]
        public void FocusStart()
        {
            TokenInputManager.Instance.Save_left_rpm = 0;
            TokenInputManager.Instance.Save_right_rpm = 0;

            m_ani.Play();

            FocusCycle_C = FocusCycle();
            StartCoroutine(FocusCycle_C);
        }
        public void FocusStop()
        {
            m_ani.Stop();

            StopCoroutine(FocusCycle_C);
        }
        public void GuageReset()
        {
            m_uiGuage_hp.ChangeRatioWithTweening(1);
            m_uiGuage_left.ChangeRatioWithTweening(0);
            m_uiGuage_Right.ChangeRatioWithTweening(0);
        }

        IEnumerator FocusCycle_C;
        IEnumerator FocusCycle()
        {
            bool unBalance = false;
            float unBalanceTime = 0;
            int unBalanceValue = 240;
            m_hp = 100;

            m_uiGuage_left.ChangeRatioWithTweening(0);
            m_uiGuage_Right.ChangeRatioWithTweening(0);
            m_uiGuage_hp.ChangeRatioWithTweening(1);

            while(true)
            {
                m_uiGuage_left.ChangeRatioWithTweening(m_leftRpm / m_guageMax);
                m_uiGuage_Right.ChangeRatioWithTweening(m_rightRpm / m_guageMax);

                if (!unBalance)
                {
                    //오차
                    if (Mathf.Abs(m_leftRpm - m_rightRpm) > unBalanceValue)
                    {
                        unBalance = true;
                        unBalanceTime = 0;
                        m_playerSlant.Slant(m_leftRpm > m_rightRpm ? -1 : 1);
                    }
                }
                if (unBalance)
                {
                    unBalanceTime += Time.deltaTime;

                    if(Mathf.Abs(m_leftRpm - m_rightRpm) <= unBalanceValue)
                    {
                        unBalanceTime = 0;
                        unBalance = false;
                        m_playerSlant.Slant(0);
                    }

                    if (unBalanceTime >= 5)
                    {
                        unBalanceTime = 0;
                        unBalance = false;

                        //여기서 휠체어 체력깎인다.
                        m_hp -= 25;
                        m_uiGuage_hp.ChangeRatioWithTweening(m_hp / 100);
                        if (m_hp <= 0)
                        {
                            //패배판정
                            m_defeatChecker.Defeat();
                        }
                    }
                }

                yield return true;
            }
        }

        private void Awake()
        {
#if UNITY_EDITOR
            TokenInputManager.Instance.AddLeftTokenEvent(()=>
            {
                AddLeftToken();
                Debug.Log($"NowTokenInfo / Left : {TokenInputManager.Instance.Save_left_rpm} / Right : {TokenInputManager.Instance.Save_right_rpm}");
            });
            TokenInputManager.Instance.AddRightTokenEvent(()=>
            {
                AddRightToken();
                Debug.Log($"NowTokenInfo / Left : {TokenInputManager.Instance.Save_left_rpm} / Right : {TokenInputManager.Instance.Save_right_rpm}");
            });
            StartCoroutine(DecreaseToken());
#endif
            TokenInputManager.Instance.ReceivedEvent += SideMove;
        }
        public void AddLeftToken()
        {
            TokenInputManager.Instance.Save_left_rpm += 50;
        }
        public void AddRightToken()
        {
            TokenInputManager.Instance.Save_right_rpm += 50;
        }
        public IEnumerator DecreaseToken()
        {
            while (gameObject != null)
            {
                TokenInputManager.Instance.Save_left_rpm = Mathf.Max(0, TokenInputManager.Instance.Save_left_rpm - 10);
                TokenInputManager.Instance.Save_right_rpm = Mathf.Max(0, TokenInputManager.Instance.Save_right_rpm - 10);
                yield return new WaitForSeconds(0.2f);
            }
        }
        void SideMove()
        {
            var speed = 240f;
            var dis = TokenInputManager.Instance.Save_right_rpm - TokenInputManager.Instance.Save_left_rpm;

            if (Mathf.Abs(dis) < 100)
            {
                speed = 0;
            }
            else
            {
                speed *= dis > 0 ? 1 : -1;
            }
        }
    }
}