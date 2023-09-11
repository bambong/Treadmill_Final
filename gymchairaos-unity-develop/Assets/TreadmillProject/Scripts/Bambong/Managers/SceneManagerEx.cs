using bambong;
using Gymchair.Core.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum E_SceneName
{
    Login,
    Join,
    Modify,
    Assessment,
    Record,
    Result,
    TargetResult,
    SelectGame,
    SelectMenu,
    Speed_GameScene,
    Speed_MenuScene,
    Obstacle_GameScene,
    Obstacle_GameScene_ZB_V2,
    Balance_MenuScene,
    Balance_GameScene_Tutorial,
    Balance_GameScene_TutorialAsk,
    Balance_GameScene_1,
    Balance_GameScene_2,
    Balance_GameScene_3,
    Balance_GameScene_4,
    Balance_GameScene_5,
    Balance_GameScene_6
}

public class SceneManagerEx
{

    private bambong.TransitionController transitionController;
    private bambong.TransitionController Transition 
    { 
        get 
        {
            if(transitionController == null) 
            {
                GenerateTransition();
            }
            return transitionController;          
        } 
    }
    private void GenerateTransition() 
    {
        var go =  GameObject.Instantiate(Resources.Load("Transition"));
        GameObject.DontDestroyOnLoad(go);
        transitionController = go.GetComponent<TransitionController>();
    }
    public void Init() 
    {
        
    
    }

    public void LoadScene(E_SceneName scene , Action success = null) 
    {
        Transition.SceneTransition(scene.ToString(), success);
    }
 

    public void LoadSceneAsync(E_SceneName scene , Action success = null)
    {

        Managers.MonoForCoroutine.StartCoroutine(OnLoadScene(scene.ToString(), success));
    }

    IEnumerator OnLoadScene(string name, Action success)
    {
        var async = SceneManager.LoadSceneAsync(name);
        yield return async;

        success?.Invoke();
    }

   
   
}
