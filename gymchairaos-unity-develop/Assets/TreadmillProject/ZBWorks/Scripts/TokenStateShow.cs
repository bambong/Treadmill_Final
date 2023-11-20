using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using bambong;

public class TokenStateShow : MonoBehaviour
{
    public static TokenStateShow instance;

    public string InputText { get => input.text; }

    [SerializeField] TextMeshProUGUI tmp_left;
    [SerializeField] TextMeshProUGUI tmp_right;
    [SerializeField] TextMeshProUGUI tmp_speed;
    [SerializeField] TMP_InputField input;

    void Update()
    {
        tmp_left.text = $"LEFT : {Managers.Token.Save_left_speed.ToString()}";
        tmp_right.text = $"RIGHT : {Managers.Token.Save_right_speed.ToString()}";
        tmp_speed.text = $"CURSPEED : {Managers.Token.CurSpeedMeterPerSec.ToString()}";
    }

    private void Awake()
    {
        instance = this;
    }
}
