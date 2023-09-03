using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ResultPageController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp_Time;
    [SerializeField] TextMeshProUGUI tmp_Distance;
    [SerializeField] GameObject interactBlock;
    [SerializeField] PausePageController pauseController;
    [SerializeField] RankingDataHolder rankingDataHolder;
    [Space]

    [SerializeField] TimeCounter timeCounter;
    [SerializeField] ZB.DistanceRecord distanceRecord;

    [SerializeField] UnityEvent uEvent_ReStart;
    [SerializeField] UnityEvent uEvent_GoMain;
    [SerializeField] RectTransform body;
    [SerializeField] UiShadow uiShadow;
    [SerializeField] float duration;

    [SerializeField] bool active;

    Vector3 originalSize;
    WaitForSeconds Unactive_WFS;

    private void Awake()
    {
        originalSize = body.transform.localScale;
        Unactive_WFS = new WaitForSeconds(duration);
        body.gameObject.SetActive(false);
    }

    public void Active(bool active)
    {
        //활성화
        if (!this.active && active)
        {
            interactBlock.SetActive(false);
            pauseController.PauseBtnInteractBlockActive(true);

            this.active = active;

            if (Unactive_C != null)
                StopCoroutine(Unactive_C);

            uiShadow.Active(true);
            body.gameObject.SetActive(true);
            body.transform.localScale = Vector2.zero;
            body.transform.DOScale(originalSize, duration).SetEase(Ease.OutQuart);

            tmp_Distance.text = ((int)distanceRecord.CurrentDistanceRecord).ToString()+"M";
            tmp_Time.text = TimeCounter.FormatTime(timeCounter.CurrentTimeScore);

            //기록저장
            rankingDataHolder.rankingData.ranking_Obstacle.Add(RankingData.GetUserName(), RankingData.GetDate(), timeCounter.CurrentTimeScore, distanceRecord.CurrentDistanceRecord);
            rankingDataHolder.Write();
        }
        //비활성화
        else if (this.active && !active)
        {
            interactBlock.SetActive(true);
            this.active = active;

            if (Unactive_C != null)
                StopCoroutine(Unactive_C);
            Unactive_C = UnactiveC();
            StartCoroutine(Unactive_C);

            uiShadow.Active(false);
            body.transform.DOScale(Vector2.zero, duration).SetEase(Ease.InQuart);
        }
    }

    public void OnBtnClicked_Restart()
    {
        uEvent_ReStart.Invoke();
    }
    public void OnBtnClicked_GoMain()
    {
        uEvent_GoMain.Invoke();
    }

    IEnumerator Unactive_C;
    IEnumerator UnactiveC()
    {
        pauseController.PauseBtnInteractBlockActive(false);
        yield return Unactive_WFS;
        body.gameObject.SetActive(false);
        uEvent_ReStart.Invoke();
    }
}
