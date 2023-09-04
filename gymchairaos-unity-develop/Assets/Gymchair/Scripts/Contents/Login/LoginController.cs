using System.Collections;
using UnityEngine;
using Gymchair.Core.Mgr;
using Gymchair.Contents.Popup;
using TMPro;
using System;
using UnityEngine.UI;

namespace Gymchair.Contents.Login
{
    public class LoginController : MonoBehaviour
    {
        [SerializeField] TMP_InputField _inputNickName;

        [SerializeField] GameObject _objBlckPanel;
        [SerializeField] Image _imgBlckPanel;

        IEnumerator onBlack(bool fadein, float time, Action success = null)
        {
            _imgBlckPanel.color = new Color(0, 0, 0, (fadein) ? 1.0f : 0.0f);
            _objBlckPanel.SetActive(true);

            float t = 0.0f;

            while (true)
            {
                t += Time.deltaTime;

                if (t >= time)
                    t = time;

                _imgBlckPanel.color = new Color(0, 0, 0, (fadein)?1.0f - (t / time): t / time);
                yield return new WaitForEndOfFrame();

                if (t == time)
                    break;
            }

            if (fadein)
                _objBlckPanel.SetActive(false);

            success?.Invoke();
        }

        private void Start()
        {
            Managers.Sound.PlayBGM("back");
            StartCoroutine(onBlack(true, 1f, () =>
            {
                WarnningPopup.Create();
            }));
        }


        public void showWarnningPopup()
        {
            Managers.Sound.PlayEffect("touch");
            WarnningPopup.Create();
        }

        public void showJoinScene()
        {
            Managers.Sound.PlayEffect("touch");
            Managers.Scene.LoadScene(E_SceneName.Join); ;
        }

        public void showGameScene()
        {
            Managers.Sound.PlayEffect("touch");
            string name = _inputNickName.text;

            UserData.UserList userList = null;

            if (PlayerPrefs.HasKey("users"))
            {
                string strText = PlayerPrefs.GetString("users");
                userList = JsonUtility.FromJson<UserData.UserList>(strText);
            }

            if (userList == null)
            {
                userList = new UserData.UserList();
            }

            if (userList.users != null)
            {
                bool login = false;

                int count = userList.users.Length;

                for (int num = 0; num < count; num++)
                {
                    if (userList.users[num] == name)
                    {
                        Managers.Data.UserName = name;

                        Managers.Scene.LoadScene(E_SceneName.SelectMenu);

                        login = true;
                        return;
                    }
                }

                if (!login)
                {
                    MessagePopup.Create()
                        .SetText("등록되지 않은 회원입니다.\n새로운 사용자를 등록하시겠습니까?")
                        .ShowCancelButton()
                        .SetOKAction((popup) =>
                        {
                            Destroy(popup.gameObject);
                            Managers.Scene.LoadScene(E_SceneName.Join);
                        })
                        .SetCancelAction((popup) =>
                        {
                            Destroy(popup.gameObject);
                        });
                }
            }
            else
            {
                MessagePopup.Create()
                .SetText("등록되지 않은 회원입니다.\n새로운 사용자를 등록하시겠습니까?")
                .ShowCancelButton()
                .SetOKAction((popup) =>
                {
                    Destroy(popup.gameObject);
                    Managers.Scene.LoadScene(E_SceneName.Join);
                })
                .SetCancelAction((popup) =>
                {
                    Destroy(popup.gameObject);
                });
            }
        }
    }
}
