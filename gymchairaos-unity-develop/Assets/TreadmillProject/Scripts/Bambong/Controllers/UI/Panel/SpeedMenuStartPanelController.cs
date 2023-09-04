using bambong;
using DG.Tweening;
using Gymchair.Core.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpeedMenuStartPanelController : PanelController
{

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private CanvasGroup imageGroup;
    [SerializeField]
    private CanvasGroup textGroup;
    [SerializeField]
    private Button but;

    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector3 endPos;
    [SerializeField]
    private float moveTime = 1f;
    [SerializeField]
    private float fadeTime = 1f;
    [SerializeField]
    private float textIntervalTime = 1f;
    [SerializeField]
    private float textFadeTime = 1f;
    private Sequence seq;

    private void Start()
    {
        Managers.Sound.PlayBGM("bgm_SpeedMenu");

        rectTransform.anchoredPosition = startPos;
    }
    public override void Open()
    {
        
        if(seq != null && seq.IsPlaying()) 
        {
            seq.Kill();
        }
        base.Open();
        rectTransform.anchoredPosition = startPos;
        imageGroup.alpha = 0;
        textGroup.alpha = 0;
        seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPos(endPos, moveTime));
        seq.Join(imageGroup.DOFade(1, fadeTime));
        seq.Append(textGroup.DOFade(1, textFadeTime));
        seq.OnComplete(() => { seq = null; });
        seq.Play();
    }
    public void SetButton(int level)
    {
        Open();
        //하단 내용 수정하였습니다. 누르는만큼 씬 여러개 로드되는 버그 -06.23
        but.onClick.RemoveAllListeners();
        but.onClick.AddListener( 
            ()=> {
                Managers.Sound.PlayTouchEffect();
                LevleTransition(level);
                but.interactable = false;
            });
    }
    public void LevleTransition(int level)
    {
        GameManager.Instance.SetLevel(level);
        Managers.Scene.LoadScene(E_SceneName.Speed_GameScene);
    }

}
