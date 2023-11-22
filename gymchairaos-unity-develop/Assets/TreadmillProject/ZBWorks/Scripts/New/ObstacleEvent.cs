using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB
{
    public class ObstacleEvent : MonoBehaviour
    {
        [SerializeField] ObstacleSpawnController obstacle;
        [SerializeField] ObjectsScrolling scroll;
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
            if (active)
            {
                scroll.ResetScrolledDist();
                obstacle.SpawnRandomObstacleSet();
            }
        }

        void Update()
        {
            if (eventChecking)
            {
                if (scroll.ScrolledDist > recentChecked + term)
                {
                    recentChecked += term;
                    obstacle.SpawnRandomObstacleSet();
                }
            }
        }
    }
}