using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bambong;
using DG.Tweening;

public class BalanceSelectScene : MonoBehaviour
{
    [SerializeField] RectTransform rtf_stageEnterBtn;
    [SerializeField] Image img_scrollAlarm;
    [SerializeField] CanvasGroup group_warn;
    [SerializeField] CanvasGroup group_startBtn;
    [SerializeField] SelectButton[] selectButtons;
    [SerializeField] E_SceneName[] stageConnects;
    [SerializeField] E_SceneName connected;

    public void OnBtnClick_StageEnter()
    {
        TransitionManager.Instance.SceneTransition(connected.ToString());
    }

    public void OnScrollInput()
    {
        img_scrollAlarm.DOKill();
        img_scrollAlarm.DOColor(Color.clear, 0.2f).OnComplete(()=>
        {
            img_scrollAlarm.DOColor(Color.white, 0.2f).SetDelay(3);
        });
    }

    private void StageEnterBtnAppear()
    {
        rtf_stageEnterBtn.DOKill();
        rtf_stageEnterBtn.anchoredPosition = new Vector2(0, -265);
        rtf_stageEnterBtn.DOAnchorPos(new Vector2(0, 30.15f), 0.75f).OnComplete(()=>
        {
            group_warn.DOFade(1, 1);
        });

        group_warn.DOKill();
        group_startBtn.DOKill();

        group_warn.alpha = 0;
        group_startBtn.alpha = 0;
        group_startBtn.DOFade(1, 1);
    }

    private void SrollAlarmAppear(bool active)
    {
        Color targetColor = active ? Color.white : Color.clear;
        img_scrollAlarm.DOKill();
        img_scrollAlarm.DOColor(targetColor, 0.5f);
    }

    private void Awake()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            int currentIndex = i;
            selectButtons[i].button.onClick.AddListener(() =>
            {
                StageEnterBtnAppear();
                connected = stageConnects[currentIndex];
                selectButtons[currentIndex].SelectShow(true);

                for (int j = 0; j < selectButtons.Length; j++)
                {
                    if (j != currentIndex) selectButtons[j].SelectShow(false);
                }
            });
        }

        rtf_stageEnterBtn.anchoredPosition = new Vector2(0, -265);

        img_scrollAlarm.DOColor(Color.white, 0.2f).SetDelay(3);

        group_warn.alpha = 0;
        group_startBtn.alpha = 0;
    }
}
