using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PausePageController : MonoBehaviour
{
    [SerializeField] GameObject interactBlock;
    [SerializeField] UiShadow uiShadow;
    [SerializeField] Transform body;
    [SerializeField] float duration;
    Vector2 originalSize;
    WaitForSecondsRealtime Unactive_WFS;

    [Space]
    [SerializeField] Image pauseBlockImg;
    [SerializeField] bool pauseBlockActive;
    [SerializeField] float pauseBlockDuration;
    WaitForSecondsRealtime pauseBlockDuration_WFS;

    public void OnBtnClicked_Pause()
    {
        PauseBtnInteractBlockActive(true);
        uiShadow.Active(true);
        Time.timeScale = 0;

        body.gameObject.SetActive(true);
        body.transform.DOKill();
        body.transform.localScale = Vector2.zero;
        body.transform.DOScale(originalSize, duration).SetEase(Ease.OutQuart).SetUpdate(true);
    }
    public void OnBtnClicked_Play()
    {
        PauseBtnInteractBlockActive(false);
        uiShadow.Active(false);
        if (Unactive_C != null)
            StopCoroutine(Unactive_C);
        Unactive_C = UnactiveC();
        StartCoroutine(Unactive_C);
    }
    public void OnBtnClicked_Exit()
    {

    }

    public void PauseBtnInteractBlockActive(bool active)
    {
        if (!pauseBlockActive && active)
        {
            pauseBlockActive = active;

            pauseBlockImg.gameObject.SetActive(true);
            pauseBlockImg.DOColor(Color.white, pauseBlockDuration).SetUpdate(true).SetEase(Ease.OutQuart);
        }
        else if (pauseBlockActive && !active)
        {
            pauseBlockActive = active;

            if (pauseBlockActive_C != null)
                StopCoroutine(pauseBlockActive_C);
            pauseBlockActive_C = pauseBlockActiveC();
            StartCoroutine(pauseBlockActive_C);
        }
    }

    private void Awake()
    {
        originalSize = body.localScale;
        Unactive_WFS = new WaitForSecondsRealtime(duration);
        pauseBlockDuration_WFS = new WaitForSecondsRealtime(pauseBlockDuration);
    }

    IEnumerator Unactive_C;
    IEnumerator UnactiveC()
    {
        body.transform.DOKill();
        body.transform.DOScale(Vector2.zero, duration).SetEase(Ease.InQuart).SetUpdate(true);

        yield return Unactive_WFS;
        body.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    IEnumerator pauseBlockActive_C;
    IEnumerator pauseBlockActiveC()
    {
        pauseBlockImg.DOColor(Color.clear, pauseBlockDuration).SetUpdate(true).SetEase(Ease.OutQuart);
        interactBlock.gameObject.SetActive(true);
        yield return pauseBlockDuration_WFS;
        interactBlock.gameObject.SetActive(false);
        pauseBlockImg.gameObject.SetActive(false);
    }
}
