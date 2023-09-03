using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bambong;
using DG.Tweening;

public class BalanceSelectScene : MonoBehaviour
{
    [SerializeField] RectTransform rtf_stageEnterBtn;
    [SerializeField] Button[] stageSelectBtns;
    [SerializeField] E_SceneName[] stageConnects;
    [SerializeField] E_SceneName connected;

    public void OnBtnClick_StageEnter()
    {
        TransitionManager.Instance.SceneTransition(connected.ToString());
    }

    private void StageEnterBtnAppear()
    {
        rtf_stageEnterBtn.DOKill();
        rtf_stageEnterBtn.anchoredPosition = new Vector2(0, -265);
        rtf_stageEnterBtn.DOAnchorPos(new Vector2(0, 30.15f), 0.75f);
    }

    private void Awake()
    {
        for (int i = 0; i < stageSelectBtns.Length; i++)
        {
            //Debug.LogError(i);
            stageSelectBtns[i].onClick.AddListener(() =>
            {
                StageEnterBtnAppear();
            });
        }

        stageSelectBtns[0].onClick.AddListener(() =>
        {
            connected = stageConnects[0];
        });
        stageSelectBtns[1].onClick.AddListener(() =>
        {
            connected = stageConnects[1];
        });
        stageSelectBtns[2].onClick.AddListener(() =>
        {
            connected = stageConnects[2];
        });

        rtf_stageEnterBtn.anchoredPosition = new Vector2(0, -265);
    }
}
