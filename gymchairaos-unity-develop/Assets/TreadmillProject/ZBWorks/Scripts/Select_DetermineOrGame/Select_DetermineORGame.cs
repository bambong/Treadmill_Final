using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class Select_DetermineORGame : MonoBehaviour
{
    [SerializeField] GameObject screenBlock;
    [SerializeField] RectTransform rtf_title;
    [SerializeField] Image img_title;

    [Space]
    [SerializeField] ScreenInfo leftScreen;
    [SerializeField] ScreenInfo rightScreen;

    Vector2 goalPos_title;
    Vector2 goalPos_text_left;
    Vector2 goalPos_text_right;
    Vector2 goalPos_btn_left;
    Vector2 goalPos_btn_right;

    Color whiteClear = new Color(1, 1, 1, 0);

    private void Start()
    {
        leftScreen.Init();
        rightScreen.Init();

        Enter();
    }

    public void OnBtnClicked_LeftScreen()
    {
        LeftScreeActive(true);
        RightScreenActive(false);
    }
    public void OnBtnClicked_RightScreen()
    {
        LeftScreeActive(false);
        RightScreenActive(true);
    }
    public void OnBtnClicked_LeftStart()
    {

    }
    public void OnBtnClicked_RightStart()
    {

    }

    private void Enter()
    {
        Vector2 titleFirstPos = rtf_title.anchoredPosition;
        rtf_title.anchoredPosition += Vector2.down * 375;
        img_title.color = new Color(1, 1, 1, 0);

        rtf_title.DOAnchorPos(titleFirstPos, 2).SetEase(Ease.OutQuart);
        img_title.DOColor(Color.white, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            leftScreen.Appear();
            rightScreen.Appear();

            screenBlock.SetActive(false);
        });
    }
    private void LeftScreeActive(bool active)
    {
        leftScreen.Active(active);
    }
    private void RightScreenActive(bool active)
    {
        rightScreen.Active(active);
    }

    [System.Serializable]
    public class ScreenInfo
    {
        [SerializeField] VideoPlayer videoPlayer;

        [SerializeField] RectTransform rtf_text;
        [SerializeField] RectTransform rtf_btn;
        [SerializeField] Image img_text;
        [SerializeField] Image img_btn;
        [SerializeField] Image img_cover;
        [SerializeField] GameObject obj_CoverBtn;

        [Space]
        [SerializeField] float duration;
        [SerializeField] Vector2 startOffset_text;
        [SerializeField] Vector2 startOffset_btn;

        Vector2 firstPos_text = Vector2.zero;
        Vector2 firstPos_btn = Vector2.zero;

        public void Init()
        {
            firstPos_text = rtf_text.anchoredPosition;
            firstPos_btn = rtf_btn.anchoredPosition;

            rtf_text.anchoredPosition += startOffset_text;
            rtf_btn.anchoredPosition += startOffset_btn;

            img_text.color = new Color(1, 1, 1, 0);
            img_btn.color = new Color(1, 1, 1, 0);

            videoPlayer.Stop();
        }
        public void Appear()
        {
            rtf_text.DOAnchorPos(firstPos_text, 2).SetEase(Ease.OutQuart);
            img_text.DOColor(Color.white, 2).SetEase(Ease.OutQuart);
        }
        public void Active(bool active)
        {
            Vector2 startPos_btn = firstPos_btn + (active ? startOffset_btn : Vector2.zero);
            Vector2 goalPos_btn = firstPos_btn + (active ? Vector2.zero : startOffset_btn);

            Color goalColor = active ? Color.white : new Color(1, 1, 1, 0);
            Color goalColor_Cover = active ? new Color(1, 1, 1, 0) : Color.white;

            obj_CoverBtn.SetActive(!active);
            rtf_btn.gameObject.SetActive(true);

            rtf_btn.DOKill();
            rtf_btn.DOAnchorPos(goalPos_btn, duration)
                .SetEase(Ease.OutQuart)
                .SetDelay(active ? 1 : 0)
                .OnComplete(()=>
            {
                rtf_btn.gameObject.SetActive(active);
            });

            img_btn.DOKill();
            img_btn.DOColor(goalColor, duration)
                .SetEase(Ease.OutQuart)
                .SetDelay(active ? 1 : 0);

            img_cover.DOColor(goalColor_Cover, duration)
                .SetEase(Ease.OutQuart);

            if (active)
                videoPlayer.Play();
            else
                videoPlayer.Stop();
        }
    }
}
