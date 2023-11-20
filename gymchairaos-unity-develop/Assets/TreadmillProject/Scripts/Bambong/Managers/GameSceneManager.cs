
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Gymchair.Core.Mgr;

namespace bambong 
{
    public class GameSceneManager : GameObjectSingletonDestroy<GameSceneManager>, IInit
    {
        #region SerializeField

        public float TimeText { get => curTime; }
        public int DistText { get => (int)curDistance; }

        [Header("Player")]
        [SerializeField]
        private PlayerController player;

        [Header("Gauage")]
        [SerializeField]
        private SpeedGuage speedGuage;
        [SerializeField]
        private float maximumGauage = 100f;

        [SerializeField]
        private float decreaseGauageAmount = 0.1f;
        [SerializeField]
        private float increaseGauageAmount = 1f;

        [SerializeField]
        private float curRoadDistance = 1000f;
        [SerializeField]
        private DestinationController destinationController;
        [SerializeField]
        private float minmumSpeedRatio = 0.01f;

        [SerializeField]
        private SpeedRevisionData speedRevisionData;

        [SerializeField]
        private LevelData levelData;

        [Header("Effect FX")]
        [SerializeField]
        private GameObject directionFx;
        [SerializeField]
        private GameObject clearFx;
        [SerializeField]
        private GameObject failFx;
        [SerializeField]
        private GameObject playerArrowFx;

        [Header("ForRecord")]
        [SerializeField]
        private HeartRate heartRate;

        [Header("Ranking")]
        [SerializeField]
        private RankingDataHolder rankingDataHolder;

        #endregion SerializeField

        #region NonSerializeField 

        private GameStateController gameStateController;
        private float curGauage = 0;
        private float curDistance = 0;
        private float curTime = 0;
        private bool isInit;
        private Action onGameStart;
        private Action onGameClear;
        #endregion NonSerializeField

        #region Property
        public GameStateController GameStateController { get { return gameStateController; } }
        public float CurGauage { get { return curGauage; } }
        public PlayerController Player {get { return player; } }
        public float CurRoadDistance { get { return curRoadDistance; } }
        public float MaxmumGauage {get { return maximumGauage; } }

        public DestinationController DestinationController { get => destinationController; }
        public float CurTime { get => curTime; }
        public Action OnGameClear { get => onGameClear; set => onGameClear = value; }
        public Action OnGameStart { get => onGameStart; set => onGameStart = value; }
        #endregion Property

        #region ReadOnly
        private readonly float WHEEL_PERIMETER = 1.8f;
        private readonly float LEVEL_DISTANCE_RATIO = 2;
        #endregion ReadOnly
        public void Init()
        {
            if(isInit)
            {
                return;
            }
            Application.targetFrameRate = 60;
            isInit = true;
            curRoadDistance = GetCurLevelInfo().distance * LEVEL_DISTANCE_RATIO;
            gameStateController = new GameStateController(this);
        }
        private void Awake()
        {
            Init();
            //Managers.Token.ConnectToDevice();

            //Managers.Token.AddRightTokenEvent(IncreaseGauage);
            //Managers.Token.AddLeftTokenEvent(IncreaseGauage);

#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayBGM("bgm_Speed");
#endif
        }
        /*
        public void IncreaseGauage()
        {
            if(!gameStateController.IsStateGamePlaying())
            {
                return;
            }
            float factor =5;
#if !UNITY_EDITOR
            factor = Managers.Token.CurRpm ;
#endif
            Debug.Log($"게이지 상승 + {increaseGauageAmount * factor}");
            curGauage = Mathf.Clamp(curGauage +  increaseGauageAmount * factor, 0 , maximumGauage);
        }
        */
        public bool CheckSpeedGageOring()
        {
            return Managers.Token.CurSpeedMeterPerSec < 0.2f;
        }
        private void Update()
        {
            GameStateController.Update();
        }
        /*
        private void DecreaseGauage()
        {
            var decGauage = curGauage - decreaseGauageAmount * Time.deltaTime;
            curGauage = Mathf.Max(0,decGauage);
        }
        */
        private void UpdateTime() 
        {
            curTime += Time.deltaTime;
        }
        public void AddDistance() 
        {
            curDistance += curGauage * Time.deltaTime * WHEEL_PERIMETER;

            speedGuage.OnPlayerMoved(GetCurDistanceRatio(player.transform));
        }

