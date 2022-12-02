using Gymchair.Contents.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information01Popup : MonoBehaviour
{
    Action _actionOk;
    Action _actionCancel;
    public static Information01Popup Create(Action _actionOK = null, Action _actionCancel = null)
    {
        GameObject prefabs = Resources.Load<GameObject>("Prefabs/Information01Popup");
        GameObject obj = Instantiate(prefabs);
        Information01Popup script = obj.GetComponent<Information01Popup>();
        script._actionOk = _actionOK;
        script._actionCancel = _actionCancel;
        return script;
    }

    public void OnOkButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

        this._actionOk?.Invoke();
        Destroy(this.gameObject);
    }

    public void OnCancelButton()
    {
        Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

        this._actionCancel?.Invoke();
        Destroy(this.gameObject);
    }
}
