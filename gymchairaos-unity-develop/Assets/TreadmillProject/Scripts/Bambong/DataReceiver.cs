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
    private TextMeshProUGUI factor;

    private float factorValue = 0.5f;  // �ΰ��� �׽�Ʈ�� ���� ���� ��
    private string lastMsg = "";

    private UdpClient udpClient;
    private IPEndPoint endPoint;

    private readonly int UDP_PORT = 12345; // ���� ��Ʈ ��ȣ
    private readonly string SERVER_IP_ADDRESS = "127.0.0.1"; // ���� IP �ּ� (���� ȣ��Ʈ)

    private bool isConect;
    public string LastMsg { get => lastMsg; }
    public bool IsConect { get => isConect;  }
    public float FactorValue { get => factorValue;  }


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


                // ������ ������ ó�� ������ ���⿡ �ۼ�
                Debug.Log("Received data from server: " + receivedMessage);
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
        factorValue += value; 
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
        factor.text = $"���� : {factorValue}";  
    }
    public void ClearMsg() => lastMsg = string.Empty;

    public void OnReceive(string msg) 
    {
        if (!isConect) 
        {
            isConect = true;
        }

        text.text = $"���� �޼��� : {msg}";
        Managers.Token.OnReceivedMessage(msg);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearMsg();
    }


}
