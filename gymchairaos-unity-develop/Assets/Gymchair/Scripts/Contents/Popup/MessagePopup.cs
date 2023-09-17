using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gymchair.Contents.Popup
{
    public class MessagePopup : MonoBehaviour
    {
        [SerializeField] Text _textMessage;
        [SerializeField] GameObject _buttonOK;
        [SerializeField] GameObject _buttonCancel;

        event Action<MessagePopup> _actionOK;
        event Action<MessagePopup> _actionCancel;

        public static MessagePopup Create()
        {
            GameObject prefabs = Resources.Load<GameObject>("Prefabs/MessagePopup");
            GameObject obj = Instantiate(prefabs);
            MessagePopup script = obj.GetComponent<MessagePopup>();
            return script;
        }

        public MessagePopup SetText(string text)
        {
            _textMessage.text = text;
            return this;
        }

        public MessagePopup ShowOKButton()
        {
            _buttonOK.SetActive(true);

            if (_buttonCancel.activeSelf)
            {
                // +- 128
                Vector3 pos = _buttonOK.transform.localPosition;
                pos.x = -128.0f;
                _buttonOK.transform.localPosition = pos;

                pos = _buttonCancel.transform.localPosition;
                pos.x = 128.0f;
                _buttonCancel.transform.localPosition = pos;
            }
            else
            {
                Vector3 pos = _buttonOK.transform.localPosition;
                pos.x = 0;
                _buttonOK.transform.localPosition = pos;
            }

            return this;
        }

        public MessagePopup HideOKButton()
        {
            _buttonOK.SetActive(false);

            if (_buttonCancel.activeSelf)
            {
                // +- 128
                Vector3 pos = _buttonCancel.transform.localPosition;
                pos.x = 0;
                _buttonCancel.transform.localPosition = pos;
            }

            return this;
        }

        public MessagePopup ShowCancelButton()
        {
            _buttonCancel.SetActive(true);

            if (_buttonOK.activeSelf)
            {
                // +- 128
                Vector3 pos = _buttonOK.transform.localPosition;
                pos.x = -128.0f;
                _buttonOK.transform.localPosition = pos;

                pos = _buttonCancel.transform.localPosition;
                pos.x = 128.0f;
                _buttonCancel.transform.localPosition = pos;
            }
            else
            {
                Vector3 pos = _buttonCancel.transform.localPosition;
                pos.x = 0;
                _buttonCancel.transform.localPosition = pos;
            }

            return this;
        }

        public MessagePopup HideCancelButton()
        {
            _buttonCancel.SetActive(false);

            if (_buttonOK.activeSelf)
            {
                // +- 128
                Vector3 pos = _buttonOK.transform.localPosition;
                pos.x = 0;
                _buttonOK.transform.localPosition = pos;
            }

            return this;
        }

        public MessagePopup SetOKAction(Action<MessagePopup> action)
        {
            _actionOK = action;
            return this;
        }

        public MessagePopup SetCancelAction(Action<MessagePopup> action)
        {
            _actionCancel = action;
            return this;
        }

        public void OnOkButton()
        {
            Managers.Sound.PlayTouchEffect();
            _actionOK?.Invoke(this);
        }

        public void OnCancelButton()
        {
            Managers.Sound.PlayTouchEffect();
            _actionCancel?.Invoke(this);
        }
    }
}