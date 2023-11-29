using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ImageGuide2 : MonoBehaviour
{
    [System.Serializable]
    public class GroupControll
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float firstAlpha;
        [SerializeField] private float targetAlpha;
        [SerializeField] private float duration;
        [SerializeField] private Condition condition;

        private UnityEvent disappearEndEvent;

        public void Appear()
        {
            group.gameObject.SetActive(true);
            group.alpha = firstAlpha;
            group.DOFade(targetAlpha, duration);

        }
        public void CheckDisappearCondition()
        {
            if (condition.style == Condition.Style.boolDelegate &&
                condition.boolDelegte.Invoke())
            {
                Disappear();
            }
        }

        public void SetDisappearStartCondition(BoolDelegate boolDelegate)
        {
            condition.style = Condition.Style.boolDelegate;
            condition.boolDelegte = boolDelegate;
        }
        public void SetDisappearEndEvent(UnityAction action)
        {
            if (disappearEndEvent == null) disappearEndEvent = new UnityEvent();
            disappearEndEvent.AddListener(action);
        }

        private void Disappear()
        {
            group.DOFade(0, duration).OnComplete(() => group.gameObject.SetActive(false));
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
}
