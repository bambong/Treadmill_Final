using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugMeterController : MonoBehaviour
{
    [Header("오른쪽 움직인 거리")]
    [SerializeField]
    private TextMeshProUGUI rightTotalMeterT;
    [Header("왼쪽 움직인 거리")]
    [SerializeField]
    private TextMeshProUGUI leftTotalMeterT;
    [Header("보정 움직인 거리")]
    [SerializeField]
    private TextMeshProUGUI calcMeterT;


    //[Header("최대 Calc")]
    //[SerializeField]
    //private TextMeshProUGUI maxCalcT;
    //[Header("최소 Calc")]
    //[SerializeField]
    //private TextMeshProUGUI minCalcT;
    //[Header("현재 Calc")]
    //[SerializeField]
    //private TextMeshProUGUI curCalcT;



    //[Header("현재 미터 측정 바퀴")]
    //[SerializeField]
    //private TextMeshProUGUI curWheelT;
    [Header("현재 속도")]
    [SerializeField]
    private TextMeshProUGUI curspeedT;
    //[Header("현재 미터")]
    //[SerializeField]
    //private TextMeshProUGUI curmeterT;
    [Header("현재 왼쪽 바퀴 속도")]
    [SerializeField]
    private TextMeshProUGUI curLwheelT;
    [Header("현재 오른쪽 바퀴 속도")]
    [SerializeField]
    private TextMeshProUGUI curRwheelT;
    //[Header("왼쪽 바퀴 최고 속도")]
    //[SerializeField]
    //private TextMeshProUGUI maxLwheelT;
    //[Header("오른쪽 바퀴 최고 속도")]
    //[SerializeField]
    //private TextMeshProUGUI maxRwheelT;

    private double totalMeterR;
    private double totalMeterL;
    private double totalMeterCalc;
    
    //private double maxLwheel;
    //private double maxRwheel;

    //private double maxCalc = 0;
    //private double minCalc =0;
    //private double curCalc =0;
    private void Start()
    {
        Managers.Token.ReceivedEvent += ReceivedEvent;
    }

    private void SetCurMeterText()
    {
        rightTotalMeterT.text = string.Format("{0:0.00#} M ", totalMeterR);
        leftTotalMeterT.text = string.Format("{0:0.00#} M ", totalMeterL);
        calcMeterT.text = string.Format("{0:0.00#} M ", totalMeterCalc);
    }
    //private void SetCalcText()
    //{
    //    curCalcT.text = string.Format("{0:0.00#}", curCalc);
    //    minCalcT.text = string.Format("{0:0.00#}", minCalc);
    //    maxCalcT.text = string.Format("{0:0.00#}", maxCalc);
    //}

    public void ResetMeter()
    {
        totalMeterR = 0;
        totalMeterL = 0;
        totalMeterCalc = 0;
        SetCurMeterText();
    }

 

    private void ReceivedEvent()
    {
        var token = Managers.Token;
        totalMeterR += token.MoveMeterR;
        totalMeterL += token.MoveMeterL;
        totalMeterCalc += token.CurrentMoveMeter;

        //curCalc = token.Calc;
        //maxCalc = curCalc > maxCalc ? curCalc : maxCalc;
        //minCalc = curCalc < minCalc ? curCalc : minCalc;
        //SetCalcText();


        curLwheelT.text = string.Format("{0:0.00#} m/s", token.CurLeftSpeedMPS);
        curRwheelT.text = string.Format("{0:0.00#} m/s", token.CurRightSpeedMPS);
        curspeedT.text = string.Format("{0:0.00#} m/s", token.CurSpeedMeterPerSec);
        //if (maxLwheel < token.CurLeftSpeedMPS)
        //{
        //    maxLwheel = token.CurLeftSpeedMPS;
        //    maxLwheelT.text = string.Format("왼쪽 바퀴 최고 속도 : {0:0.00#} m/s ", maxLwheel);
        //}

        //if (maxRwheel < token.CurRightSpeedMPS)
        //{
        //    maxRwheel = token.CurRightSpeedMPS;
        //    maxRwheelT.text = string.Format("오른쪽 바퀴 최고 속도 : {0:0.00#} m/s", maxRwheel);
        //}

        //curmeterT.text = string.Format("현재 미터 : {0:0.00#} M ", token.MoveMeter);
        SetCurMeterText();
    }

    private void OnDestroy()
    {
        Managers.Token.ReceivedEvent = null;
    }
}
