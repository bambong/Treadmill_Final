using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace bambong 
{
    public class GameSceneManager : GameObjectSingletonDestroy<GameSceneManager>, IInit
    {
        #region SerializeField

        [Header("Player")]
        [SerializeField]
        private PlayerController player;

        [Header("Gauage")]
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

        #endregion SerializeField

        #region NonSerializeField 

        private GameStateController gameStateController;
        private float curGauage = 0;
        private float curDistance = 0;
        private float curTime = 0;
        private bool isInit;
        #endregion NonSerializeField

        #region Property
        public GameStateController GameStateController { get { return gameStateController; } }
        public float CurGauage { get { return curGauage; } }
        public PlayerController Player {get { return player; } }
        public float CurRoadDistance { get { return curRoadDistance; } }
        public float MaxmumGauage {get { return maximumGauage; } }

        public DestinationController DestinationController { get => destinationController; }
        public float CurTime { get => curTime; }

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
            isInit = true;
            curRoadDistance = GetCurLevelInfo().distance * LEVEL_DISTANCE_RATIO;
            gameStateController = new GameStateController(this);
        }
        private void Awake()
        {
            Init();
           /// TokenInputManager.Instance.ConnectToDevice();
            //TokenInputManager.Instance.AddRightTokenEvent(IncreaseGauage);
            //TokenInputManager.Instance.AddLeftTokenEvent(IncreaseGauage);
            TokenInputManager.Instance.ReceivedEvent += IncreaseGauage;
        }

        public void IncreaseGauage()
        {
            if(!gameStateController.IsStateGamePlaying())
            {
                return;
            }
            curGauage = Mathf.Min(curGauage + TokenInputManager.Instance.CurRpm * increaseGauageAmount * 0.005f, maximumGauage);
        }
        public bool CheckSpeedGageOring() => curGauage <= 0;
        private void Update()
        {
            GameStateController.Update();
        }

        private void DecreaseGauage()
        {
            var decGauage = curGauage - decreaseGauageAmount * Time.deltaTime;
            curGauage = Mathf.Max(0,decGauage);
        }
        private void UpdateTime() 
        {
            curTime += Time.deltaTime;
        }
        public void AddDistance() 
        {
            curDistance += curGauage * Time.deltaTime * WHEEL_PERIMETER;
        }

        public void OnStart()
        {
            UISceneManager.Instance.CommonPanelOpen();
            player.SetStateIdle();

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
            DecreaseGauage();

        }
        #region SetState
        public void SetStateGameStart() 
        {
            gameStateController.ChangeState(GameStart.Instance);
            SetDestination();
            UISceneManager.Instance.WaitPanelOpen(OnStart);
        }
        public void SetStateGamePlay()
        {
            gameStateController.ChangeState(GamePlay.Instance);
            AIControlManager.Instance.SetStateAIsRun();
        }
        public void SetStateGamePause() 
        {
            gameStateController.ChangeState(GamePause.Instance);
            AIControlManager.Instance.SetStateAIsStop();
        }
        public void SetStateGameClear() 
        {
            gameStateController.ChangeState(GameClear.Instance);
            player.SetStateNone();
            GameManager.Instance.ClearLevel(GameManager.Instance.CurrentLevel);
            AIControlManager.Instance.SetStateAIsStop();
            UISceneManager.Instance.EndPanelOpen();
        }
        public void SetStateGameOver()
        {
            gameStateController.ChangeState(GameOver.Instance);
            player.SetStateNone();
            AIControlManager.Instance.SetStateAIsStop();
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

