using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SingleBar : MonoBehaviour
{
    public RectTransform RTF { get => rtf; }

    [SerializeField] TextMeshProUGUI tmp_name;
    [SerializeField] TextMeshProUGUI tmp_score;
    [SerializeField] TextMeshProUGUI tmp_date;
    [SerializeField] RectTransform rtf;

    public void InfoUpdate(string name, string score, string date)
    {
        tmp_name.text = name;
        tmp_score.text = score;
        tmp_date.text = date;
    }
}
