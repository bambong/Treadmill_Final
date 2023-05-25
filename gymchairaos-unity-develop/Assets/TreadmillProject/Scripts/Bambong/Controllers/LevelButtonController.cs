using Gymchair.Core.Mgr;
using System;
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
                button.onClick.AddListener(()
                    =>
                {
                    SoundMgr.Instance.PlayEffect("touch");
                    action?.Invoke(level); 
                });
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
