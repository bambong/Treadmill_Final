using bambong;
using Gymchair.Contents.Popup;
using Gymchair.Core.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public float CurRpm { get { return (_save_left_rpm + _save_right_rpm) * 0.5f; } }
    public float Bpm { get => _save_bpm; }
    public bool IsConnect { get => _connect; } 

    private InputToken leftToken;
    private InputToken rightToken;

    private float _save_bpm = 0.0f;
    private float _save_left_rpm = 0.0f;
    private float _save_right_rpm = 0.0f;

    private GymchairConnectPopupController _popup;
    private bool _connect = false;
    public Action ReceivedEvent { get; set; }

    // ������ �̺�Ʈ �Է��� ���� �� �� �ð� (�� �� �� �ϳ��� �Է��� ������ �� �������� ��ȯ) 
    public float LastEventTime
    {
        get { return Math.Min(leftToken.LastEventTime, rightToken.LastEventTime); }
    }

    public float Save_left_rpm { get => _save_left_rpm; set => _save_left_rpm = value; }
    public float Save_right_rpm { get => _save_right_rpm; set => _save_right_rpm = value; }

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
#if UNITY_EDITOR
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
          StartCoroutine(WaitStart());
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
#if UNITY_EDITOR
        leftToken.InputUpdate();
        rightToken.InputUpdate();
#endif

    }
    public void OnConnected()
    {
        if (!this._connect)
        {
            this._connect = true;
        }
    }

    private readonly int LEFT_RPM_INDEX = 1;
    private readonly int RIGHT_RPM_INDEX = 5;
    private readonly int BPM_INDEX = 9;
    private readonly int MIN_CHECK_RPM = 0;
   
    
    public void OnReceivedMessage(string message)
    {
        OnConnected();
        try
        {
            int count = message.Length;
            Debug.Log($"�޼��� ���� : {message}");
            var splitMessage = message.Split('/');


            _save_left_rpm = float.Parse(splitMessage[LEFT_RPM_INDEX]);
            Debug.Log($"LEFT_RPM : {splitMessage[LEFT_RPM_INDEX]}");
            _save_right_rpm = float.Parse(splitMessage[RIGHT_RPM_INDEX]);
            Debug.Log($"RIGHT_RPM : {splitMessage[RIGHT_RPM_INDEX]}");
            _save_bpm = float.Parse(splitMessage[BPM_INDEX]);
           
            ReceivedEvent?.Invoke();

            if(_save_left_rpm > MIN_CHECK_RPM) 
            {
                leftToken.CallEvent();
            }
            if(_save_right_rpm > MIN_CHECK_RPM) 
            {
                rightToken.CallEvent();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    IEnumerator connectServer()
    {
        _popup = GymchairConnectPopupController.Create();
        if (!BluetoothMgr.Instance.isConnect())
            BluetoothMgr.Instance.Connect("wheelchair");
        
        int count = 0;

        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            if (this._connect)
                break;

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
            BluetoothMgr.Instance.Connect("wheelchair");

            int count = 0;

            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (this._connect)
                    break;

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
    private void OnEnable()
    { 
#if !UNITY_EDITOR
        if (BluetoothMgr.Instance)
        {
            BluetoothMgr.Instance._actionConnect += OnConnected;
            BluetoothMgr.Instance._actionReceivedMessage += OnReceivedMessage;
            BluetoothMgr.Instance._actionDisconnect += OnDisconnect;
        }
#endif
    }
    private void OnDisable()
    {
#if !UNITY_EDITOR
        if (BluetoothMgr.Instance)
        {
            BluetoothMgr.Instance._actionConnect -= OnConnected;
            BluetoothMgr.Instance._actionReceivedMessage -= OnReceivedMessage;
            BluetoothMgr.Instance._actionDisconnect -= OnDisconnect;
            ReceivedEvent = null;
        }

        _connect = false;
        Debug.Log("OnDisable");
        BluetoothMgr.Instance.Disconnect();
#endif
    }
}

