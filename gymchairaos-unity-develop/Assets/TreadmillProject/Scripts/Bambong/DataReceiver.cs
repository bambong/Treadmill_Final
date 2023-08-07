using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataReceiver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    public void OnReceive(string msg) 
    {
        text.text = $"받은 메세지 : {msg}";
    }
 
}
