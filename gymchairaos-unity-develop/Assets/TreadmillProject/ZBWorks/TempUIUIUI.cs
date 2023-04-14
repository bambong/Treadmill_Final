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
        TokenInputManager.Instance.ReceivedEvent += temp;
    }

    void Update()
    {
        left.text = TokenInputManager.Instance.Save_left_rpm.ToString();
        right.text = TokenInputManager.Instance.Save_right_rpm.ToString();
        tempt += Time.deltaTime;
    }

    [SerializeField] float tempt;
    public void temp()
    {
        tmpro.text = tempt.ToString();
        tempt = 0;
    }
}
