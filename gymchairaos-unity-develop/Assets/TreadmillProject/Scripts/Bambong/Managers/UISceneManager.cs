using Gymchair.Core.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace bambong
{
    public class UISceneManager : GameObjectSingletonDestroy<UISceneManager>, IInit
    {

        [SerializeField]
        private CommonPanelController commonPanel;
        [SerializeField]
        private PausePanelController pausePanel;
        [SerializeField]
        private WaitPanelController waitPanel;
        [SerializeField]
        private ExitPanelController exitPanel;
        [SerializeField]
        private EndPanelController endPanel;
        [SerializeField]
        private ClearPanelController clearPanel;
        [SerializeField]
        private DestinationUIController destinationUI;
        [SerializeField]
        private HeartRate heartRate;

        public void Init()
        {
            destinationUI.Init();
        }
        public void EndPanelOpen() => endPanel.Open();
        public void EndPanelClose() => endPanel.Close();
        public void CommonPanelOpen() => commonPanel.Open();
        public void CommonPanelClose() => commonPanel.Close();
        public void ClearPanelOpen() 
        {
            clearPanel.SetInfo(
                GameSceneManager.Instance.DistText.ToString(),
                GetTimeText(),
                ((int)heartRate.Average).ToString(),
                "0"
                );

            clearPanel.Open(); 
        } 

        public void UI_UpdateOnPlay()
        {
            destinationUI.UpdateActive();
            commonPanel.UpdateTimeText(GetTimeText());
            commonPanel.UpdateSpeedText(GetSpeedText());
        }
        public void WaitPanelOpen(Action callOnStart)
        {
            commonPanel.Close();
            waitPanel.Open(callOnStart);
        }
        public void OnClickExitbutton()
        {
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayTouchEffect();
#endif
            GameSceneManager.Instance.SetStateGamePause();
            pausePanel.Open();
            exitPanel.Open();
        }
        public void RevertExit()
        {
            pausePanel.Close();
            exitPanel.Close();
        }
        private string GetTimeText()
        {
            float curTime = GameSceneManager.Instance.TimeText;
            string curMin = ((int)curTime / 60).ToString("00");
            curTime %= 60;
            return curMin + ":" + curTime.ToString("00.00").Replace('.', ':');
        }
        private string GetSpeedText()
        {
            var curGuage = Managers.Token.CurSpeedMeterPerSec;
          
            return curGuage.ToString("00") + " m/s";
        }
    }
}
