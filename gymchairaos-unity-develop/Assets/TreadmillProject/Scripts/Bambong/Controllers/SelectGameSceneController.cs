using Gymchair.Core.Mgr;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace bambong 
{
    public class SelectGameSceneController : MonoBehaviour
    {
        private void Start()
        {
            SoundMgr.Instance.PlayBGM("bgm_GameSelect");
        }
        public void OnLobbyButtonDown()
        {
            //SceneMgr.Instance.UnLoadSceneAsync(E_SceneName.SelectGame.ToString(), () =>
            //{
            //    SceneMgr.Instance.LoadSceneAsync("Login", LoadSceneMode.Additive);
            //});
            SoundMgr.Instance.PlayEffect("touch");
            TransitionManager.Instance.SceneTransition("Login");
            //TransitionManager.Instance.SceneTransition();
        }
        public void OnSpeedGameStartButtonDown()
        {
            SoundMgr.Instance.PlayEffect("touch");
            TransitionManager.Instance.SceneTransition(E_SceneName.Speed_MenuScene.ToString());
        }
        public void OnObstacleGameStartButtonDown()
        {
            SoundMgr.Instance.PlayEffect("touch");
            TransitionManager.Instance.SceneTransition(E_SceneName.Obstacle_GameScene_ZB_V2.ToString());
        }
        public void OnBalanceGameStartButtonDown()
        {
            SoundMgr.Instance.PlayEffect("touch");
            TransitionManager.Instance.SceneTransition(E_SceneName.Balance_Game.ToString());
        }

    }

}
