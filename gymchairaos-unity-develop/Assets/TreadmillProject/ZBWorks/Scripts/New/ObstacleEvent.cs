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

                    //장애물스폰
                    if (Random.Range(0, 2) == 1)
                    {
                        //여기서 장애물 스폰코드작성
                        obstacle.SpawnRandomObstacleSet(this.gameObject);
                    }

                    //현재속도검사, 체력감소
                    if (TokenInputManager.Instance.CurSpeed < checkSpeed)
                    {
                        playerHP.MinusHP(1);
                    }
                }
            }
        }
    }
}