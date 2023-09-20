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

    private float factorValue = 0.5f;  // 민감도 테스트를 위한 기준 값
    private string lastMsg = "";

    private bool isConect;
    public string LastMsg { get => lastMsg; }
    public bool IsConect { get => isConect;  }
    public float FactorValue { get => factorValue;  } 

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateValue();
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

#else
        if (isConect && LastMsg != string.Empty)  
        { 
            Managers.Token.OnReceivedMessage(LastMsg); // 토큰 매니저에게 메세지를 전달
            ClearMsg();
        }
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
        lastMsg = msg;
        
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        lastMsg = string.Empty;
    }


}
