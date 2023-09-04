using bambong;
using Gymchair.Core.Mgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenuSceneController : MonoBehaviour
{
    public void OnGameSelectButtonActive() 
    {
        Managers.Scene.LoadScene(E_SceneName.SelectGame);
    }
    public void OnOriginTestButtonActive()
    {
        Managers.Scene.LoadScene(E_SceneName.Balance_MenuScene);
    }
    public void OnLobbyButtonActive()
    {
        Managers.Scene.LoadScene(E_SceneName.Login);
    }
}
