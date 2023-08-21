using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataReceiver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI factor;

    private float factorValue = 0.5f;
    private string lastMsg = "";

    private static DataReceiver instance;
    public static DataReceiver Instance { get { return instance; } }

    private bool isConect;
    public string LastMsg { get => lastMsg; }
    public bool IsConect { get => isConect;  }
    public float FactorValue { get => factorValue;  }

    private void Awake()
    {
        if(instance != null && instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateValue();

    }

    public void AddValue(float value) 
    {
        factorValue += value;
        UpdateValue();
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
        lastMsg = msg;
        
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        lastMsg = string.Empty;
        isConect = false;
    }


}
