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
        [SerializeField] ObstacleTutorial tutorial;

        MoveDist moveDist = new MoveDist();
        CalorieCheck calorieCheck = new CalorieCheck();

        [Space]
        [SerializeField] float startDelay;
        WaitForSeconds startDelay_WFS; 

        public void GameRestart()
        {
            Managers.Scene.LoadScene(E_SceneName.Obstacle_GameScene_ZB_V2);
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

            //��ŷ ��� ����
            rankingDataHolder.rankingData.ranking_Obstacle.Add(
                RankingData.GetUserName(),
                RankingData.GetDate(),
                timeCounter.CurrentTimeScore, 
                (float)moveDist.movedDist);
            rankingDataHolder.Write();

            //���UI ����
            resultPage.SetTextInfo(
                $"{moveDist.GetString()} M",
                TimeCounter.FormatTime(timeCounter.CurrentTimeScore),
                $"{heartRate.GetString()} Bpm",
                $"{calorieCheck.GetString()} Kcal"
                );
            resultPage.Active(true);
        }

        private void Awake()
        {
            Instance = this;
            startDelay_WFS = new WaitForSeconds(startDelay);
        }
        private void Start()
        {
            StartCoroutine(BloothConnectWait());
            gameStartProduction.AddProductionEndEvent(BaseTokenCallback);
            gameStartProduction.AddProductionEndEvent(GameStart);
            tutorial.AddEvent_GameStart(ProductionStart);

            scroll.ScrollStart();
            player.EnableWheelEffect(true);
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
            //�Ÿ� ���� �ݹ� ����
            Managers.Token.ReceivedEvent -= moveDist.MoveDistUpdate;
            Managers.Token.ReceivedEvent -= DistanceTextUpdate;

            //�ӵ� ���� �ݹ� ����
            Managers.Token.ReceivedEvent -= SpeedTextUpdate;
        }
        private void DistanceTextUpdate()
        {
            distanceRecord.UpdateText((float)moveDist.movedDist, moveDist.GetString());
        }
        private void SpeedTextUpdate()
        {
            speedShow.SpeedTextUpdate((float)Managers.Token.CurSpeedMeterPerSec, string.Format("{0:0.00}", Managers.Token.CurSpeedMeterPerSec));
        }
        private void BaseTokenCallback()
        {
            //�Ÿ� ���� �ݹ� �߰�
            Managers.Token.ReceivedEvent += moveDist.MoveDistUpdate;
            Managers.Token.ReceivedEvent += DistanceTextUpdate;
            //�ӵ� ���� �ݹ� �߰�
            Managers.Token.ReceivedEvent += SpeedTextUpdate;
        }

        private void TutorialCheckAndStart()
        {
            //Ʃ�丮�� ������� Ȯ��
            Ranking_Obstacle rankings = rankingDataHolder.rankingData.ranking_Obstacle;

            for (int i = 0; i < rankings.datas.Length; i++)
            {
                if (rankings.datas[i].name == Managers.Data.UserName)
                {
                    //���ƴ� ���, ���� ����
                    ProductionStart();
                    return;
                }
            }

            //����� ���, Ʃ�丮�� �� ������ ���
            tutorial.TutorialAsk();
        }
        private void ProductionStart()
        {
            player.CheckActive(false);
            player.MoveFront_OnRebirth();
            gameStartProduction.ProudctionStart();
        }
        private void GameStart()
        {
            playerHP.PlusHP(3);
            playerHP.StartAutoHpUByDistance();
            timeCounter.CountStart();
            player.CheckActive(true);
            obstacle.CheckActive(true);
            boostGuage.ChargeStart();
        }

        IEnumerator BloothConnectWait() 
        {
            while(!Managers.Token.IsConnect)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(E_SceneName.Obstacle_GameScene_ZB_V2.ToString()));
            TutorialCheckAndStart();
        }
        IEnumerator GameStart_C;
        IEnumerator GameStartC()
        {
            yield return startDelay_WFS;
            playerHP.PlusHP(3);
            ProductionStart();
        }



        [ContextMenu("Pause")]
        public void GamePause()
        {
            scroll.ScrollStop();
            timeCounter.CountPause();
        }
    }
}