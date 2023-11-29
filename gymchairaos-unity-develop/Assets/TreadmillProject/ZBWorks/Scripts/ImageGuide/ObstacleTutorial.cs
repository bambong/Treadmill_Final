using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
/*
 * Speed -> Guage -> Distance -> heart -> 전진 -> 왼쪽 -> 오른쪽
 */
public class ObstacleTutorial : MonoBehaviour
{
    public enum State { none, tutorialAskFading, tutorialDoing }

    [SerializeField] private ZB.PlayerInputManager2 player;
    [SerializeField] private CanvasGroup tutorialAskGroup;
    [SerializeField] private RectTransform rtf_Yes;
    [SerializeField] private RectTransform rtf_No;

    [SerializeField] private ImageGroupControl[] groupControls;
    [SerializeField] private ImageGroupControl groupControlVariable;
    [SerializeField] private int focusingIndex;

    [SerializeField] private State state;

    private UnityEvent GameStartEvent;
    private bool tutorialAskFading;
    private ImageGroupControl focusingGroup { get => groupControls[focusingIndex]; }

    public void AddEvent_GameStart(UnityAction action)
    {
        if (GameStartEvent == null) GameStartEvent = new UnityEvent();
        GameStartEvent.AddListener(action);

        //마지막 인덱스의 사라짐이벤트에 게임시작을 추가한다.
        groupControls[groupControls.Length - 1].SetDisappearEndEvent(GameStartEvent.Invoke);
    }
    public void TutorialAsk()
    {
        tutorialAskGroup.gameObject.SetActive(true);
        state = State.none;
        tutorialAskGroup.alpha = 0;
        tutorialAskGroup.DOFade(1, 1);
    }
    public void OnBtnClick_Yes()
    {
        if (state != State.tutorialAskFading) 
        {
            rtf_Yes.DOShakeScale(0.5f, 0.5f);
            TutorialAskClose(TutorialStart, true);
        }
    }
    public void OnBtnClick_No()
    {
        if (state != State.tutorialAskFading)
        {
            rtf_No.DOShakeScale(0.5f, 0.5f);
            TutorialAskClose(GameStartEvent.Invoke, false);
        }
    }


    private void TutorialStart()
    {
        focusingIndex = 0;
        focusingGroup.Appear();
    }
    private void TutorialAskClose(UnityAction action, bool isYes)
    {
        state = State.tutorialAskFading;
        UnityEvent closeEvent = new UnityEvent();
        closeEvent.AddListener(action);
        tutorialAskGroup.DOFade(0, 1).SetDelay(0.5f).OnComplete(() =>
        {
            state = isYes ? State.tutorialDoing : State.none;
            closeEvent.Invoke();
            tutorialAskFading = false;
            tutorialAskGroup.gameObject.SetActive(false);
        });
    }

    [ContextMenu("SetVariable")]
    private void SetVariable()
    {
        for (int i = 0; i < groupControls.Length; i++)
        {
            groupControls[i].firstAlpha = groupControlVariable.firstAlpha;
            groupControls[i].targetAlpha = groupControlVariable.targetAlpha;
            groupControls[i].duration = groupControlVariable.duration;
        }
    }

    private void Awake()
    {
        tutorialAskGroup.gameObject.SetActive(false);
        tutorialAskGroup.alpha = 0;
        //각 사라짐 이벤트에 다음 등장 이벤트를 추가한다.
        for (int i = 0; i < groupControls.Length - 1; i++) 
        {
            groupControls[i].SetDisappearEndEvent(groupControls[i + 1].Appear);
        }
        //특수조건 추가
        groupControls[5].SetDisappearStartCondition(player.IsFrontMoving);
        groupControls[8].SetDisappearStartCondition(player.IsLeftMoving);
        groupControls[11].SetDisappearStartCondition(player.IsRightMoving);
    }
    private void Update()
    {
        if (state == State.tutorialDoing &&
            !focusingGroup.CheckDisappearCondition() &&
            focusingIndex + 1 < groupControls.Length) 
        {
            focusingIndex++;
        }
    }
}