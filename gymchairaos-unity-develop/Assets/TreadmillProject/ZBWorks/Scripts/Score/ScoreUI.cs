using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ZB;


namespace ZB
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] Score score;
        [SerializeField] TextMeshProUGUI tmp;

        public void OnScoreChanged()
        {
            tmp.text = score.NowScore.ToString();
        }
    }
}
