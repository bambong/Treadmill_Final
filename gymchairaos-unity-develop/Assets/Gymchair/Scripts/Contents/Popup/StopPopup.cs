using System;
using Gymchair.Contents.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPopup : MonoBehaviour
{
    event Action _actionOKButton;
    event Action _actionCancelButton;
    public static StopPopup Create(Action okButton = null, Action stopButton = null)
    {
        Canvas obj = Resources.Load<Canvas>("Prefabs/StopPopup");
        Canvas canvas = Instantiate(obj);
        StopPopup script = canvas.GetComponent<StopPopup>();
        script._actionOKButton = okButton;
        script._actionCancelButton = stopButton;
        return script;
    }

    public void OnOkButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        Destroy(this.gameObject);
        _actionOKButton?.Invoke();
    }
    public void OnCancelButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
        Destroy(this.gameObject);
        _actionCancelButton?.Invoke();
    }
}
