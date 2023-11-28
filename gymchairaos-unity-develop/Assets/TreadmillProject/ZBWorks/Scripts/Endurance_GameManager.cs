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

        public string Dist { get => moveDist.GetString(); }

        [SerializeField] GameStartProduction gameStartProduction;
        [SerializeField] ObstacleEvent obstacle;
        [SerializeField] HeartRate heartRate;
        [SerializeField] PlayerInputManager2 player;
        [SerializeField] ObjectsScrolling scroll;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] PlayerHP playerHP;
        [SerializeField] BoostGuage boostGuage;
        [SerializeField] ResultPageController resultPage;
        [SerializeField] RankingDataHolder rankingDataHolder;
        [SerializeField] DistanceRecord distanceRecord;
        [SerializeField] SpeedShow speedShow;
        MoveDist moveDist = new MoveDist();
        CalorieCheck calorieCheck = new CalorieCheck();

        [Space]
        [SerializeField] float startDelay;
        WaitForSeconds startDelay_WFS; 

        private void Awake()
        {
            Instance = this;
            startDelay_WFS = new WaitForSeconds(startDelay);
        }
        private void Start()
        {
            //거리 측정 콜백 추가
            Managers.Token.ReceivedEvent += moveDist.MoveDistUpdate;
            Managers.Token.ReceivedEvent += DistanceTextUpdate;
            //속도 측정 콜백 추가
            Managers.Token.ReceivedEvent += SpeedTextUpdate;

            StartCoroutine(BloothConnectWait());
        }
        private void Update()
        {
            calorieCheck.Update();
#if UNITY_EDITOR
            speedShow.SpeedTextUpdate(player.FocusPower, string.Format("{0:0.00}", player.FocusPower));
#endif
        }
        private void OnDestroy()
        {
            //거리 측정 콜백 제거
            Managers.Token.ReceivedEvent -= moveDist.MoveDistUpdate;
            Managers.Token.ReceivedEvent -= DistanceTextUpdate;

            //속도 측정 콜백 제거
            Managers.Token.ReceivedEvent -= SpeedTextUpdate;
        }
        private void DistanceTextUpdate()
        {
            distanceRecord.UpdateText((float)moveDist.movedDist, moveDist.ToString());
        }
        private void SpeedTextUpdate()
        {
            speedShow.SpeedTextUpdate((float)Managers.Token.CurSpeedMeterPerSec, string.Format("{0:0.00}", Managers.Token.CurSpeedMeterPerSec));
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
            scroll.ResetFlexible();
            obstacle.CheckActive(false);
            obstacle.ResetState();
            boostGuage.ChargeStop();
            heartRate.AverageCheckStop();

            //랭킹 기록 저장
            rankingDataHolder.rankingData.ranking_Obstacle.Add(
                RankingData.GetUserName(),
                RankingData.GetDate(),
                timeCounter.CurrentTimeScore, 
                (float)moveDist.movedDist);
            rankingDataHolder.Write();

            //결과UI 등장
            resultPage.SetTextInfo(
                $"{moveDist.GetString()} M",
                TimeCounter.FormatTime(timeCounter.CurrentTimeScore),
                $"{heartRate.GetString()} Bpm",
                $"{calorieCheck.GetString()} Kcal"
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