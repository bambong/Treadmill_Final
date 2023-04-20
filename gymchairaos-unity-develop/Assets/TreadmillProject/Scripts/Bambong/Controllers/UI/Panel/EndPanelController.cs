using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class EndPanelController : PanelController
    {

        [SerializeField]
        private CanvasGroup canvasGroup;
        private readonly float VISIBLE_TIME = 0.5f;

        public override void Open()
        {
            Time.timeScale = 0.2f;
            
            canvasGroup.alpha = 0;
            base.Open();
            StartCoroutine(WaitClose());
        }

        IEnumerator WaitClose() 
        {
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
            yield return new WaitForSeconds(1f);
            canvasGroup.DOFade(1, 2f);
            yield return new WaitForSeconds(2f + VISIBLE_TIME);
            Close();
        
        }

        public override void Close()
        {
           
            base.Close();
            UISceneManager.Instance.ClearPanelOpen();
        }



    }

}
