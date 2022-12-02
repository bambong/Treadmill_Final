using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class WarnningStartWaitPopup : MonoBehaviour
    {
        event Action _actionButton;

        public static WarnningStartWaitPopup Create(Action action)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/WarnningStartWaitPopup");
            Canvas canvas = Instantiate(obj);
            WarnningStartWaitPopup script = canvas.GetComponent<WarnningStartWaitPopup>();
            script._actionButton = action;
            return script;
        }

        public void hide()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            _actionButton?.Invoke();
            Destroy(this.gameObject);
        }
    }
}