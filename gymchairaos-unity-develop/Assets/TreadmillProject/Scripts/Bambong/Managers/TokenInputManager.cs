
using Gymchair.Contents.Popup;
using System;
using System.Collections;
using System.Text;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputToken
{
    // � Ű�� �Է����� ������ 
    private KeyCode keyCode;
    // ���� ȸ�� �� 
    private int curTokenCount;
    // ����� ȸ���� ���� �� �Է����� ������ ��
    private int inputNeedCount;

    private float lastEventTime;

    // ���� �Է��� �����Ǿ��� �� �߻���ų �̺�Ʈ
    public Action CallEvent { get; set; }
    // �Է� ������ �� -> �ش� �����ͷ� �ӷ��� ����
    public float TokenEventTerm { get; private set; } = float.MaxValue;
    // ������ �̺�Ʈ �Է��� ���� �� �� �ð�
    public float LastEventTime { get { return lastEventTime; } }
    
    public InputToken(KeyCode keyCode,int inputNeedCount) 
    {
        this.keyCode = keyCode;
        this.inputNeedCount = inputNeedCount;
    }

    public void InputUpdate() 
    {
        lastEventTime += Time.deltaTime;

        if(Input.GetKeyDown(keyCode))
        {
            curTokenCount++;
            if(inputNeedCount <= curTokenCount) 
            {
                TokenEventTerm = lastEventTime;
                lastEventTime = 0;
                CallEvent?.Invoke();
                
                ResetCurToken();
            }
        }
    }
    public void OnTokenEvent() 
    {
    
    }
    public void ResetCurToken() => curTokenCount = 0;
}


public class TokenInputManager 
{
    private readonly KeyCode FOR_PC_LEFT_KEY = KeyCode.A;
    private readonly KeyCode FOR_PC_RIGHT_KEY = KeyCode.D;
    private readonly int INPUT_NEED_COUNT = 1;
    private float METER_FACTOR = 1.884f; // PI * 7f(����) * 0.01(METER)

    // ���� ���� �Է� ��    
    public float LeftTokenTerm { get => leftToken.TokenEventTerm; }
    // ������ ���� �Է� ��
    public float RightTokenTerm { get => rightToken.TokenEventTerm; }

    public double CurSpeedMeterPerSec { get; private set; } // ��� ���� ���ǵ� m/s  abs ��
    public float CurRpm { get { return math.abs(prevLeftAsX) + math.abs(prevRightAsX) * 0.5f; } }
    public double CurLeftSpeedMPS { get; private set; }// ���� ���� ���ǵ� m/s abs ��
    public double CurRightSpeedMPS { get; private set; }//  ������ ���� ���ǵ� m/s abs ��

    public float Bpm { get => MathF.Abs(_save_bpm); }

    public bool IsConnect { get => _connect; } 

    private InputToken leftToken;
    private InputToken rightToken;

    private float _save_bpm = 0.0f;
    
    // ���ӵ� ���� ����
    private float prevLeftAsX = 0.0f;
    private float prevRightAsX = 0.0f;
    
    // ���� Meter ���� ����
    private double prevMeterL = 0.0;
    private double prevMeterR = 0.0;

    //  ������ ����
    private double moveMeterR = 0;
    private double moveMeterL = 0;

    // ���� ��� ����
    private double curMoveMeter;
    private double curcalc = 0;
    private GymchairConnectPopupController _popup;
    private bool _connect = false;
    private bool _meterWheel = false;
    private DataReceiver dataReceiver;
    public float FactorValue_1 { get => dataReceiver.FactorValue_1; }
    public float FactorValue_2 { get => dataReceiver.FactorValue_2; }
    public Action ReceivedEvent { get; set; }


    // ������ �̺�Ʈ �Է��� ���� �� �� �ð� (�� �� �� �ϳ��� �Է��� ������ �� �������� ��ȯ) 
    public float LastEventTime
    {
        get { return Math.Min(leftToken.LastEventTime, rightToken.LastEventTime); }
    }
  
    public float Save_left_speed { get => prevLeftAsX; set => prevLeftAsX = value;  }
    public float Save_right_speed { get => prevRightAsX; set => prevRightAsX = value; }
   
    public InputToken LeftToken { get => leftToken; }
    public InputToken RightToken { get => rightToken; }
    
