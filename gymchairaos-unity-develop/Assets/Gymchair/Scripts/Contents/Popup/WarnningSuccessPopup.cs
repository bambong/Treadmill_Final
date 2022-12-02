using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class WarnningSuccessPopup : MonoBehaviour
    {
        event Action success;

        public static WarnningSuccessPopup Create(Action success = null)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/WarnningSuccessPopup");
            Canvas canvas = Instantiate(obj);
            WarnningSuccessPopup script = canvas.GetComponent<WarnningSuccessPopup>();

            if (success != null)
            {
                script.success = success;
            }
            return script;
        }

        public void hide()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            success?.Invoke();
            Destroy(this.gameObject);
        }
    }
}