        //게임 시작
        public void OnStart()
        {
            UISceneManager.Instance.CommonPanelOpen();
            player.SetStateIdle();

            directionFx.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(3f);
            sequence.AppendCallback(() => { directionFx.SetActive(false); });
            sequence.Play();
            playerArrowFx.SetActive(true);
            player.ArrowAnimStart();

            heartRate.AverageCheckStart();

            SetStateGamePlay();
        }
        public float GetCurSpeed()
        {
            return curGauage/increaseGauageAmount * WHEEL_PERIMETER;
        }
        public float GetGaugeRatio() => CurGauage / maximumGauage;
        public float GetCurDistanceRatio(Transform transform)
        {
            return  (CurRoadDistance -(DestinationController.transform.position.z - transform.position.z))/ CurRoadDistance;
        }
        public float GetAIAnimSpeedRevision() 
        {
            return MaxmumGauage / player.SpeedRatio * player.MoveSpeed;
        }
        public void SetDestination() 
        {
           var pos  = DestinationController.transform.position;
           pos.z = player.transform.position.z + curRoadDistance;
           DestinationController.transform.position = pos;
           DestinationController.gameObject.SetActive(true);
        }
        public LevelInfo GetCurLevelInfo() 
        {
            return levelData.levelInfos[GameManager.Instance.CurrentLevel];
        }
        public void UpdateOnStateGamePlay()
        {
            UISceneManager.Instance.UI_UpdateOnPlay();
            UpdateTime();
            //DecreaseGauage();

        }
#region SetState
        public void SetStateGameStart() 
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(E_SceneName.Speed_GameScene.ToString()));
            gameStateController.ChangeState(GameStart.Instance);
            SetDestination();
            UISceneManager.Instance.WaitPanelOpen(OnStart);

            speedGuage.Init();
        }
        public void SetStateGamePlay()
        {
            OnGameStart?.Invoke();
            gameStateController.ChangeState(GamePlay.Instance);
            AIControlManager.Instance.SetStateAIsRun();
        }
        public void SetStateGamePause() 
        {
            gameStateController.ChangeState(GamePause.Instance);
            AIControlManager.Instance.SetStateAIsStop();
        }

        //플레이어가 먼저 지나갔을 때 호출
        public void SetStateGameClear()
        {
            OnGameClear?.Invoke();
            gameStateController.ChangeState(GameClear.Instance);
            //player.SetStateNone();
            GameManager.Instance.ClearLevel(GameManager.Instance.CurrentLevel);
           // AIControlManager.Instance.SetStateAIsStop();
            UISceneManager.Instance.EndPanelOpen();

            clearFx.SetActive(true);
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.StopBGM();
            Managers.Sound.PlayEffect("sfx_Speed_Clear");
#endif
            heartRate.AverageCheckStop();

            //랭킹 저장
            rankingDataHolder.rankingData.ranking_Speed.Add(RankingData.GetUserName(), RankingData.GetDate(), curTime, (int)curDistance);
            rankingDataHolder.Write();
        }

        //AI가 먼저 지나갔을 때 호출
        public void SetStateGameOver()
        {
            OnGameClear?.Invoke();
            gameStateController.ChangeState(GameOver.Instance);
            //player.SetStateNone();
            playerArrowFx.SetActive(false);
            failFx.SetActive(true);
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.StopBGM();
            Managers.Sound.PlayEffect("sfx_Speed_GameOver");
#endif
            //AIControlManager.Instance.SetStateAIsStop();
            heartRate.AverageCheckStop();

            UISceneManager.Instance.EndPanelOpen();
        }
#endregion SetState

#region ease
        protected float easeInQuad(float start,float end,float value)
        {
            end -= start;
            return end * value * value + start;
        }
        protected float easeOutQuad(float start,float end,float value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        protected float easeOutQuart(float start,float end,float value)
        {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

       
#endregion ease
    }

}