    public double MoveMeterL { get => moveMeterL;}
    public double MoveMeterR { get => moveMeterR;}
    public double CurrentMoveMeter { get => curMoveMeter; }
    public double Calc { get => curcalc; }
    public void Init()
    {
       
        leftToken = new InputToken(FOR_PC_LEFT_KEY, INPUT_NEED_COUNT);
        rightToken = new InputToken(FOR_PC_RIGHT_KEY, INPUT_NEED_COUNT);
        SceneManager.sceneUnloaded += OnSceneUnloaded;
#if UNITY_EDITOR || NO_BLUETOOTH
        _connect = true;
#else
        Application.targetFrameRate = 60;
        ConnectToDevice();
#endif
        GenerateDataReceiver();
        Debug.Log("��ū �Ŵ��� ����!");

    }
    private void GenerateDataReceiver()
    {
        if(dataReceiver != null) 
        {
            return;
        }
        var go = Resources.Load("Prefabs/DataReceiver");
        var instanceGo = GameObject.Instantiate(go);
        instanceGo.name = "DataReceiver";
        GameObject.DontDestroyOnLoad(instanceGo);
        dataReceiver = instanceGo.GetComponent<DataReceiver>();
    }
  
    /// <summary>
    /// ���� ������ �Է��� ���ý� ����� �̺�Ʈ�� �߰��Ѵ�.
    /// </summary>
    /// <param name="action"></param>
    public void AddLeftTokenEvent(Action action)
    {
        leftToken.CallEvent -= action;
        leftToken.CallEvent += action;
    }

