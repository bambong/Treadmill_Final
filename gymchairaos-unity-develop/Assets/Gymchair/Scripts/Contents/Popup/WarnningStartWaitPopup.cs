using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gymchair.Contents.Popup
{
    public class WarnningStartWaitPopup : MonoBehaviour
    {
        event Action _actionButton;
        [SerializeField] ScrollRect _scrollRect;
        [SerializeField] Button _buttonOK;

        public static WarnningStartWaitPopup Create(Action action)
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/WarnningStartWaitPopup");
            Canvas canvas = Instantiate(obj);
            WarnningStartWaitPopup script = canvas.GetComponent<WarnningStartWaitPopup>();
            script._actionButton = action;
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
            _actionButton?.Invoke();
            Destroy(this.gameObject);
        }
    }
}