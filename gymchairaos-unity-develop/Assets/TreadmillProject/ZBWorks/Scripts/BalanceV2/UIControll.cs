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
    [SerializeField] CanvasGroup group_result;
    [SerializeField] CanvasGroup group_pause;
    [SerializeField] TextMeshProUGUI tmp_stage;
    [SerializeField] TextMeshProUGUI tmp_time;
    [SerializeField] TextMeshProUGUI tmp_bpm;
    [SerializeField] UiShadow2 shadow;

    public void PageActive_Result(bool active)
    {
        if (active)
        {
            group_result.gameObject.SetActive(true);
            group_result.alpha = 0;
        }
        group_result.DOKill();
        group_result.DOFade(active ? 1 : 0, 1).SetUpdate(true).OnComplete(()=>
        {
            if (!active) group_result.gameObject.SetActive(false);
        });

        shadow.SetActive(true);
        UnityAction action = null;
        if (!active) action = () => shadow.SetActive(false);
        shadow.SetAlpha(active ? 0.75f : 0, true, 0.5f, action);
    }
    public void PageActive_Pause(bool active)
    {
        group_pause.gameObject.SetActive(active);

        shadow.SetActive(true);
        UnityAction action = null;
        if (!active) action = () => shadow.SetActive(false);
        shadow.SetAlpha(active ? 0.75f : 0, true, 0.5f, action);
    }

    public void PageActiveDirectly_Result(bool active)
    {
        group_result.DOKill();
        group_result.gameObject.SetActive(active);
    }
    public void PageActiveDirectly_Pause(bool active)
    {
        group_pause.DOKill();
        group_pause.gameObject.SetActive(active);
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
        Managers.Scene.LoadScene(E_SceneName.Balance_MenuScene);
    }

    private void Awake()
    {
        group_result.gameObject.SetActive(false);
        group_pause.gameObject.SetActive(false);

        PageActiveDirectly_Result(false);
        PageActiveDirectly_Pause(false);
    }
}
