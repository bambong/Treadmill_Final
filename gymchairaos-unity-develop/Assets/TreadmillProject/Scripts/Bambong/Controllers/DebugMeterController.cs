using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugMeterController : MonoBehaviour
{
    [Header("������ ������ �Ÿ�")]
    [SerializeField]
    private TextMeshProUGUI rightTotalMeterT;
    [Header("���� ������ �Ÿ�")]
    [SerializeField]
    private TextMeshProUGUI leftTotalMeterT;
    [Header("���� ������ �Ÿ�")]
    [SerializeField]
    private TextMeshProUGUI calcMeterT;


    //[Header("�ִ� Calc")]
    //[SerializeField]
    //private TextMeshProUGUI maxCalcT;
    //[Header("�ּ� Calc")]
    //[SerializeField]
    //private TextMeshProUGUI minCalcT;
    //[Header("���� Calc")]
    //[SerializeField]
    //private TextMeshProUGUI curCalcT;



    //[Header("���� ���� ���� ����")]
    //[SerializeField]
    //private TextMeshProUGUI curWheelT;
    [Header("���� �ӵ�")]
    [SerializeField]
    private TextMeshProUGUI curspeedT;
    //[Header("���� ����")]
    //[SerializeField]
    //private TextMeshProUGUI curmeterT;
    [Header("���� ���� ���� �ӵ�")]
    [SerializeField]
    private TextMeshProUGUI curLwheelT;
    [Header("���� ������ ���� �ӵ�")]
    [SerializeField]
    private TextMeshProUGUI curRwheelT;
    //[Header("���� ���� �ְ� �ӵ�")]
    //[SerializeField]
    //private TextMeshProUGUI maxLwheelT;
    //[Header("������ ���� �ְ� �ӵ�")]
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
        //    maxLwheelT.text = string.Format("���� ���� �ְ� �ӵ� : {0:0.00#} m/s ", maxLwheel);
        //}

        //if (maxRwheel < token.CurRightSpeedMPS)
        //{
        //    maxRwheel = token.CurRightSpeedMPS;
        //    maxRwheelT.text = string.Format("������ ���� �ְ� �ӵ� : {0:0.00#} m/s", maxRwheel);
        //}

        //curmeterT.text = string.Format("���� ���� : {0:0.00#} M ", token.MoveMeter);
        SetCurMeterText();
    }

    private void OnDestroy()
    {
        Managers.Token.ReceivedEvent = null;
    }
}
