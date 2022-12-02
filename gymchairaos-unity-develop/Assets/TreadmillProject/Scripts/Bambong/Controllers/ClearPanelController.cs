using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace bambong 
{
    public class ClearPanelController : PanelController
    {
        [SerializeField]
        private TextMeshProUGUI timeText;
       
        public void SetTimeText(string text) 
        {
           timeText.text = "½Ã°£ :  " + text;
        }
        public void OnClickMenuButton() 
        {
            TransitionManager.Instance.SceneTransition(E_SceneName.Speed_MenuScene.ToString());
        }
        public void OnClickRetryButton() 
        {
            TransitionManager.Instance.SceneTransition(E_SceneName.Speed_GameScene.ToString());
        }
        public void OnClickExitButton()
        {
            TransitionManager.Instance.SceneTransition(E_SceneName.SelectGame.ToString());
        }

    }
}
