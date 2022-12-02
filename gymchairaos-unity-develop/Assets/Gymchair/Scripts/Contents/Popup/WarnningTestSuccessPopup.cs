using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class WarnningTestSuccessPopup : MonoBehaviour
    {
        public static WarnningTestSuccessPopup Create()
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/WarnningTestSuccessPopup");
            Canvas canvas = Instantiate(obj);
            WarnningTestSuccessPopup script = canvas.GetComponent<WarnningTestSuccessPopup>();
            return script;
        }

        public void hide()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            Destroy(this.gameObject);
        }
    }
}