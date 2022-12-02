using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class WarnningTestPopup : MonoBehaviour
    {
        event Action _actionButton;

        public static WarnningTestPopup Create(Action action)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/WarnningTestPopup");
            Canvas canvas = Instantiate(obj);
            WarnningTestPopup script = canvas.GetComponent<WarnningTestPopup>();
            script._actionButton = action;
            return script;
        }

        public void hide()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            Destroy(this.gameObject);
            _actionButton?.Invoke();
        }
    }
}