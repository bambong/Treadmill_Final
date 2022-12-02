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
            clearPanel.SetTimeText(GetTimeText());
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
            float curTime = GameSceneManager.Instance.CurTime;
            return curTime.ToString("00.00").Replace('.', ':');
        }
        private string GetSpeedText()
        {
            var curGuage = TokenInputManager.Instance.CurSpeed;
          
            return curGuage.ToString("00") + " m/s";
        }
    }
}
