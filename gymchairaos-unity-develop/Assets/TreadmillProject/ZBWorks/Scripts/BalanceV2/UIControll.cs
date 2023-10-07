using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using bambong;
using DG.Tweening;
using TMPro;

public class UIControll : MonoBehaviour
{
    [SerializeField] Transform ui_Result;
    [SerializeField] Transform ui_Pause;
    [SerializeField] TextMeshProUGUI tmp_stage;
    [SerializeField] TextMeshProUGUI tmp_time;
    [SerializeField] TextMeshProUGUI tmp_bpm;
    [SerializeField] UiShadow2 shadow;

    public void PageActive_Result(bool active)
    {
        ui_Result.DOKill();
        ui_Result.DOScale(active ? Vector3.one : Vector3.zero, 0.3f).SetUpdate(true);

        if (active)
        {
            Time.timeScale = 0;
        }

        shadow.SetActive(true);
        UnityAction action = null;
        if (!active) action = () => shadow.SetActive(false);
        shadow.SetAlpha(active ? 0.75f : 0, true, 0.5f, action);
    }
    public void PageActive_Pause(bool active)
    {
        ui_Pause.DOKill();
        ui_Pause.DOScale(active ? Vector3.one : Vector3.zero, 0.3f).SetUpdate(true);

        shadow.SetActive(true);
        UnityAction action = null;
        if (!active) action = () => shadow.SetActive(false);
        shadow.SetAlpha(active ? 0.75f : 0, true, 0.5f, action);
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

    public void SetTextInfo(string stage, float time, string bpm)
    {
        tmp_stage.text = $"{stage} ´Ü°è";
        tmp_time.text = $"{TimeCounter.FormatTime(time)}";
        tmp_bpm.text = $"{bpm} BPM";
    }

    public void GoMain()
    {
        Time.timeScale = 1;
        Managers.Scene.LoadScene(E_SceneName.SelectGame);
    }

    private void Awake()
    {
        ui_Result.gameObject.SetActive(true);
        ui_Pause.gameObject.SetActive(true);

        PageActiveDirectly_Result(false);
        PageActiveDirectly_Pause(false);
    }
}
