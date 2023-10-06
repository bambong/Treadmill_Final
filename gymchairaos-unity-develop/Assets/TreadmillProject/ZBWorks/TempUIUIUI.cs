using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TempUIUIUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI left;
    [SerializeField] TextMeshProUGUI right;
    [SerializeField] TextMeshProUGUI tmpro;

    void Awake()
    {
        Managers.Token.ReceivedEvent += temp;
    }

    void Update()
    {
        left.text = Managers.Token.Save_left_speed.ToString();
        right.text = Managers.Token.Save_right_speed.ToString();
        tempt += Time.deltaTime;
    }

    [SerializeField] float tempt;
    public void temp()
    {
        tmpro.text = tempt.ToString();
        tempt = 0;
    }
}
