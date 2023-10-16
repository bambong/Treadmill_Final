using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using ZB;

namespace ZB
{
    public class PlayerHP : MonoBehaviour
    {
        [SerializeField] UnityEvent dieEvent;
        [SerializeField] UnityEvent uEvent_Hit;
        [SerializeField] UnityEvent uEvent_plusHp;

        [SerializeField] CheckLayer_Trigger checkLayer_Obstacle;
        [SerializeField] DistanceRecord distance;
        [SerializeField] BoostGuage boostGuage;
        [SerializeField] PoliceCarChase policeCarChase;
        [SerializeField] Endurance_GameManager gameManager;

        //ÀÚµ¿Ã¼·ÂÈ¸º¹ °Å¸®
        [SerializeField] float hpGainDistance;
        float currentCheckedDistance;
        bool resetAutoGain;

        [Space]
        [SerializeField] HpPoint[] points;
        [SerializeField] int maxHP;
        [SerializeField] int nowHP;
        [SerializeField] bool checking;

        bool invincibility;

        [ContextMenu("StartChecking")]
        public void StartChecking()
        {
            checking = true;
        }

        [ContextMenu("PauseChecking")]
        public void PauseChecking()
        {
            checking = false;
        }


        //°Å¸®ºñ·Ê ÀÚµ¿ Ã¼·Â È¹µæ ½ÃÀÛ
        [ContextMenu("ÀÚµ¿Á¡¼öÈ¹µæ ½ÃÀÛ")]
        public void StartAutoHpUByDistance()
        {
            if (AutoHpUpByDistance_C != null)
            {
                StopCoroutine(AutoHpUpByDistance_C);
            }
            AutoHpUpByDistance_C = AutoHpUpByDistance();
            StartCoroutine(AutoHpUpByDistance_C);
        }
        //°Å¸®ºñ·Ê ÀÚµ¿ Ã¼·Â È¹µæ Á¾·á
        [ContextMenu("ÀÚµ¿Á¡¼öÈ¹µæ Á¾·á")]
        public void StopAutoScoreUpByDistance()
        {
            currentCheckedDistance = 0;
            if (AutoHpUpByDistance_C != null)
            {
                StopCoroutine(AutoHpUpByDistance_C);
            }
        }

        public void MinusHP_N_tempInvincibility()
        {
            if (MinusHP_N_tempInvincibility_C != null)
                StopCoroutine(MinusHP_N_tempInvincibility_C);

            SingleObstacle targetObstacle;
          
            if (checkLayer_Obstacle.LastTouchedTransform.parent.TryGetComponent(out targetObstacle))
            {
                MinusHP_N_tempInvincibility_C = MinusHP_N_tempInvincibilityC(targetObstacle.IsDynamic ? 3 : 1);
                StartCoroutine(MinusHP_N_tempInvincibility_C);
            }
        }

        public void MinusHP(int value)
        {
            if (checking && !invincibility && boostGuage.NowState != BoostGuage.State.Boost) 
            {
                uEvent_Hit.Invoke();
                for (int i = nowHP - 1; i > nowHP - value - 1; i--)
                {
                    if (i >= 0 && i < points.Length)
                    {
                        points[i].Minus();
                    }
                }
                nowHP = nowHP - value > 0 ? nowHP - value : 0;
                resetAutoGain = true;

                policeCarChase.Move(nowHP);

                if (nowHP <= 0)
                {
                    gameManager.GameOver();
                    dieEvent.Invoke();
                }
            }
        }

        public void PlusHP(int value)
        {
            if (checking)
            {
                for (int i = nowHP; i <= nowHP + value - 1; i++)
                {
                    if (i >= 0 && i < points.Length)
                    {
                        points[i].Plus();
                    }
                }
                if (nowHP < maxHP)
                    uEvent_plusHp.Invoke();
                nowHP = nowHP + value < maxHP ? nowHP + value : maxHP;
                policeCarChase.Move(nowHP);
            }
        }

        void Awake()
        {
            nowHP = maxHP;
        }

        IEnumerator AutoHpUpByDistance_C;
        IEnumerator AutoHpUpByDistance()
        {
            while (true)
            {
                if (distance.NowRecord > currentCheckedDistance + hpGainDistance)
                {
                    currentCheckedDistance += hpGainDistance;
                    PlusHP(1);
                }

                if (resetAutoGain)
                {
                    resetAutoGain = false;
                    currentCheckedDistance = distance.NowRecord;
                }

                yield return null;
            }
        }
        IEnumerator MinusHP_N_tempInvincibility_C;
        IEnumerator MinusHP_N_tempInvincibilityC(int dmgValue)
        {
            MinusHP(dmgValue);
            invincibility = true;
            yield return new WaitForSeconds(0.7f);
            invincibility = false;

        }
    }
}