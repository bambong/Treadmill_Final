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
            SoundMgr.Instance.PlayEffect("touch");
#endif
            UISceneManager.Instance.RevertExit();
            TransitionManager.Instance.SceneTransition(E_SceneName.Speed_MenuScene.ToString());
        }
        public void OnClickNoButton()
        {
#if !UNITY_EDITOR || SOUND_TEST
            SoundMgr.Instance.PlayEffect("touch");
#endif
            GameSceneManager.Instance.SetStateGamePlay();
            UISceneManager.Instance.RevertExit();
        }
    }
}
