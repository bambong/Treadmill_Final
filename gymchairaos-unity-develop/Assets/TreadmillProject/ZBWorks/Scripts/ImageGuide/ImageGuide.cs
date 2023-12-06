using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public delegate bool BoolDelegate();
public class ImageGuide : MonoBehaviour
{
    [SerializeField] private bool guideStartOnStart;
    [SerializeField] private UnityEvent enterEvent;
    [SerializeField] private UnityEvent stayEvent;
    [SerializeField] private UnityEvent exitEvent;
    [SerializeField] private Guide[] guides;

    private UnityEvent GuideEndEvent;

    private int nowGuideIndex = -1;
    private Guide nowGuide { get => nowGuideIndex < guides.Length && nowGuideIndex >= 0 ? guides [nowGuideIndex] : null; }

    public void InitNStart()
    {
        for (int i = 0; i < guides.Length; i++)
        {
            guides[i].SetEvents(enterEvent, stayEvent, exitEvent);
        }

        nowGuideIndex = 0;
        nowGuide.OnEnter();
        enterEvent.Invoke();
    }
    public void AddCondition(int index, BoolDelegate condition)
    {
        guides[index].condition = condition;
    }
    public void AddGuideEndAction(UnityAction action)
    {
        if (GuideEndEvent == null) GuideEndEvent = new UnityEvent();
        GuideEndEvent.AddListener(action);
    }

    private void Start()
    {
        if (guideStartOnStart)
            InitNStart();
        transform.SetSiblingIndex(4);
    }
    private void Update()
    {
        if (nowGuide != null)
        {
            switch (nowGuide.nowState)
            {
                case Guide.State.isStay:
                    if (nowGuide.ConditionCheckOnUpdate())
                    {
                        nowGuide.OnExit();
                        exitEvent.Invoke();
                    }

                    break;
                case Guide.State.isExit:
                    if (nowGuide.ExitGroupFaded) 
                    {
                        nowGuide.StateNone();
                        nowGuideIndex++;

                        if (nowGuide != null)
                        {
                            nowGuide.OnEnter();
                            enterEvent.Invoke();
                        }
                        else
                            GuideEndEvent.Invoke();
                    }
                    break;
            }
        }
    }

    [Space(20), Header("ValueHandler")]
    [SerializeField] private Guide valueHandler;
    [ContextMenu("UnifyValue")]
    private void UnifyValue()
    {
        for (int i = 0; i < guides.Length; i++)
        {
            guides[i].enterDuration = valueHandler.enterDuration;
            guides[i].enterDelay = valueHandler.enterDelay;
            guides[i].stayDuration = valueHandler.stayDuration;
            guides[i].exitDuration = valueHandler.exitDuration;
            guides[i].exitDelay = valueHandler.exitDelay;
        }
    }

    [System.Serializable]
    public class Guide
    {
        public State nowState;
        [Space]
        public CanvasGroup enterGroup;
        public float enterDuration;
        public float enterDelay;
        [Space]
        public CanvasGroup stayGroup;
        public float stayDuration;
        public BoolDelegate condition;
        [Space]
        public CanvasGroup exitGroup;
        public float exitDuration;
        public float exitDelay;

        public bool ExitGroupFaded { get; private set; }

        private UnityEvent enterEvent;
        private UnityEvent stayEvent;
        private UnityEvent exitEvent;

        public bool ConditionCheckOnUpdate()
        {
            return condition.Invoke();
        }
        public void SetEvents(UnityEvent enterEvent, UnityEvent stayEvent, UnityEvent exitEvent)
        {
            this.enterEvent = enterEvent;
            this.stayEvent = stayEvent;
            this.exitEvent = exitEvent;
        }
        public void StateNone()
        {
            nowState = State.None;
        }

        public void OnEnter() 
        {
            ExitGroupFaded = false;

            //enterGroup ���� �ž� �������
            if (enterGroup != null)
            {
                nowState = State.isEnter;
                enterEvent.Invoke();

                enterGroup.gameObject.SetActive(true);
                enterGroup.DOFade(1, enterDuration).OnComplete(() =>
                {
                    enterGroup.DOFade(0, enterDuration).SetDelay(enterDelay).OnComplete(() =>
                    {
                        enterGroup.gameObject.SetActive(false);

                        nowState = State.isStay;
                        stayEvent.Invoke();
                        stayGroup.gameObject.SetActive(true);
                        stayGroup.DOFade(1, stayDuration);
                    });
                });
            }
            //enterGroup ���� �ȵž� ���� ���
            else
            {
                nowState = State.isStay;
                stayEvent.Invoke();
                stayGroup.gameObject.SetActive(true);
                stayGroup.DOFade(1, stayDuration);
            }
        }
        public void OnExit()
        {
            if (stayGroup != null)
            {
                stayGroup.DOKill();
                stayGroup.DOFade(0, stayDuration).OnComplete(()=> { stayGroup.gameObject.SetActive(false); });
            }

            //exitGroup ���� �ž� �������
            if (exitGroup != null)
            {
                nowState = State.isExit;
                exitEvent.Invoke();
                exitGroup.gameObject.SetActive(true);
                exitGroup.DOFade(1, exitDuration).OnComplete(() =>
                {
                    exitGroup.DOFade(0, exitDuration).SetDelay(exitDelay).OnComplete(() =>
                    {
                        exitGroup.gameObject.SetActive(false);
                        ExitGroupFaded = true;
                        FadeDirectly();
                    });
                });
            }
            else
            {
                nowState = State.None;
                ExitGroupFaded = true;
                FadeDirectly();
            }
        }
        private void FadeDirectly()
        {
            enterGroup.DOKill();
            enterGroup.alpha = 0;
            stayGroup.DOKill();
            stayGroup.alpha = 0;
            exitGroup.DOKill();
            exitGroup.alpha = 0;

            enterGroup.gameObject.SetActive(false);
            stayGroup.gameObject.SetActive(false);
            exitGroup.gameObject.SetActive(false);
        }

        public enum State
        {
            None,
            isEnter,
            isStay,
            isExit
        }
    }
}