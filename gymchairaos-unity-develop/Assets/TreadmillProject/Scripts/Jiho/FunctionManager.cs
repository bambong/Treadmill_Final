using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jiho
{
    public class FunctionManager : MonoBehaviour
    {
        [SerializeField] private GameObject player_obj;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private TestPlayerController player;

        private readonly float FUNC_UPDATE_TIME = 0.2f;
        private Vector3 startPos;

        private float distance;
        private float last_distance = 0;
        private int health = 100;
        private int gaugeCount = 0;
        private bool isFunction = true;
        private bool isMoving = true;
        private bool isSet = true;
        private float showDistance = 0;

        private void Awake()
        {
            startPos = player_obj.transform.position;
            //TokenInputManager.Instance.ConnectToDevice();
            StartCoroutine(Function());
        }

        private void UpdateDistance()
        {
            distance = (Vector3.Distance(startPos, player_obj.transform.position)) / 3;
            showDistance += (TokenInputManager.Instance.CurSpeed * FUNC_UPDATE_TIME);
        }

        private void UpdateHealth()
        {
            float temp_distance = distance - last_distance;

            if(temp_distance >= 5)
            {
                health -= 1;
                last_distance = distance;
            }

            if(health <= 0)
            {
                PlayerDead();
            }
        }

        private IEnumerator Function()
        {
            while(isFunction)
            {
                if (player_obj.transform.rotation.y > 0) gaugeCount = 1;
                else if (player_obj.transform.rotation.y < 0) gaugeCount = -1;
                else gaugeCount = 0;

                UpdateFunction();
                yield return new WaitForSeconds(FUNC_UPDATE_TIME);
            }
            StopFunction();
        }

        private void PlayerDead()
        {
            isFunction = false;
            player.IsMoving(false);
            uiManager.GameOverActive(true);
        }



        private void StopFunction()
        {
            StopCoroutine(Function());
        }

        private void Gauge(int _count)
        {
            if(_count == -1)
            {
                float gauge = (player_obj.transform.rotation.y * _count) * 100;
                uiManager.UpdateGauge(gauge, 0);
            }
            else if(_count == 1)
            {
                float gauge = (player_obj.transform.rotation.y * _count) * 100;
                uiManager.UpdateGauge(0, gauge);
            }
            else
            {
                uiManager.UpdateGauge(0, 0);
            }
        }

        private IEnumerator Hit()
        {
            player.IsMoving(!isMoving);
            for(int i = 0; i < 6; i++)
            {
                isSet = !isSet;
                player_obj.SetActive(isSet);
                yield return new WaitForSeconds(0.1f);

            }
            player.IsMoving(isMoving);
            StopCoroutine(Hit());
        }

        private void MiusHealth(int _count)
        {
            health = health - _count;
        }

        private void PlusHealth(int _count)
        {
            health = health + _count;

            if (health > 100)
                health = 100;
        }

        public void PlusItem(int _count)
        {
            PlusHealth(_count);
        }

        public void UpdateFunction()
        {
            UpdateDistance();
            UpdateHealth();
            UpdateGauge(gaugeCount);
            uiManager.UpdateUI(health, showDistance);
        }

        public void UpdateGauge(int _count)
        {
            Gauge(_count);
        }

        public void PlayerHit(int _count)
        {
            MiusHealth(_count);
            StartCoroutine(Hit());
        }
    }
}

