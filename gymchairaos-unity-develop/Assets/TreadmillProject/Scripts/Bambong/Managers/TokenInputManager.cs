
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
    // 어떤 키를 입력으로 받을지 
    private KeyCode keyCode;
    // 현재 회전 수 
    private int curTokenCount;
    // 몇번의 회전이 모였을 떄 입력으로 인정할 지
    private int inputNeedCount;

    private float lastEventTime;

    // 일정 입력이 인정되었을 때 발생시킬 이벤트
    public Action CallEvent { get; set; }
    // 입력 사이의 텀 -> 해당 데이터로 속력을 산출
    public float TokenEventTerm { get; private set; } = float.MaxValue;
    // 마지막 이벤트 입력이 들어온 이 후 시간
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
    private float METER_FACTOR = 1.884f; // PI * 7f(지름) * 0.01(METER)

    // 왼쪽 바퀴 입력 텀    
    public float LeftTokenTerm { get => leftToken.TokenEventTerm; }
    // 오른쪽 바퀴 입력 텀
    public float RightTokenTerm { get => rightToken.TokenEventTerm; }

    public double CurSpeedMeterPerSec { get; private set; } // 평균 바퀴 스피드 m/s  abs 값
    public float CurRpm { get { return math.abs(prevLeftAsX) + math.abs(prevRightAsX) * 0.5f; } }
    public double CurLeftSpeedMPS { get; private set; }// 왼쪽 바퀴 스피드 m/s abs 값
    public double CurRightSpeedMPS { get; private set; }//  오른쪽 바퀴 스피드 m/s abs 값

    public float Bpm { get => MathF.Abs(_save_bpm); }

    public bool IsConnect { get => _connect; } 

    private InputToken leftToken;
    private InputToken rightToken;

    private float _save_bpm = 0.0f;
    
    // 각속도 저장 변수
    private float prevLeftAsX = 0.0f;
    private float prevRightAsX = 0.0f;
    
    // 이전 Meter 저장 변수
    private double prevMeterL = 0.0;
    private double prevMeterR = 0.0;

    //  움직인 미터
    private double moveMeterR = 0;
    private double moveMeterL = 0;

    // 보정 계산 미터
    private double curMoveMeter;
    private double curcalc = 0;
    private GymchairConnectPopupController _popup;
    private bool _connect = false;
    private bool _meterWheel = false;
    private DataReceiver dataReceiver;
    public float FactorValue_1 { get => dataReceiver.FactorValue_1; }
    public float FactorValue_2 { get => dataReceiver.FactorValue_2; }
    public Action ReceivedEvent { get; set; }


    // 마지막 이벤트 입력이 들어온 이 후 시간 (두 개 중 하나라도 입력이 들어오면 더 작은쪽을 반환) 
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
        Debug.Log("토큰 매니저 시작!");

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
    /// 왼쪽 바퀴에 입력이 들어올시 실행될 이벤트를 추가한다.
    /// </summary>
    /// <param name="action"></param>
    public void AddLeftTokenEvent(Action action)
    {
        leftToken.CallEvent -= action;
        leftToken.CallEvent += action;
    }

    /// <summary>
    /// 오른쪽 바퀴에 입력이 들어올시 실행될 이벤트를 추가한다.
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

        // 왼쪽 바퀴 데이터 매핑
        var leftAsX = float.Parse(splitMessage[leftIndex + AS_X_INDEX]);
        if (math.abs(leftAsX) < 5) 
        {
            prevLeftAsX = 0;
        }
        else  
        {
            prevLeftAsX = leftAsX;
        }
       

        CurLeftSpeedMPS = float.Parse(splitMessage[leftIndex + SPEED_INDEX]); // 직접 m/s 이용 방식
        if (math.abs(CurLeftSpeedMPS) < 0.1)
        {
            CurLeftSpeedMPS = 0;
        }


        // 오른쪽 바퀴 데이터 매핑
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
            //Debug.Log($" 메세지 받음 : {message}");

            
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
            // 디버그 용 텍스트   
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
                Debug.Log("블루투스 연결이 끊겼습니다!");
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
                    Debug.Log("연결 성공 !");
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

