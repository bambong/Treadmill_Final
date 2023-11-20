using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ResultPageController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp_Distance;
    [SerializeField] TextMeshProUGUI tmp_Time;
    [SerializeField] TextMeshProUGUI tmp_Bpm;
    [SerializeField] TextMeshProUGUI tmp_Calorie;
    [SerializeField] GameObject interactBlock;
    [SerializeField] PausePageController pauseController;
    [Space]

    [SerializeField] TimeCounter timeCounter;
    [SerializeField] ZB.DistanceRecord distanceRecord;

    [SerializeField] UnityEvent uEvent_ReStart;
    [SerializeField] UnityEvent uEvent_GoMain;
    [SerializeField] CanvasGroup group;
    [SerializeField] UiShadow uiShadow;
    [SerializeField] float duration;

    [SerializeField] bool active;

    Vector3 originalSize;
    WaitForSeconds Unactive_WFS;

    private void Awake()
    {
        Unactive_WFS = new WaitForSeconds(duration);
        group.gameObject.SetActive(false);
    }

    public void SetTextInfo(string dist, string time, string bpm, string calorie)
    {
        tmp_Distance.text = dist;
        tmp_Time.text = time;
        tmp_Bpm.text = bpm;
        tmp_Calorie.text = calorie;
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
            group.gameObject.SetActive(true);
            group.DOKill();
            group.alpha = 0;
            group.DOFade(1, duration).SetUpdate(true);

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
            group.DOKill();
            group.DOFade(0, duration).SetUpdate(true);
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
        group.gameObject.SetActive(false);
        uEvent_ReStart.Invoke();
    }
}
