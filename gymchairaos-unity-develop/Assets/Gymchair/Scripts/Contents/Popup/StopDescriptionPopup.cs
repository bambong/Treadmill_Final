using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gymchair.Contents.Popup
{
    public class StopDescriptionPopup : MonoBehaviour
    {
        event Action<string> _actionButton;

        [SerializeField] InputField _inputDescription;

        public static StopDescriptionPopup Create(Action<string> action)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/StopDescriptionPopup");
            Canvas canvas = Instantiate(obj);
            StopDescriptionPopup script = canvas.GetComponent<StopDescriptionPopup>();
            script._actionButton = action;
            return script;
        }

        public void hide()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            Destroy(this.gameObject);
            _actionButton?.Invoke(_inputDescription.text);
        }
    }
}