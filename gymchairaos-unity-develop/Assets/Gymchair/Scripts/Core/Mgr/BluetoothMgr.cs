using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using ArduinoBluetoothAPI;

namespace Gymchair.Core.Mgr
{
    public class BluetoothMgr : Behaviour.ManagerBehaviour<BluetoothMgr>
    {
        public event Action _actionConnect;
        public event Action _actionDisconnect;
        public event Action<string> _actionReceivedMessage;
        BluetoothHelper bluetoothHelper;

        BluetoothDevice[] _devices = null;
        public BluetoothDevice[] Devices { get => _devices; }

        bool _connect = false;

        void OnApplicationQuit()
        {
            if (bluetoothHelper != null)
            {
                Debug.Log("BluetoothMgr OnApplicationQuit Disconnect");
                bluetoothHelper.OnConnected -= OnConnected;
                bluetoothHelper.OnConnectionFailed -= OnConnectionFailed;
                bluetoothHelper.OnDataReceived -= OnMessageReceived; //read the data

                bluetoothHelper.Disconnect();
                _connect = false;
            }
        }

        public void Connect(string name)
        {
            if (bluetoothHelper != null)
            {
                Debug.Log("BluetoothMgr Connect Disconnect");
                bluetoothHelper.OnConnected -= OnConnected;
                bluetoothHelper.OnConnectionFailed -= OnConnectionFailed;
                bluetoothHelper.OnDataReceived -= OnMessageReceived; //read the data

                bluetoothHelper.Disconnect();
                _connect = false;
            }

            bluetoothHelper = BluetoothHelper.GetInstance(name);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data

            bluetoothHelper.setTerminatorBasedStream("\n");

            try
            {
                bluetoothHelper.Connect();
                Debug.Log("Connected");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        public bool isConnect()
        {
            return _connect;
        }

        public void Disconnect()
        {
            Debug.Log("Disconnect");
            if (bluetoothHelper != null)
            {
                Debug.Log("BluetoothMgr Disconnect");
                bluetoothHelper.OnConnected -= OnConnected;
                bluetoothHelper.OnConnectionFailed -= OnConnectionFailed;
                bluetoothHelper.OnDataReceived -= OnMessageReceived; //read the data

                bluetoothHelper.Disconnect();
                Debug.Log("bluetoothHelper.Disconnect();");

                bluetoothHelper = null;
                _connect = false;
            }
        }

        public bool CheckDevice()
        {
            return bluetoothHelper.isDevicePaired();
        }

        void OnMessageReceived()
        {
            Debug.Log("OnMessageReceived");
            string recv = bluetoothHelper.Read();
            Debug.Log(recv);
            _actionReceivedMessage?.Invoke(recv);
        }

        void OnConnected()
        {
            try
            {
                Debug.Log("OnConnected");
                _connect = true;
                bluetoothHelper.StartListening();
                _actionConnect?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        void OnConnectionFailed()
        {
            Debug.Log("OnConnectionFailed");
            _actionDisconnect?.Invoke();
        }

        public override void OnCoreMessage(object msg)
        {
        }
    }
}