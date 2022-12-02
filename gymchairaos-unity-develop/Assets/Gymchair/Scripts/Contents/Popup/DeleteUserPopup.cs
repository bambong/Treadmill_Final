using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUserPopup : MonoBehaviour
{
    Action _actionOK;
    Action _actionCancel;

    public static DeleteUserPopup Create(Action actionOK = null, Action actionCancel = null)
    {
        Canvas obj = Resources.Load<Canvas>("Prefabs/DeleteUserPopup");
        Canvas canvas = Instantiate(obj);
        DeleteUserPopup script = canvas.GetComponent<DeleteUserPopup>();
        script._actionOK = actionOK;
        script._actionCancel = actionCancel;
        return script;
    }

    public void OnOkButton()
    {
        _actionOK?.Invoke();
        Destroy(gameObject);
    }

    public void OnCancelButton()
    {
        _actionCancel?.Invoke();
        Destroy(gameObject);
    }
}
