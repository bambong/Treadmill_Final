using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gymchair.Core.Mgr;
using System;
using DG.Tweening;
using TMPro;

namespace bambong 
{
    public class TransitionController : MonoBehaviour
    {

        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private GameObject openGo;
        [SerializeField]
        private TextMeshProUGUI loadingText;
        [SerializeField]
        private RectTransform loadingSpin;


        private readonly float MinimumTime = 1f;
        private float curTransitionTime = 0f;
        private bool isTranstionOn = false;
        private readonly string LOADTIN_TEXT = "화면을 불러오는 중입니다";
        private void Start()
        {
            openGo.gameObject.SetActive(false);
        }

        public void SceneTransition(string sceneName , Action success = null)
        {
            if (isTranstionOn)
            {
                return;
            }

            StartCoroutine(Transition(sceneName,success));
            StartCoroutine(LoadingTextCo());
        }
        IEnumerator LoadingTextCo()
        {
            int count = 0;
            int len = LOADTIN_TEXT.Length;
            while (isTranstionOn) 
            {
                loadingText.text = LOADTIN_TEXT.PadRight(len+count, '.');
                count++;
                count %= 4;
                yield return new WaitForSeconds(0.2f);
            }
        }
    
        IEnumerator Transition(string sceneName, Action success = null)
        {
            isTranstionOn = true;
           
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 0; 

            curTransitionTime = 0;

            openGo.SetActive(false);
           
            while (curTransitionTime < 1 )
            {
                curTransitionTime += Time.deltaTime * 2;
                canvasGroup.alpha =  curTransitionTime;
                yield return null;
            }
          
            canvasGroup.alpha = 1;

            openGo.SetActive(true);
            
            var sceneLoad = SceneManager.LoadSceneAsync(sceneName);
            sceneLoad.allowSceneActivation = false;
    
            Vector3 spinEuler = Vector3.zero;

            curTransitionTime = 0;
           
            while (sceneLoad.progress < 0.9f || curTransitionTime < MinimumTime)
            {
                spinEuler += Vector3.forward * -360 * Time.deltaTime;
                loadingSpin.rotation = Quaternion.Euler(spinEuler);
                curTransitionTime += Time.deltaTime;
           //     canvasGroup.alpha = curTransitionTime ;
                yield return null;
            }
            //canvasGroup.alpha = 1;
           
            sceneLoad.allowSceneActivation = true;
 
            while(!sceneLoad.isDone)
            {
                yield return null;
            }

            curTransitionTime = 0f;
            openGo.SetActive(false);

            while (curTransitionTime < 1)
            {
                curTransitionTime += Time.deltaTime;
                canvasGroup.alpha = 1 - curTransitionTime;
                yield return null;
            }
            canvasGroup.alpha = 0;

            //Transitioner.Instance.TransitionInWithoutChangingScene();
  
            success?.Invoke();


            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            isTranstionOn = false;

        } 
    }

}

