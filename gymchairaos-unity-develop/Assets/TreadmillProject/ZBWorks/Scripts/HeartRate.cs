using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using bambong;
using TMPro;

public class HeartRate : MonoBehaviour
{
    public float Average { get => sum / time; }

    [SerializeField] private TextMeshProUGUI tmpro;
    [SerializeField] private float sum;
    [SerializeField] private float time;
    bool checking;

    void Update()
    {
        tmpro.text = Managers.Token.Bpm.ToString();
        if (checking)
        {
            sum += Managers.Token.Bpm;
            time += Time.deltaTime;
            Debug.LogError($"{sum}, {time}, {Average}");
        }
    }

    public void AverageCheckStart()
    {
        sum = 0;
        time = 0;
        checking = true;
    }
    public void AverageCheckStop()
    {
        checking = false;
    }
}