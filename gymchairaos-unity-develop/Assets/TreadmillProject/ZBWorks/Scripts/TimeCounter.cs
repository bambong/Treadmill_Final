using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    public float NowTime { get { return nowTime; } }
    public float CurrentTimeScore { get { return currentTimeScore; } }
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] float nowTime;
    bool paused;

    float currentTimeScore;

    public void CountStart()
    {
        if (!paused)
        {
            count_C = countC();
            StartCoroutine(count_C);
        }
        else
        {
            paused = false;
        }
    }
    public void CountPause()
    {
        paused = true;
    }
    public void CountStop()
    {
        paused = false;
        if (count_C != null)
        {
            StopCoroutine(count_C);
        }
        currentTimeScore = nowTime;
        nowTime = 0;
        tmp.text = FormatTime(NowTime);
    }

    IEnumerator count_C;
    IEnumerator countC()
    {
        paused = false;
        nowTime = 0;
        while (true)
        {
            if (!paused)
            {
                nowTime += Time.deltaTime;
                tmp.text = FormatTime(nowTime);
            }
            yield return true;
        }
    }

    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}

