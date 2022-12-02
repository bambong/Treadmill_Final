using Gymchair.Core.Mgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace bambong 
{
    public class SelectGameSceneController : MonoBehaviour
    {
        public void OnLobbyButtonDown()
        {
            SceneMgr.Instance.UnLoadSceneAsync(E_SceneName.SelectGame.ToString(), () =>
            {
                SceneMgr.Instance.LoadSceneAsync("Login", LoadSceneMode.Additive);
            });
            //TransitionManager.Instance.SceneTransition();
        }
        public void OnSpeedGameStartButtonDown()
        {
            TransitionManager.Instance.SceneTransition(E_SceneName.Speed_MenuScene.ToString());
        }
        public void OnObstacleGameStartButtonDown()
        {
            TransitionManager.Instance.SceneTransition(E_SceneName.Obstacle_GameScene.ToString());
        }

    }

}
