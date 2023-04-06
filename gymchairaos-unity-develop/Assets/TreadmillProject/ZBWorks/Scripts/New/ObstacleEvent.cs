using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB
{
    public class ObstacleEvent : MonoBehaviour
    {
        [SerializeField] ObstacleSpawnController obstacle;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] PlayerHP playerHP;
        [SerializeField] float term;
        [SerializeField] float checkSpeed;
        [SerializeField] float recentChecked = 0;

        [SerializeField] bool eventChecking;

        public void ResetState()
        {
            recentChecked = 0;
        }

        public void CheckActive(bool active)
        {
            eventChecking = active;
        }

        void Update()
        {
            if (eventChecking)
            {
                if (timeCounter.NowTime > recentChecked + term)
                {
                    Debug.Log("!@");
                    recentChecked += term;

                    //��ֹ�����
                    if (Random.Range(0, 2) == 1)
                    {
                        //���⼭ ��ֹ� �����ڵ��ۼ�
                        obstacle.SpawnRandomObstacleSet(this.gameObject);
                    }

                    //����ӵ��˻�, ü�°���
                    if (TokenInputManager.Instance.CurSpeed < checkSpeed)
                    {
                        playerHP.MinusHP(1);
                    }
                }
            }
        }
    }
}