using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gymchair.Contents.Popup
{
    public class WarnningPopup : MonoBehaviour
    {
        [SerializeField] ScrollRect _scrollRect;
        [SerializeField] Button _buttonOK;

        public static WarnningPopup Create()
        {
            Canvas obj = Resources.Load<Canvas>("Prefabs/WarnningPopup");
            Canvas canvas = Instantiate(obj);
            WarnningPopup script = canvas.GetComponent<WarnningPopup>();
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
            Destroy(this.gameObject);
        }
    }
}