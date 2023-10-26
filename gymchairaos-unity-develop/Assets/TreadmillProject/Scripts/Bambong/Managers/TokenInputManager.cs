
using Gymchair.Contents.Popup;
using System;
using System.Collections;
using System.Runtime.Remoting.Contexts;
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

    // 왼쪽 바퀴 입력 텀    
    public float LeftTokenTerm { get => leftToken.TokenEventTerm; }
    // 오른쪽 바퀴 입력 텀
    public float RightTokenTerm { get => rightToken.TokenEventTerm; }
    private const float METER_FACTOR = 0.4396f; // 2f * PI * 0.07f(반지름)

    public float CurSpeedMeterPerSec { get => (METER_FACTOR * CurRpm) / 360.0f; } // 평균 바퀴 스피드 m/s  abs 값
    public float CurRpm { get { return math.abs(_save_left_speed) + math.abs(_save_right_speed) * 0.5f; } }
    public float CurLeftSpeedMPS { get => (METER_FACTOR * math.abs(_save_left_speed)) / 360.0f; }// 왼쪽 바퀴 스피드 m/s abs 값
    public float CurRightSpeedMPS { get => (METER_FACTOR * math.abs(_save_right_speed)) / 360.0f; }//  오른쪽 바퀴 스피드 m/s abs 값
    public float Bpm { get => MathF.Abs(_save_bpm); }
    public bool IsConnect { get => _connect; } 

    private InputToken leftToken;
    private InputToken rightToken;

    private float _save_bpm = 0.0f;
    private float _save_left_speed = 0.0f;
    private float _save_right_speed = 0.0f;

    private GymchairConnectPopupController _popup;
    private bool _connect = false;

    private DataReceiver dataReceiver;
    public float FactorValue { get => dataReceiver.FactorValue; }
    public Action ReceivedEvent { get; set; }

    // 마지막 이벤트 입력이 들어온 이 후 시간 (두 개 중 하나라도 입력이 들어오면 더 작은쪽을 반환) 
    public float LastEventTime
    {
        get { return Math.Min(leftToken.LastEventTime, rightToken.LastEventTime); }
    }
  
    public float Save_left_speed { get => _save_left_speed; set => _save_left_speed = value;  }
    public float Save_right_speed { get => _save_right_speed; set => _save_right_speed = value; }
    public InputToken LeftToken { get => leftToken; }
    public InputToken RightToken { get => rightToken; }

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
    private readonly int AXIS_INDEX = 1;

    private readonly string LEFT_DEVICE_NAME = "WTwheelL";
    private readonly string RIGHT_DEVICE_NAME = "WTwheelR";

    private readonly int BPM_INDEX = 11;
    private readonly float MIN_CHECK_RPM = 900;
   
    private float GetSpeed( float v ) 
    {
        return v * FactorValue * -1;
        
        // 각속도 -> 속도 계산법 (V = (2π X 반지름 X 각속도) / 360
    }
    public void OnReceivedMessage(string message)
    {
            int count = message.Length;
            var splitMessage = message.Split(',');
            Debug.Log($" 메세지 받음 : {message}");
            if (splitMessage[FIRST_DEVICE_INDEX] == LEFT_DEVICE_NAME) 
            {
                _save_left_speed = GetSpeed(float.Parse(splitMessage[FIRST_DEVICE_INDEX + AXIS_INDEX]));
                _save_right_speed = GetSpeed(float.Parse(splitMessage[SECOND_DEVICE_INDEX + AXIS_INDEX]));
            }
            else if(splitMessage[FIRST_DEVICE_INDEX] == RIGHT_DEVICE_NAME)
            {
                _save_left_speed = GetSpeed(float.Parse(splitMessage[SECOND_DEVICE_INDEX + AXIS_INDEX]));
                _save_right_speed = GetSpeed(float.Parse(splitMessage[FIRST_DEVICE_INDEX + AXIS_INDEX]));
             }

            //Debug.Log($"LEFT_RPM : {splitMessage[X_AXIS_SPEED_INDEX_L]}");
            //Debug.Log($"RIGHT_RPM : {splitMessage[Y_AXIS_SPEED_INDEX_L]}");
            _save_bpm = float.Parse(splitMessage[BPM_INDEX]);
           
            ReceivedEvent?.Invoke();
          //  Debug.Log("LEFT_RPM : " + _save_left_speed + " RIGHT_RPM : " + _save_right_speed);

            if (_save_left_speed > MIN_CHECK_RPM) 
            {
                leftToken.CallEvent?.Invoke();
            }
            if(_save_right_speed > MIN_CHECK_RPM) 
            {
                rightToken.CallEvent?.Invoke();
            }
          
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
            _save_left_speed = 0;
            _save_right_speed = 0;        
            leftToken.CallEvent = null;
            rightToken.CallEvent = null;
            ReceivedEvent = null;
    
    }
}

