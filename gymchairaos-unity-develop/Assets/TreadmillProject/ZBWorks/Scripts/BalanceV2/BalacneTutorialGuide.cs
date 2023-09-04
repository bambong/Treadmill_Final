using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

namespace ZB.Balance2
{
    public class BalacneTutorialGuide : MonoBehaviour
    {
        public delegate bool GuideEndCondition();

        [SerializeField] UiShadow2 uiShadow;
        [SerializeField] RectTransform rtf_left;
        [SerializeField] RectTransform rtf_right;
        [SerializeField] RectTransform rtf_tmp;
        [SerializeField] Image img_left;
        [SerializeField] Image img_right;
        [SerializeField] TextMeshProUGUI tmp;
        [SerializeField] GuideSpriteSets[] guideSpriteSets;

        GuideEndCondition currentCondition;
        UnityAction onEndCycleStart;
        UnityAction onEndCycleEnd;

        int currentIndex;
        bool activing;

        Vector2 pos_img_Start = new Vector2(300, 91);
        Vector2 pos_img_End = new Vector2(752, -43);
        Vector2 size_img_End = Vector2.one * 0.65f;

        Vector2 pos_tmp_Start = Vector2.down * 262;
        Vector2 pos_tmp_End = Vector2.down * 488;
        Vector2 size_tmp_End = Vector2.one * 0.55f;

        public void Active(int index, GuideEndCondition endCondition, UnityAction onCenterAppearEnd, UnityAction onEndCycleStart, UnityAction onEndCycleEnd)
        {
            Debug.LogError("ACTIVE!!");
            activing = true;

            uiShadow.SetActive(true);
            uiShadow.SetAlpha(0.75f, true, 0.75f);

            if (index >= 0 && index < guideSpriteSets.Length)
            {
                img_left.sprite = guideSpriteSets[index].leftSprite;
                img_right.sprite = guideSpriteSets[index].rightSprite;
                tmp.text = guideSpriteSets[index].text;
            }

            currentCondition = endCondition;
            this.onEndCycleStart = onEndCycleStart;
            this.onEndCycleEnd = onEndCycleEnd;

            tmp.gameObject.SetActive(true);
            rtf_tmp.DOKill();
            rtf_tmp.transform.DOKill();
            rtf_tmp.anchoredPosition = pos_tmp_Start;
            rtf_tmp.localScale = Vector2.zero;
            rtf_tmp.transform.DOScale(Vector2.one, 1).SetEase(Ease.InOutQuart).SetUpdate(true);

            rtf_right.gameObject.SetActive(true);
            rtf_right.DOKill();
            rtf_right.transform.DOKill();
            rtf_right.anchoredPosition = pos_img_Start;
            rtf_right.transform.localScale = Vector2.zero;
            rtf_right.transform.DOScale(Vector2.one, 1).SetEase(Ease.InOutQuart).SetUpdate(true);

            rtf_left.gameObject.SetActive(true);
            rtf_left.DOKill();
            rtf_left.transform.DOKill();
            rtf_left.anchoredPosition = new Vector2(-pos_img_Start.x, pos_img_Start.y);
            rtf_left.transform.localScale = Vector2.zero;
            rtf_left.transform.DOScale(Vector2.one, 1).SetEase(Ease.InOutQuart).SetUpdate(true).OnComplete(() =>
            {
                rtf_tmp.transform.DOScale(size_tmp_End, 0.75f).SetEase(Ease.InOutQuart).SetDelay(0.75f).SetUpdate(true);
                rtf_tmp.DOAnchorPos(pos_tmp_End, 0.75f).SetEase(Ease.InOutQuart).SetDelay(0.75f).SetUpdate(true);

                rtf_right.DOAnchorPos(pos_img_End, 0.75f).SetEase(Ease.InOutQuart).SetDelay(0.75f).SetUpdate(true);
                rtf_right.transform.DOScale(size_img_End, 0.75f).SetEase(Ease.InOutQuart).SetDelay(0.75f).SetUpdate(true);

                rtf_left.DOAnchorPos(new Vector2(-pos_img_End.x, pos_img_End.y), 0.75f).SetEase(Ease.InOutQuart).SetDelay(0.75f).SetUpdate(true);
                rtf_left.transform.DOScale(size_img_End, 0.75f).SetEase(Ease.InOutQuart).SetDelay(0.75f).SetUpdate(true).OnComplete(() =>
                {
                    uiShadow.SetAlpha(0, true, 0.75f, () => uiShadow.SetActive(false));

                    if (onCenterAppearEnd != null)
                    {
                        UnityEvent unityEvent = new UnityEvent();
                        unityEvent.AddListener(onCenterAppearEnd);
                        unityEvent.Invoke();
                    }
                });
            });
        }

        private void Update()
        {
            if (activing && currentCondition())
            {
                activing = false;

                UnityEvent unityEvent = new UnityEvent();
                if (onEndCycleStart != null)
                {
                    unityEvent.AddListener(onEndCycleStart);
                    unityEvent.Invoke();
                }

                rtf_tmp.DOScale(Vector2.zero, 0.75f).SetEase(Ease.InOutQuart).SetUpdate(true);
                rtf_right.DOScale(Vector2.zero, 0.75f).SetEase(Ease.InOutQuart).SetUpdate(true);
                rtf_left.DOScale(Vector2.zero, 0.75f).SetEase(Ease.InOutQuart).SetUpdate(true).OnComplete(() =>
                {
                    if (onEndCycleEnd != null)
                    {
                        unityEvent.RemoveAllListeners();
                        unityEvent.AddListener(onEndCycleEnd);
                        unityEvent.Invoke();
                    }
                });
            }
        }

        [ContextMenu("Test")]
        private void TestActive()
        {
            Active(0, null, null, null, null);
        }

        [System.Serializable]
        public class GuideSpriteSets
        {
            public Sprite leftSprite;
            public Sprite rightSprite;
            public string text;
        }
    }
}