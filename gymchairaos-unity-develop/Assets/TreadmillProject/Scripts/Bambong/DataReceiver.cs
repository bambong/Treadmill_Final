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

    private float factorValue = 0.5f;  // 민감도 테스트를 위한 기준 값
    private string lastMsg = "";

    private UdpClient udpClient;
    private IPEndPoint endPoint;

    private readonly int UDP_PORT = 12345; // 서버 포트 번호
    private readonly string SERVER_IP_ADDRESS = "127.0.0.1"; // 서버 IP 주소 (로컬 호스트)

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
                // UDP 서버로부터 데이터 수신
                UdpReceiveResult receivedResult = await udpClient.ReceiveAsync();
                byte[] receivedBytes = receivedResult.Buffer; // 수신한 데이터를 바이트 배열로 추출
                string receivedMessage = System.Text.Encoding.UTF8.GetString(receivedBytes);


                // 수신한 데이터 처리 로직을 여기에 작성
                Debug.Log("Received data from server: " + receivedMessage);
                // 수신한 데이터를 원하는 방식으로 처리
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

#if UNITY_EDITOR || NO_BLUETOOTH // 에디터에서 플레이시 키보드 입력을 받도록 
        Managers.Token.LeftToken.InputUpdate();
        Managers.Token.RightToken.InputUpdate();
#endif
    }
    private void UpdateValue() 
    {
        factor.text = $"기준 : {factorValue}";  
    }
    public void ClearMsg() => lastMsg = string.Empty;

    public void OnReceive(string msg) 
    {
        if (!isConect) 
        {
            isConect = true;
        }

        text.text = $"받은 메세지 : {msg}";
        Managers.Token.OnReceivedMessage(msg);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearMsg();
    }


}
