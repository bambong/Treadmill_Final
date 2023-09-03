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


public class TokenInputManager : GameObjectSingletonDestroy<TokenInputManager>, IInit
{


    [SerializeField]
    private KeyCode leftKey = KeyCode.A;
    [SerializeField]
    private KeyCode rightKey = KeyCode.D;

    [Header("Input ���� �Ǵ��� token �� ")]
    [SerializeField]
    private int inputNeedCount = 1;


    // ���� ���� �Է� ��    
    public float LeftTokenTerm { get => leftToken.TokenEventTerm; }
    // ������ ���� �Է� ��
    public float RightTokenTerm { get => rightToken.TokenEventTerm; }

    [Obsolete("���� ����")]
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

    // ������ �̺�Ʈ �Է��� ���� �� �� �ð� (�� �� �� �ϳ��� �Է��� ������ �� �������� ��ȯ) 
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
        Debug.Log("��ū �Ŵ��� ����!");

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
        // ���ӵ� -> �ӵ� ���� (V = (2�� X ������ X ���ӵ�) / 360
    }
    public void OnReceivedMessage(string message)
    {

            Debug.Log($"�޼��� ���� : {message}");
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
                Debug.Log("������� ������ ������ϴ�!");
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

