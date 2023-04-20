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
        [SerializeField] PlayerInputManager2 player;
        [SerializeField] ObjectsScrolling scroll;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] PlayerHP playerHP;

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
            while(!TokenInputManager.Instance.IsConnect)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(E_SceneName.Obstacle_GameScene_ZB_V2.ToString()));
            GameStartProduction_Start();
        }

        IEnumerator GameStart_C;
        IEnumerator GameStartC()
        {
            GameOver();
            yield return startDelay_WFS;
            playerHP.PlusHP(3);
            GameStartProduction_Start();
        }

        public void OnPlayerHpZero()
        {
            GameOver();
        }
        public void GameRestart()
        {
            if (GameStart_C != null)
                StopCoroutine(GameStart_C);
            GameStart_C = GameStartC();
            StartCoroutine(GameStart_C);
        }

        public void GameStartProduction_Start()
        {
            player.MoveFront_OnRebirth();
            gameStartProduction.ProudctionStart();
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
        }

        [ContextMenu("Pause")]
        public void GamePause()
        {
            scroll.ScrollStop();
            timeCounter.CountPause();
        }

        [ContextMenu("GameOver")]
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
        }
    }
}