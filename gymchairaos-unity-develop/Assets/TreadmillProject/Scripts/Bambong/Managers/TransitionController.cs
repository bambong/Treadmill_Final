using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gymchair.Core.Mgr;
using System;

namespace bambong 
{
    public class TransitionController : MonoBehaviour
    {
        private readonly float MinimumTime = 1f;
        private float curTransitionTime = 0f;
        private bool isTranstionOn = false;


        public void SceneTransition(string sceneName , Action success = null)
        {
            if (isTranstionOn)
            {
                return;
            }

            StartCoroutine(Transition(sceneName,success));
        }
    
        IEnumerator Transition(string sceneName, Action success = null)
        {
            isTranstionOn = true;
            
            var sceneLoad = SceneManager.LoadSceneAsync(sceneName);
            sceneLoad.allowSceneActivation = false;
            
            Transitioner.Instance.TransitionOutWithoutChangingScene();
            
            while (sceneLoad.progress < 0.9f || curTransitionTime < MinimumTime)
            {
                curTransitionTime += Time.deltaTime;
                yield return null;
            }
            sceneLoad.allowSceneActivation = true;
 

            while(!sceneLoad.isDone)
            {
                yield return null;
            }
   
            //Transitioner.Instance.TransitionInWithoutChangingScene();
            curTransitionTime = 0f;
            
            success?.Invoke();

            isTranstionOn = false;

        } 
    }

}

