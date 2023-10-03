using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB.UsingTweening.Guage;

public class SpeedGuage : MonoBehaviour
{
    [SerializeField] TweeningGuage guage;
    [SerializeField] RectTransform rtf_right;
    [SerializeField] RectTransform rtf_bar;

    Vector2 rightInitPos;
    float barInitSizeX;

    public void Init()
    {
        guage.ChangeNotTweening(0);
    }

    public void OnPlayerMoved(float ratio)
    {
        guage.Change(ratio);
    }

    private void Awake()
    {
        rightInitPos = rtf_right.anchoredPosition;
        barInitSizeX = rtf_bar.sizeDelta.x;
    }

    private void Update()
    {
        rtf_right.anchoredPosition = rightInitPos + Vector2.left * (barInitSizeX - rtf_bar.sizeDelta.x);
    }
}
