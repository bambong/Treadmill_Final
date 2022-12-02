using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace bambong
{
    public class CommonPanelController : PanelController
    {
        [SerializeField]
        private TextMeshProUGUI curTimeText;
        [SerializeField]
        private TextMeshProUGUI curSpeedText;

        public void UpdateTimeText(string text)
        {
            curTimeText.text =  text;
        }
        public void UpdateSpeedText(string text)
        {
            curSpeedText.text = text;
        }
    }
}
