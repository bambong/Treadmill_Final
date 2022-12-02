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

        private readonly int WaitTime = 3;
        private readonly string BeforeStartText = "Ω√¿€";
        private readonly float BeforeStartWaitTime = 0.5f;
        private readonly float OriginFontSize = 300f;
        private readonly float BeforeStartFontSize = 250f;

        private int curWaitTime;
        public void Open(Action callOnClose) 
        {
            curWaitTime = WaitTime;
            waitText.fontSize = OriginFontSize;
            SetWaitTimeText();
            gameObject.SetActive(true);
            StartCoroutine(ProcessWait(callOnClose));
        }
        private void SetWaitTimeText() 
        {
            waitText.text = curWaitTime.ToString();
        }

        IEnumerator ProcessWait(Action callOnClose) 
        {
            while(true) 
            {
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