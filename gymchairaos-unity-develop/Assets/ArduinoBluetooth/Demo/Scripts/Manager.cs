using System;
using System.Collections;
using System.Collections.Generic;
using ArduinoBluetoothAPI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

    // Use this for initialization
    BluetoothHelper bluetoothHelper;
    string Bluetooth_DeviceName;

    [SerializeField]
    InputField InputField_DeviceName;
    [SerializeField]
    Toggle Toggle_isDevicePaired;
    [SerializeField]
    Toggle Toggle_isConnected;
    [SerializeField]
    Button Btn_Initial;
    [SerializeField]
    Button Btn_Connect;
    [SerializeField]
    Button Btn_Disconnect;
    [SerializeField]
    Button Btn_Send;

    public Text Text_Debug;
    string string_ReceivedMessage;

    void Start()
    {
        Btn_Connect.onClick.AddListener(() =>
        {
            if (bluetoothHelper.isDevicePaired())
            {
                Debug.Log("try to connect");
                Text_Debug.text += "try to connect\n";
                bluetoothHelper.Connect();
            }
            else
            {
                Debug.Log("not DevicePaired");
                Text_Debug.text += "not DevicePaired\n";
            }
        });

        Btn_Disconnect.onClick.AddListener(() =>
        {
            bluetoothHelper.Disconnect();
            Debug.Log("try to Disconnect");
            Text_Debug.text += "try to connect\n";
        });

        Btn_Send.onClick.AddListener(() =>
        {
            bluetoothHelper.SendData("Hello");
            Text_Debug.text += "SendData Hello\n";
        });

        Btn_Initial.onClick.AddListener(() =>
        {
            Text_Debug.text = "";
            Bluetooth_DeviceName = InputField_DeviceName.text;
            try
            {
                bluetoothHelper = BluetoothHelper.GetInstance(Bluetooth_DeviceName);
                bluetoothHelper.OnConnected += OnConnected;
                bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
                bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data

                bluetoothHelper.setTerminatorBasedStream("\n");

                if (bluetoothHelper.isDevicePaired())
                    Toggle_isDevicePaired.isOn = true;
                else
                    Toggle_isDevicePaired.isOn = false;
            }
            catch (Exception ex)
            {
                Toggle_isDevicePaired.isOn = false;
                Debug.Log(ex.Message);
                Text_Debug.text += ex.Message + '\n';
            }
        });

        Bluetooth_DeviceName = InputField_DeviceName.text;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Asynchronous method to receive messages
    void OnMessageReceived()
    {
        string_ReceivedMessage = bluetoothHelper.Read();
        Debug.Log(string_ReceivedMessage);
        Text_Debug.text += string_ReceivedMessage + '\n';
    }

    void OnConnected()
    {
        Toggle_isConnected.isOn = true;
        try
        {
            bluetoothHelper.StartListening();
            Debug.Log("Connected");
            Text_Debug.text += "Connected" + '\n';
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Text_Debug.text += ex.Message + '\n';
        }
    }

    void OnConnectionFailed()
    {
        Toggle_isConnected.isOn = false;
        Debug.Log("Connection Failed");
        Text_Debug.text += "Connection Failed" + '\n';
    }

    void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }

    void OnApplicationQuit()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }
}