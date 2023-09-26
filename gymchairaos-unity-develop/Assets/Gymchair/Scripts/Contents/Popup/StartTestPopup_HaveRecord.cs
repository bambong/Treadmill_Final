using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class StartTestPopup_HaveRecord : MonoBehaviour
    {
        event Action _actionButton_left;
        event Action _actionButton_right;

        public static StartTestPopup_HaveRecord Create(Action action_left, Action action_right)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/StartTestPopup");
            Canvas canvas = Instantiate(obj);
            StartTestPopup_HaveRecord script = canvas.GetComponent<StartTestPopup_HaveRecord>();
            script._actionButton_right = action_left;
            script._actionButton_right = action_right;
            return script;
        }

        public void hideLeft()
        {
            Managers.Sound.PlayTouchEffect();
            _actionButton_left?.Invoke();
            Destroy(this.gameObject);
        }
        public void hideRight()
        {
            Managers.Sound.PlayTouchEffect();
            _actionButton_right?.Invoke();
            Destroy(this.gameObject);
        }
    }
}