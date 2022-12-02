using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace bambong
{
    public class ExitPanelController : PanelController
    {

        public void OnClickYesButton()
        {
            UISceneManager.Instance.RevertExit();
            TransitionManager.Instance.SceneTransition(E_SceneName.Speed_MenuScene.ToString());
        }
        public void OnClickNoButton()
        {
            GameSceneManager.Instance.SetStateGamePlay();
            UISceneManager.Instance.RevertExit();
        }
    }
}
