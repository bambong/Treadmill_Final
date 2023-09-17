using Gymchair.Core.Mgr;
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
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayTouchEffect();
#endif
            UISceneManager.Instance.RevertExit();
            Managers.Scene.LoadScene(E_SceneName.Speed_MenuScene);
        }
        public void OnClickNoButton()
        {
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayTouchEffect();
#endif
            GameSceneManager.Instance.SetStateGamePlay();
            UISceneManager.Instance.RevertExit();
        }
    }
}
