using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gymchair.Contents.Popup
{
    public class WarnningSuccessPopup : MonoBehaviour
    {
        event Action success;
        [SerializeField] ScrollRect _scrollRect;
        [SerializeField] Button _buttonOK;
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
        private void Awake()
        {
            _buttonOK.interactable = false;

            StartCoroutine(OnCheckCorutine());
        }

        IEnumerator OnCheckCorutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (_scrollRect.normalizedPosition.y <= 0.0f)
                {
                    if (!_buttonOK.interactable)
                    {
                        _buttonOK.interactable = true;
                        yield break;
                    }
                }
            }
        }

        public void hide()
        {
            Managers.Sound.PlayTouchEffect();
            success?.Invoke();
            Destroy(this.gameObject);
        }
    }
}