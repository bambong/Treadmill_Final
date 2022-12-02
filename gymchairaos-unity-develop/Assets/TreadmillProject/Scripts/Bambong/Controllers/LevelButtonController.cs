using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace bambong 
{
    public class LevelButtonController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label;
        [SerializeField]
        private Button button;
        [SerializeField]
        private Color disableColor;

        [SerializeField]
        private Image buttonImage;

        public void SetButton(Action<int> action,int level ,string text)
        {
            label.text = text;
            if(GameManager.Instance.IsOpenLevel(level))
            { 
                button.onClick.AddListener(()=> action?.Invoke(level));
            }
            else 
            {
                Disable();
            }
        }
        private void Disable()
        {
            buttonImage.color = disableColor;
        }
    }

}
