using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedShow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    private float value;

    public void SpeedTextUpdate(float value, string text)
    {
        this.value = value;
        this.tmp.text = text;
    }
}
