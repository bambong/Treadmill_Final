using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using bambong;
using TMPro;

public class HeartRate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpro;
    void Update()
    {
        tmpro.text = Managers.Token.Bpm.ToString();
    }
}