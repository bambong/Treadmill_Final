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
    public enum E_SceneName 
    {
        Login,
        SelectGame,
        Speed_GameScene,
        Speed_MenuScene,
        Obstacle_GameScene,
        Obstacle_GameScene_ZB_V2,
        Balance_Game
    }
    public class TransitionManager : GameObjectSingleton<TransitionManager>, IInit
    {
        [SerializeField]
        private Animator animator;

        private readonly float MinimumTime = 1f;
        private readonly string AnimTrigger = "IsFadeOut";
        private float curTransitionTime = 0f;
        private bool isTranstionOn = false;
        private string curScenename = E_SceneName.Login.ToString();

        public void OnSceneLoadSuccsess()
        {
            
        }
        public void SceneTransition(string sceneName, Action onAnimEnd = null)
        {
            //StartCoroutine(Transition(sceneName));
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Init"));
            
            StartCoroutine(UnLoad(sceneName, onAnimEnd));
            //SceneMgr.Instance.UnLoadSceneAsync(curScenename, () =>
            //{
            //    Transitioner.Instance.TransitionInWithoutChangingScene();
            //    SceneMgr.Instance.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            //});
            //curScenename = sceneName;

            //if (isTranstionOn)
            //{
            //    return;
            //}
            //StartCoroutine(Transition(sceneName));
        }
        IEnumerator UnLoad(string sceneName, Action action = null)
        {
            Transitioner.Instance.TransitionOutWithoutChangingScene();
            yield return new WaitForSeconds(1f);
            action?.Invoke();
            var cur = curScenename;
            SceneMgr.Instance.UnLoadSceneAsync(cur, () =>
            {
                //Transitioner.Instance.TransitionInWithoutChangingScene();
                SceneMgr.Instance.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                  //, () => { Transitioner.Instance.TransitionInWithoutChangingScene(); });
            });
            curScenename = sceneName;
        }
        IEnumerator Transition(string sceneName)
        {

            var sceneLoad = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = false;
            isTranstionOn = true;
            //animator.SetBool(AnimTrigger,true);
            Transitioner.Instance.TransitionOutWithoutChangingScene();
            while (sceneLoad.progress < 0.9f || curTransitionTime < MinimumTime)
            {
                curTransitionTime += Time.deltaTime;
                yield return null;
            }
            sceneLoad.allowSceneActivation = true;
            yield return SceneManager.UnloadSceneAsync(curScenename);

            while(!sceneLoad.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            Transitioner.Instance.TransitionInWithoutChangingScene();
            //animator.SetBool(AnimTrigger,false);
            curTransitionTime = 0f;
            isTranstionOn = false;
            curScenename = sceneName;

        }

        public void TransitionToSelectScene() 
        {
            StartCoroutine(Transition(E_SceneName.SelectGame.ToString()));
        }
        public void Init()
        {
            DontDestroyOnLoad(this);
        }
        public void Awake()
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}

