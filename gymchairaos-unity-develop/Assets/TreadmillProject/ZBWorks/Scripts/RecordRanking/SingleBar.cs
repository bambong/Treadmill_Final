using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SingleBar : MonoBehaviour
{
    public RectTransform RTF { get => rtf; }

    [SerializeField] TextMeshProUGUI[] tmps;
    [SerializeField] RectTransform rtf;

    public void InfoUpdate(string[] strings)
    {
        for (int i = 0; i < tmps.Length; i++)
        {
            tmps[i].text = strings[i];
        }
    }
}
