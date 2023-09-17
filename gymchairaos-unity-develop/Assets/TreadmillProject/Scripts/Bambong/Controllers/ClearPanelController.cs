using DG.Tweening;
using Gymchair.Core.Mgr;
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
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayTouchEffect();
#endif
            Managers.Scene.LoadScene(E_SceneName.Speed_MenuScene);
        }
        public void OnClickRetryButton() 
        {
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayTouchEffect();
#endif
            Managers.Scene.LoadScene(E_SceneName.Speed_GameScene);
        }
        public void OnClickExitButton()
        {
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayTouchEffect();
#endif
            Managers.Scene.LoadScene(E_SceneName.SelectGame);
        }

    }
}
