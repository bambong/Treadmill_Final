using bambong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZB;

namespace ZB
{
    public class Endurance_GameManager : MonoBehaviour
    {
        public static Endurance_GameManager Instance;

        [SerializeField] GameStartProduction gameStartProduction;
        [SerializeField] ObstacleEvent obstacle;
        [SerializeField] DistanceRecord distance;
        [SerializeField] HeartRate heartRate;
        [SerializeField] PlayerInputManager2 player;
        [SerializeField] ObjectsScrolling scroll;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] PlayerHP playerHP;
        [SerializeField] BoostGuage boostGuage;
        [SerializeField] ResultPageController resultPage;
        [SerializeField] RankingDataHolder rankingDataHolder;

        [Space]
        [SerializeField] float startDelay;
        WaitForSeconds startDelay_WFS; 

        void Awake()
        {
            Instance = this;
            startDelay_WFS = new WaitForSeconds(startDelay);
        }
        void Start()
        {
            StartCoroutine(BloothConnectWait());
        }

        IEnumerator BloothConnectWait() 
        {
            while(!Managers.Token.IsConnect)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(E_SceneName.Obstacle_GameScene_ZB_V2.ToString()));
            GameStartProduction_Start();
        }

        IEnumerator GameStart_C;
        IEnumerator GameStartC()
        {
            yield return startDelay_WFS;
            playerHP.PlusHP(3);
            GameStartProduction_Start();
        }
        public void GameRestart()
        {
            Managers.Scene.LoadScene(E_SceneName.Obstacle_GameScene_ZB_V2);
        }
        public void GameStartProduction_Start()
        {
            player.MoveFront_OnRebirth();
            gameStartProduction.ProudctionStart();
        }

        public void GameOver()
        {
            timeCounter.CountStop();
            player.ResetState();
            player.CheckActive(false);
            player.EnableWheelEffect(false);
            distance.RecordStop();
            scroll.ResetFlexible();
            obstacle.CheckActive(false);
            obstacle.ResetState();
            boostGuage.ChargeStop();
            heartRate.AverageCheckStop();

            //랭킹 기록 저장
            rankingDataHolder.rankingData.ranking_Obstacle.Add(RankingData.GetUserName(), RankingData.GetDate(), timeCounter.CurrentTimeScore, distance.CurrentDistanceRecord);
            rankingDataHolder.Write();

            //결과UI 등장
            resultPage.SetTextInfo(
                ((int)distance.CurrentDistanceRecord).ToString(),
                TimeCounter.FormatTime(timeCounter.CurrentTimeScore),
                ((int)heartRate.Average).ToString(),
                "0"
                );
            resultPage.Active(true);
        }

        [ContextMenu("Start")]
        public void GameStart()
        {
            playerHP.PlusHP(3);
            playerHP.StartAutoHpUByDistance();
            scroll.ScrollStart();
            timeCounter.CountStart();
            player.CheckActive(true);
            player.EnableWheelEffect(true);
            distance.RecordStart();
            obstacle.CheckActive(true);
            boostGuage.ChargeStart();
        }

        [ContextMenu("Pause")]
        public void GamePause()
        {
            scroll.ScrollStop();
            timeCounter.CountPause();
        }
    }
}