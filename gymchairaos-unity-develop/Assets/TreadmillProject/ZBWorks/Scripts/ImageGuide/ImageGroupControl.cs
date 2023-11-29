using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class ImageGroupControl
{
    public enum State { appear, disappearing, disappeared}

    public float firstAlpha;
    public float targetAlpha;
    public float duration;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Condition condition;
    [SerializeField] private State state;

    private UnityEvent disappearEndEvent;
    private float waitTime;

    public void Appear()
    {
        state = State.appear;
        waitTime = condition.waitTime;

        group.gameObject.SetActive(true);
        group.alpha = firstAlpha;
        group.DOFade(targetAlpha, duration);
    }
    public bool CheckDisappearCondition()
    {
        if (state == State.appear) 
        {
            if (condition.style == Condition.Style.boolDelegate &&
                condition.boolDelegte.Invoke())
            {
                Disappear();
            }

            else if (condition.style == Condition.Style.waitTime)
            {
                if (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else if (waitTime <= 0)
                {
                    Disappear();
                }
            }
        }

        return state != State.disappeared;
    }

    public void SetDisappearStartCondition(BoolDelegate boolDelegate)
    {
        condition.style = Condition.Style.boolDelegate;
        condition.boolDelegte = boolDelegate;
    }
    public void SetDisappearStartCondition(float waitTime)
    {
        condition.style = Condition.Style.waitTime;
        condition.waitTime = waitTime;
    }
    public void SetDisappearEndEvent(UnityAction action)
    {
        if (disappearEndEvent == null) disappearEndEvent = new UnityEvent();
        disappearEndEvent.AddListener(action);
    }

    private void Disappear()
    {
        state = State.disappearing;
        group.DOFade(0, duration).OnComplete(() =>
        {
            state = State.disappeared;
            group.gameObject.SetActive(false);
            disappearEndEvent.Invoke();
        });
    }

    [System.Serializable]
    public class Condition
    {
        public Style style;
        public float waitTime;
        public BoolDelegate boolDelegte;

        public enum Style
        {
            boolDelegate,
            waitTime
        }
    }
}