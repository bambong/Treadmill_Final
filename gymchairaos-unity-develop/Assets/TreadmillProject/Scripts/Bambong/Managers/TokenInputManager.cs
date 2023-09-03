using bambong;
using Gymchair.Contents.Popup;
using Gymchair.Core.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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


public class TokenInputManager : GameObjectSingletonDestroy<TokenInputManager>, IInit
{


    [SerializeField]
    private KeyCode leftKey = KeyCode.A;
    [SerializeField]
    private KeyCode rightKey = KeyCode.D;

    [Header("Input 으로 판단할 token 수 ")]
    [SerializeField]
    private int inputNeedCount = 1;


    // 왼쪽 바퀴 입력 텀    
    public float LeftTokenTerm { get => leftToken.TokenEventTerm; }
    // 오른쪽 바퀴 입력 텀
    public float RightTokenTerm { get => rightToken.TokenEventTerm; }

    [Obsolete("미터 기준")]
    public float CurSpeed { get => CurRpm * 240.0f / 60000.0f; }
    public float CurRpm { get { return _save_left_speed + _save_right_speed * 0.5f; } }
    public float Bpm { get => _save_bpm; }
    public bool IsConnect { get => _connect; } 

    private InputToken leftToken;
    private InputToken rightToken;

    private float _save_bpm = 0.0f;
    private float _save_left_speed = 0.0f;
    private float _save_right_speed = 0.0f;

    private GymchairConnectPopupController _popup;
    private bool _connect = false;
    public Action ReceivedEvent { get; set; }

    // 마지막 이벤트 입력이 들어온 이 후 시간 (두 개 중 하나라도 입력이 들어오면 더 작은쪽을 반환) 
    public float LastEventTime
    {
        get { return Math.Min(leftToken.LastEventTime, rightToken.LastEventTime); }
    }

    public float Save_left_rpm { get => _save_left_speed; set => _save_left_speed = value;  }
    public float Save_right_rpm { get => _save_right_speed; set => _save_right_speed = value; }

    public void Init()
    {
        leftToken = new InputToken(leftKey, inputNeedCount);
        rightToken = new InputToken(rightKey, inputNeedCount);
    }
    
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
#if UNITY_EDITOR || NO_BLUETOOTH
        _connect = true;
#else
        ConnectToDevice();
#endif
        Debug.Log("토큰 매니저 시작!");

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
        StartCoroutine(connectServer());
        //StartCoroutine(WaitStart());
    }
    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(1f);
        if (!_connect)
        {
            StartCoroutine(connectServer());
        }

    }
    void Update()
    {
#if UNITY_EDITOR || NO_BLUETOOTH
        leftToken.InputUpdate();
        rightToken.InputUpdate();
#else
        if(_connect && DataReceiver.Instance.LastMsg != string.Empty) 
        {
            OnReceivedMessage(DataReceiver.Instance.LastMsg);
        }
#endif

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

    private readonly string LEFT_DEVICE_NAME = "WTwheelL";
    private readonly string RIGHT_DEVICE_NAME = "WTwheelR";

    private readonly int BPM_INDEX = 9;
    private readonly float MIN_CHECK_RPM = 0;
   
    private float GetSpeed( float y ) 
    {
        return y * DataReceiver.Instance.FactorValue; 
        // 각속도 -> 속도 계산법 (V = (2π X 반지름 X 각속도) / 360
    }
    public void OnReceivedMessage(string message)
    {

            Debug.Log($"메세지 받음 : {message}");
            int count = message.Length;
            var splitMessage = message.Split(',');
            
            if(splitMessage[FIRST_DEVICE_INDEX] == LEFT_DEVICE_NAME) 
            {
                _save_left_speed = GetSpeed(float.Parse(splitMessage[FIRST_DEVICE_INDEX + 2]));
                _save_right_speed = GetSpeed(float.Parse(splitMessage[SECOND_DEVICE_INDEX + 2]));
            }
            else if(splitMessage[FIRST_DEVICE_INDEX] == RIGHT_DEVICE_NAME)
            {
                _save_left_speed = GetSpeed(float.Parse(splitMessage[SECOND_DEVICE_INDEX + 2]));
                _save_right_speed = GetSpeed(float.Parse(splitMessage[FIRST_DEVICE_INDEX + 2]));
             }

            //Debug.Log($"LEFT_RPM : {splitMessage[X_AXIS_SPEED_INDEX_L]}");
            //Debug.Log($"RIGHT_RPM : {splitMessage[Y_AXIS_SPEED_INDEX_L]}");
           // _save_bpm = float.Parse(splitMessage[BPM_INDEX]);
           
            ReceivedEvent?.Invoke();
            Debug.Log("LEFT_RPM : " + _save_left_speed + " RIGHT_RPM : " + _save_right_speed);

            if (_save_left_speed > MIN_CHECK_RPM) 
            {
                leftToken.CallEvent();
            }
            if(_save_right_speed > MIN_CHECK_RPM) 
            {
                rightToken.CallEvent();
            }

            DataReceiver.Instance.ClearMsg();
       
    }
    IEnumerator connectServer()
    {
        _popup = GymchairConnectPopupController.Create();
        
        int count = 0;

        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            if (DataReceiver.Instance.IsConect) 
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
                    StartCoroutine(connectServer());
                }, () =>
                {
                    Destroy(_popup.gameObject);

                    TransitionManager.Instance.TransitionToSelectScene();
                });

                yield break;
            }
        }
        _popup.ShowSuccess();
        yield return new WaitForSeconds(1.0f);
        Destroy(_popup.gameObject);
    }
        public void OnDisconnect()
        {
            if (this._connect)
            {
                Debug.Log("블루투스 연결이 끊겼습니다!");
                this._connect = false;

                Information01Popup.Create(() =>
                {
                    SoundMgr.Instance.PlayEffect("touch");
                    _popup = GymchairConnectPopupController.Create();
                    StartCoroutine(ReConnectServer());
                }, () =>
                {
                    SoundMgr.Instance.PlayEffect("touch");
                    Destroy(_popup.gameObject);

                    TransitionManager.Instance.TransitionToSelectScene();
                });
            }
        }

        IEnumerator ReConnectServer()
        {
            int count = 0;

            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (DataReceiver.Instance.IsConect)
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
                        SoundMgr.Instance.PlayEffect("touch");
                        _popup.gameObject.SetActive(true);
                        StartCoroutine(connectServer());
                    }, () =>
                    {
                        SoundMgr.Instance.StopBGM();
                        SoundMgr.Instance.PlayEffect("touch");
                        Destroy(_popup.gameObject);
                        TransitionManager.Instance.TransitionToSelectScene();
                    });
                    yield break;
                }
            }

            _popup.ShowSuccess();
            yield return new WaitForSeconds(1.0f);
            Destroy(_popup.gameObject);
        }

    private void OnDisable()
    {
        _connect = false;
    }
}

