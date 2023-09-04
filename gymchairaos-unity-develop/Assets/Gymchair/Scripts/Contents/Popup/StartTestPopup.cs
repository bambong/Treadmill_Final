using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class StartTestPopup : MonoBehaviour
    {
        event Action _actionButton;

        public static StartTestPopup Create(Action action)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/StartTestPopup");
            Canvas canvas = Instantiate(obj);
            StartTestPopup script = canvas.GetComponent<StartTestPopup>();
            script._actionButton = action;
            return script;
        }

        public void hide()
        {
            Managers.Sound.PlayTouchEffect();
            _actionButton?.Invoke();
            Destroy(this.gameObject);
        }
    }
}