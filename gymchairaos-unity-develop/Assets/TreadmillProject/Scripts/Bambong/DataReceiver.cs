using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System;

public class DataReceiver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TMP_InputField factor_1;
    [SerializeField]
    private TMP_InputField factor_2;

    private float factorValue_1 = 300f;  // �ΰ��� �׽�Ʈ�� ���� ���� ��
    private float factorValue_2 = 0; 
    private string lastMsg = "";

    private UdpClient udpClient;
    private IPEndPoint endPoint;

    private readonly int UDP_PORT = 12345; // ���� ��Ʈ ��ȣ
    private readonly string SERVER_IP_ADDRESS = "127.0.0.1"; // ���� IP �ּ� (���� ȣ��Ʈ)
    private readonly string END_EVENT_KEY = "GYMCHAIR END"; // ���� IP �ּ� (���� ȣ��Ʈ)

    private bool isConect;
    public string LastMsg { get => lastMsg; }
    public bool IsConect { get => isConect;  }
    public float FactorValue_1 { get => factorValue_1;  }
    public float FactorValue_2 { get => factorValue_2;  }


    private void Start()
    {

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateValue();
#if UNITY_EDITOR || NO_BLUETOOTH
        isConect = true;
#else
        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(SERVER_IP_ADDRESS), UDP_PORT);
       
        if (udpClient != null && endPoint != null)
        {
            SendDataToServer("UNITY UDP OK");
            StartReceivingData();
        }
#endif
    }
    public void OnValueChange_1() 
    {
        float num = 0;
        if(!float.TryParse(factor_1.text,out num)) 
        {
            return;
        }
        factorValue_1 = num;
    }

    public void OnValueChange_2()
    {
        float num = 0;
        if (!float.TryParse(factor_2.text, out num))
        {
            return;
        }
        factorValue_2 = num;
    }
    public void SendDataToServer(string message)
    {
        try
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, endPoint);
            Debug.Log("Sent to server: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }
    private async void StartReceivingData()
    {
        while (true)
        {
            try
            {
                // UDP �����κ��� ������ ����
                UdpReceiveResult receivedResult = await udpClient.ReceiveAsync();
                byte[] receivedBytes = receivedResult.Buffer; // ������ �����͸� ����Ʈ �迭�� ����
                string receivedMessage = System.Text.Encoding.UTF8.GetString(receivedBytes);

                // ������ �����͸� ���ϴ� ������� ó��
                OnReceive(receivedMessage);

             
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data: " + e.Message);
            }
        }
    }

    public void AddValue(float value) 
    {
        factorValue_1 = (FactorValue_1*1000 + value*1000)/1000; 
        UpdateValue();
    }
    private void Update()
    {

#if UNITY_EDITOR || NO_BLUETOOTH // �����Ϳ��� �÷��̽� Ű���� �Է��� �޵��� 
        Managers.Token.LeftToken.InputUpdate();
        Managers.Token.RightToken.InputUpdate();
#endif
    }
    private void UpdateValue() 
    {
        factor_1.text = factorValue_1.ToString();  
    }
    public void ClearMsg() => lastMsg = string.Empty;

    public void OnReceive(string msg) 
    {
        if (!isConect) 
        {
            isConect = true;
        }

        Managers.Token.OnReceivedMessage(msg);
    }
    public void SetText(string msg)
    {
        text.text = msg;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearMsg();
    }
    public void OnEnterQuitButton()
    {
        SendDataToServer(END_EVENT_KEY); // �� ����� ���� �ۿ��Ե� ���� ��ȣ �߻�
        Debug.Log("END EVENT");
        Application.Quit();
    }
    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        SendDataToServer(END_EVENT_KEY); // �� ����� ���� �ۿ��Ե� ���� ��ȣ �߻�
           Debug.Log("END EVENT");
#endif
    }

}

