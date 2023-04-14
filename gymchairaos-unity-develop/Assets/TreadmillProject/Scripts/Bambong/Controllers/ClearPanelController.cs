using DG.Tweening;
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
        [SerializeField]
        private CanvasGroup canvasGroup;
        private readonly float FADE_TIME = 1f;
        public void SetTimeText(string text) 
        {
           timeText.text = text;
        }
        public override void Open()
        {
            canvasGroup.alpha = 0;
            base.Open();
            canvasGroup.DOFade(1, FADE_TIME);
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
