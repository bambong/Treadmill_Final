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

        public double m_NowSpeed { get => Managers.Token.CurSpeedMeterPerSec; }
        public float m_leftRpm { get => Managers.Token.Save_left_speed; }
        public float m_rightRpm { get => Managers.Token.Save_right_speed; }

        [SerializeField] private float m_guageMax;
        [SerializeField] private float m_hp;

        [ContextMenu("Start")]
        public void FocusStart()
        {
            Managers.Token.Save_left_speed = 0;
            Managers.Token.Save_right_speed = 0;

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
                        int slantDir = m_leftRpm > m_rightRpm ? -1 : 1;
                        m_playerSlant.Slant(slantDir);
                        SoundLocator.Instance.PlaySfx(slantDir == -1 ?
                            "sfx_to_the_right" : "sfx_to_the_left");
                        Debug.LogError("???");
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
                        Debug.LogError("!!!");
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
                            m_playerSlant.SlantReset();
                            //패배판정
                            m_defeatChecker.Defeat();
                            SoundLocator.Instance.PlaySfx("sfx_oops");
                        }
                    }
                }

                yield return true;
            }
        }

        private void Awake()
        {
#if UNITY_EDITOR
            Managers.Token.AddLeftTokenEvent(()=>
            {
                AddLeftToken();
                Debug.Log($"NowTokenInfo / Left : {Managers.Token.Save_left_speed} / Right : {Managers.Token.Save_right_speed}");
            });
            Managers.Token.AddRightTokenEvent(()=>
            {
                AddRightToken();
                Debug.Log($"NowTokenInfo / Left : {Managers.Token.Save_left_speed} / Right : {Managers.Token.Save_right_speed}");
            });
            StartCoroutine(DecreaseToken());
#endif
            Managers.Token.ReceivedEvent += SideMove;
        }
        public void AddLeftToken()
        {
            Managers.Token.Save_left_speed += 50;
        }
        public void AddRightToken()
        {
            Managers.Token.Save_right_speed += 50;
        }
        public IEnumerator DecreaseToken()
        {
            while (gameObject != null)
            {
                Managers.Token.Save_left_speed = Mathf.Max(0, Managers.Token.Save_left_speed - 10);
                Managers.Token.Save_right_speed = Mathf.Max(0, Managers.Token.Save_right_speed - 10);
                yield return new WaitForSeconds(0.2f);
            }
        }
        void SideMove()
        {
            var speed = 240f;
            var dis = Managers.Token.Save_right_speed - Managers.Token.Save_left_speed;

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