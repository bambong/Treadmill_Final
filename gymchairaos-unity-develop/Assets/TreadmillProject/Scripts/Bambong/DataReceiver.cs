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
        text.text = $"���� �޼��� : {msg}";
    }
 
}
