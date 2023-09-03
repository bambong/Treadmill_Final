using bambong;
using Gymchair.Core.Mgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenuSceneController : MonoBehaviour
{
    public void OnGameSelectButtonActive() 
    {
        TransitionManager.Instance.SceneTransition(E_SceneName.SelectGame.ToString(), () =>
        {
            SoundMgr.Instance.StopBGM();
        });
    }
    public void OnOriginTestButtonActive()
    {
        TransitionManager.Instance.SceneTransition(E_SceneName.Balance_MenuScene.ToString(), () =>
        {
            SoundMgr.Instance.StopBGM();
        });
    }
    public void OnLobbyButtonActive()
    {
        TransitionManager.Instance.SceneTransition(E_SceneName.Login.ToString());
    }
}
