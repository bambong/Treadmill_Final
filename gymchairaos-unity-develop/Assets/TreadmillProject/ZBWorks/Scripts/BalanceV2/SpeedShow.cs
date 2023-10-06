using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedShow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;

    private void Update()
    {
        tmp.text = ((int)Managers.Token.CurSpeedMeterPerSec).ToString();
    }
}
