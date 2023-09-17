using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace jiho
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI distance_text;
        [SerializeField] private TextMeshProUGUI gameStart_text;
        [SerializeField] private Image health_image;
        [SerializeField] private Image left_gauge_image;
        [SerializeField] private Image right_gauge_image;
        [SerializeField] private GameObject gameOver_obj;
        [SerializeField] private GameObject gameStart_obj;
        [SerializeField] private TestPlayerController player;
        [SerializeField] private ItemSpawner itemSpawner;
        [SerializeField] private HurdleSpawner hurdleSpawner;
        [SerializeField] private TextMeshProUGUI gameover_distance_text;
        private int totalDistance; 
        private int waitsecond;

        private void Awake()
        {
         
           StartCoroutine(GameStaartCheckConnect());
        }



        private void UpdateHp(float _hp)
        {
            health_image.fillAmount = _hp / 100;
        }

        private void UpdateDistance(float _distance)
        {
            distance_text.text = ((int)_distance).ToString() + "M";
        }

        private void UpdateGaugeLeft(float _Gauge)
        {
            left_gauge_image.fillAmount = _Gauge / 30;
        }

        private void UpdateGaugeRight(float _Gauge)
        {
            right_gauge_image.fillAmount = _Gauge / 30;
        }

        private void gameStart()
        {
            gameStart_obj.SetActive(true);
            StartCoroutine(gameStartWaitCoroutine());
        }
        private IEnumerator GameStaartCheckConnect() 
        {
            while (!Managers.Token.IsConnect) 
            {
                yield return new WaitForSeconds(1f);
            }
            gameStart();
        }
        private IEnumerator gameStartWaitCoroutine()
        {
            waitsecond = 3;
            player.IsMoving(false);
            itemSpawner.IsItem(false);
            hurdleSpawner.IsHurdle(false);

            while(waitsecond > 0)
            {
                gameStart_text.text = waitsecond.ToString();
                yield return new WaitForSeconds(1f);
                waitsecond--;
            }
            gameStart_text.text = "시작";
            yield return new WaitForSeconds(1f);

            gameStart_obj.SetActive(false);
            player.IsMoving(true);
            itemSpawner.IsItem(true);
            hurdleSpawner.IsHurdle(true);
        }
       
        public void GameOverActive(bool _active)
        {
            if (_active)
            {
                gameover_distance_text.text = "거리:" + distance_text.text;
            }
            gameOver_obj.SetActive(_active);
        }

        public void UpdateUI(float _hp, float _distance)
        {
            UpdateHp(_hp);
            UpdateDistance(_distance);
        }

        public void UpdateGauge(float _leftGauge, float _rightGauge)
        {
            UpdateGaugeLeft(_leftGauge);
            UpdateGaugeRight(_rightGauge);
        }

    }
}

