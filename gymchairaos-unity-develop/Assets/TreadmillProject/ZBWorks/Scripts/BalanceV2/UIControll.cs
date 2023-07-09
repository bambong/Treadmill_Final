using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIControll : MonoBehaviour
{
    [SerializeField] TimeCounter timeCounter;
    [SerializeField] Transform ui_Result;
    [SerializeField] Transform ui_Pause;
    [SerializeField] TextMeshProUGUI tmp_time;

    public void PageActive_Result(bool active)
    {
        ui_Result.DOKill();
        ui_Result.DOScale(active ? Vector3.one : Vector3.zero, 0.3f).SetUpdate(true);

        if (active)
        {
            tmp_time.text = TimeCounter.FormatTime(timeCounter.NowTime);
            Time.timeScale = 0;
        }
    }
    public void PageActive_Pause(bool active)
    {
        ui_Pause.DOKill();
        ui_Pause.DOScale(active ? Vector3.one : Vector3.zero, 0.3f).SetUpdate(true);
    }

    public void PageActiveDirectly_Result(bool active)
    {
        ui_Result.DOKill();
        ui_Result.localScale = active ? Vector3.one : Vector3.zero;
    }
    public void PageActiveDirectly_Pause(bool active)
    {
        ui_Pause.DOKill();
        ui_Pause.localScale = active ? Vector3.one : Vector3.zero;
    }

    private void Awake()
    {
        ui_Result.gameObject.SetActive(true);
        ui_Pause.gameObject.SetActive(true);

        PageActiveDirectly_Result(false);
        PageActiveDirectly_Pause(false);
    }
}
