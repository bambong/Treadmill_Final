using System;
using Gymchair.Contents.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStopPopup : MonoBehaviour
{
    event Action _actionOKButton;
    event Action _actionCancelButton;
    public static TestStopPopup Create(Action okButton = null, Action stopButton = null)
    {
        Canvas obj = Resources.Load<Canvas>("Prefabs/TestStopPopup");
        Canvas canvas = Instantiate(obj);
        TestStopPopup script = canvas.GetComponent<TestStopPopup>();
        script._actionOKButton = okButton;
        script._actionCancelButton = stopButton;
        return script;
    }

    public void OnOkButton()
    {
        Managers.Sound.PlayTouchEffect();
        Destroy(this.gameObject);
        _actionOKButton?.Invoke();
    }
    public void OnCancelButton()
    {
        Managers.Sound.PlayTouchEffect();
        Destroy(this.gameObject);
        _actionCancelButton?.Invoke();
    }
}
