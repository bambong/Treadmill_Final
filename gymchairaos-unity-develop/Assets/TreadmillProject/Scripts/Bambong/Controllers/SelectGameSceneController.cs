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
            Managers.Sound.PlayBGM("bgm_GameSelect");
        }
        public void OnLobbyButtonDown()
        {
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.SelectMenu_ZB);
        }
        public void OnSpeedGameStartButtonDown()
        {
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.Speed_MenuScene);
        }
        public void OnObstacleGameStartButtonDown()
        {
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.Obstacle_GameScene_ZB_V2);
        }
        public void OnBalanceGameStartButtonDown()
        {
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.Balance_MenuScene);
        }

    }

}
