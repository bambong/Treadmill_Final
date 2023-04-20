using DG.Tweening;
using Gymchair.Core.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace bambong
{
    public class WaitPanelController : PanelController
    {
        [SerializeField]
        private TextMeshProUGUI waitText;
        [SerializeField]
        private CanvasGroup canvasGroup;
        private readonly int WaitTime = 3;
        private readonly string BeforeStartText = "½ÃÀÛ";
        private readonly float BeforeStartWaitTime = 0.5f;
        private readonly float OriginFontSize = 300f;
        private readonly float BeforeStartFontSize = 250f;

        private int curWaitTime;
        private readonly float START_FADE_TIME = 0.5f;
        public void Open(Action callOnClose) 
        {
            curWaitTime = WaitTime;
            canvasGroup.alpha = 0;
            waitText.fontSize = OriginFontSize;
            waitText.transform.localScale = Vector3.zero;
            SetWaitTimeText();
            gameObject.SetActive(true);
            canvasGroup.DOFade(1, START_FADE_TIME).OnComplete(() => { StartCoroutine(ProcessWait(callOnClose)); });
            
        }
        private void SetWaitTimeText() 
        {
            waitText.text = curWaitTime.ToString();
        }
        private IEnumerator PlayReadytSoundEffect() 
        {
            yield return new WaitForSeconds(1f);
#if !UNITY_EDITOR || SOUND_TEST
            SoundMgr.Instance.PlayEffect("sfx_Speed_Ready");
#endif
        }
        IEnumerator ProcessWait(Action callOnClose) 
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(PlayReadytSoundEffect());
            while (true) 
            {
                waitText.transform.localScale = Vector3.zero;
                waitText.transform.DOScale(new Vector3(2, 2, 2), 0.5f);
                yield return new WaitForSeconds(1f);
                curWaitTime--;
                if(curWaitTime <= 0) 
                {
                    waitText.text = BeforeStartText;
                    waitText.fontSize = BeforeStartFontSize;
                    yield return new WaitForSeconds(BeforeStartWaitTime);
                    callOnClose?.Invoke();
                    Close();
                    break;
                }
                SetWaitTimeText();
            }
        }

      
    }
}