using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gymchair.Core.Mgr;

namespace bambong 
{
    public enum E_SceneName 
    {
        SelectGame,
        Speed_GameScene,
        Speed_MenuScene,
        Obstacle_GameScene
    }
    public class TransitionManager : GameObjectSingleton<TransitionManager>, IInit
    {
        [SerializeField]
        private Animator animator;

        private readonly float MinimumTime = 1f;
        private readonly string AnimTrigger = "IsFadeOut";
        private float curTransitionTime = 0f;
        private bool isTranstionOn = false;
        private string curScenename = E_SceneName.SelectGame.ToString();
        public void SceneTransition(string sceneName)
        {
            animator.SetBool(AnimTrigger, true);
            SceneMgr.Instance.UnLoadSceneAsync(curScenename, () =>
            {
                animator.SetBool(AnimTrigger, false);
                SceneMgr.Instance.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
               
            });
            curScenename = sceneName;
            //if (isTranstionOn)
            //{
            //    return;
            //}
            //StartCoroutine(Transition(sceneName));
        }


        IEnumerator Transition(string sceneName)
        {

            var sceneLoad = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = false;
            isTranstionOn = true;
            animator.SetBool(AnimTrigger,true);
            while(sceneLoad.progress < 0.9f || curTransitionTime < MinimumTime)
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
            animator.SetBool(AnimTrigger,false);
            curTransitionTime = 0f;
            isTranstionOn = false;
            curScenename = sceneName;

        }

        public void TransitionToSelectScene() 
        {
            Transition(E_SceneName.SelectGame.ToString());
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