    /// <summary>
    /// ������ ������ �Է��� ���ý� ����� �̺�Ʈ�� �߰��Ѵ�.
    /// </summary>
    /// <param name="action"></param>
    public void AddRightTokenEvent(Action action)
    {
        rightToken.CallEvent -= action;
        rightToken.CallEvent += action;
    }
    public void ConnectToDevice() 
    {
        Managers.MonoForCoroutine.StartCoroutine(connectServer());
        //StartCoroutine(WaitStart());
    }
    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(1f);
        if (!_connect)
        {
            Managers.MonoForCoroutine.StartCoroutine(connectServer());
        }

    }

    public void OnConnected()
    {
        if (!this._connect)
        {
            this._connect = true;
        }
    }

    private readonly int FIRST_DEVICE_INDEX = 0;
    private readonly int SECOND_DEVICE_INDEX = 5;
    private readonly int AS_X_INDEX = 1;
    private readonly int METER_INDEX = 2;
    private readonly int SPEED_INDEX = 3;

    private readonly string LEFT_DEVICE_NAME = "WTwheelL";
    private readonly string RIGHT_DEVICE_NAME = "WTwheelR";

    private readonly int BPM_INDEX = 11;
    private readonly float MIN_CHECK_RPM = 900;
   

 

    private void MessageParse(string[] splitMessage ,int leftIndex , int rightIndex ) 
    {
        double curMeterL = Double.Parse(splitMessage[leftIndex + METER_INDEX]);
        double curMeterR = Double.Parse(splitMessage[rightIndex + METER_INDEX]);

        
        moveMeterL = curMeterL - prevMeterL;
        moveMeterR = curMeterR - prevMeterR;
       
        curMoveMeter = (moveMeterL + moveMeterR) *0.5f;
       
        prevMeterL = curMeterL;
        prevMeterR = curMeterR;

        // ���� ���� ������ ����
        var leftAsX = float.Parse(splitMessage[leftIndex + AS_X_INDEX]);
        if (math.abs(leftAsX) < 5) 
        {
            prevLeftAsX = 0;
        }
        else  
        {
            prevLeftAsX = leftAsX;
        }
       

        CurLeftSpeedMPS = float.Parse(splitMessage[leftIndex + SPEED_INDEX]); // ���� m/s �̿� ���
        if (math.abs(CurLeftSpeedMPS) < 0.1)
        {
            CurLeftSpeedMPS = 0;
        }


        // ������ ���� ������ ����
        var rightAsX = float.Parse(splitMessage[rightIndex + AS_X_INDEX]);

        if (math.abs(rightAsX) < 5)
        {
            prevRightAsX = 0;
        }
        else
        {
            prevRightAsX = rightAsX;
        }

        CurRightSpeedMPS = float.Parse(splitMessage[rightIndex + SPEED_INDEX]);
        if (math.abs(CurRightSpeedMPS) < 0.1)
        {
            CurRightSpeedMPS = 0;
        }

        CurSpeedMeterPerSec = (CurLeftSpeedMPS + CurRightSpeedMPS) * 0.5f;
    }
    public string ForDebugDataMEssage(string[] splitMessage)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(splitMessage[FIRST_DEVICE_INDEX]);
        stringBuilder.Append($" AsX : {splitMessage[FIRST_DEVICE_INDEX + AS_X_INDEX]}\t");
        stringBuilder.Append($" total m : {splitMessage[FIRST_DEVICE_INDEX + METER_INDEX]}\t");
        stringBuilder.Append($" calc: {splitMessage[FIRST_DEVICE_INDEX + SPEED_INDEX]}\t");
        stringBuilder.Append(splitMessage[SECOND_DEVICE_INDEX]);
        stringBuilder.Append($" AsX : {splitMessage[SECOND_DEVICE_INDEX + AS_X_INDEX]}\t");
        stringBuilder.Append($" total m : {splitMessage[SECOND_DEVICE_INDEX + METER_INDEX]}\t");
        stringBuilder.Append($" calc : {splitMessage[SECOND_DEVICE_INDEX + SPEED_INDEX]}\t");
        return stringBuilder.ToString();
    }
    public void OnReceivedMessage(string message)
    {
            int count = message.Length;
            var splitMessage = message.Split(',');
            //Debug.Log($" �޼��� ���� : {message}");

            
        //dataReceiver.
            if (splitMessage[FIRST_DEVICE_INDEX] == LEFT_DEVICE_NAME)
            {
                MessageParse(splitMessage, FIRST_DEVICE_INDEX, SECOND_DEVICE_INDEX);
            }
            else
            {
                MessageParse(splitMessage, SECOND_DEVICE_INDEX, FIRST_DEVICE_INDEX);
            }

            //Debug.Log($"LEFT_RPM : {splitMessage[X_AXIS_SPEED_INDEX_L]}");
            //Debug.Log($"RIGHT_RPM : {splitMessage[Y_AXIS_SPEED_INDEX_L]}");
            _save_bpm = float.Parse(splitMessage[BPM_INDEX]);
           
            ReceivedEvent?.Invoke();
          //  Debug.Log("LEFT_RPM : " + _save_left_speed + " RIGHT_RPM : " + _save_right_speed);

            if (prevLeftAsX > MIN_CHECK_RPM) 
            {
                leftToken.CallEvent?.Invoke();
            }
            if(prevRightAsX > MIN_CHECK_RPM) 
            {
                rightToken.CallEvent?.Invoke();
            }
            // ����� �� �ؽ�Ʈ   
            dataReceiver.SetText(ForDebugDataMEssage(splitMessage));
            dataReceiver.ClearMsg();
    }
    IEnumerator connectServer()
    {
        _popup = GymchairConnectPopupController.Create();
        
        int count = 0;

        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            if (dataReceiver.IsConect) 
            {
                _connect = true;
                break;
            }

            count++;

            if (count > 10)
            {
                _popup.gameObject.SetActive(false);

                Information01Popup.Create(() =>
                {
                    _popup.gameObject.SetActive(true);
                    Managers.MonoForCoroutine.StartCoroutine(connectServer());
                }, () =>
                {
                    GameObject.Destroy(_popup.gameObject);

                    Managers.Scene.LoadScene(E_SceneName.SelectMenu);
                });

                yield break;
            }
        }
        _popup.ShowSuccess();
        yield return new WaitForSeconds(1.0f);
       GameObject.Destroy(_popup.gameObject);
    }
        public void OnDisconnect()
        {
            if (this._connect)
            {
                Debug.Log("������� ������ ������ϴ�!");
                this._connect = false;

                Information01Popup.Create(() =>
                {
                    Managers.Sound.PlayTouchEffect();
                    _popup = GymchairConnectPopupController.Create();
                    Managers.MonoForCoroutine.StartCoroutine(ReConnectServer());
                }, () =>
                {
                    Managers.Sound.PlayTouchEffect();
                    GameObject.Destroy(_popup.gameObject);

                    Managers.Scene.LoadScene(E_SceneName.SelectMenu);
                });
            }
        }

        IEnumerator ReConnectServer()
        {
            int count = 0;

            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (dataReceiver.IsConect)
                {
                    Debug.Log("���� ���� !");
                    _connect = true;
                    break;
                }

                count++;

                if (count > 10)
                {
                    _popup.gameObject.SetActive(false);

                    Information01Popup.Create(() =>
                    {
                        Managers.Sound.PlayTouchEffect();
                        _popup.gameObject.SetActive(true);
                        Managers.MonoForCoroutine.StartCoroutine(connectServer());
                    }, () =>
                    {
                        Managers.Sound.StopBGM();
                        Managers.Sound.PlayTouchEffect();
                        GameObject.Destroy(_popup.gameObject);
                        Managers.Scene.LoadScene(E_SceneName.SelectMenu);
                    });
                    yield break;
                }
            }

            _popup.ShowSuccess();
            yield return new WaitForSeconds(1.0f);
            GameObject.Destroy(_popup.gameObject);
        }
        void OnSceneUnloaded(Scene scene)
        {
            _save_bpm = 0;
            prevLeftAsX = 0;
            prevRightAsX = 0;        
            leftToken.CallEvent = null;
            rightToken.CallEvent = null;
            ReceivedEvent = null;
    
    }
}

