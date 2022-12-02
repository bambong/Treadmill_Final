using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class WarnningPopup : MonoBehaviour
    {
        public static WarnningPopup Create()
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/WarnningPopup");
            Canvas canvas = Instantiate(obj);
            WarnningPopup script = canvas.GetComponent<WarnningPopup>();
            return script;
        }

        public void hide()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            Destroy(this.gameObject);
        }
    }
}