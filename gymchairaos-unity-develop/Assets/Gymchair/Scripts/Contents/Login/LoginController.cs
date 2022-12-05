using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gymchair.Core.Mgr;
using Gymchair.Contents.Popup;
using TMPro;
using System;
using UnityEngine.SceneManagement;
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

        private void Awake()
        {
            StartCoroutine(onBlack(true, 0.5f));
        }

        public void showWarnningPopup()
        {
            SoundMgr.Instance.PlayEffect("touch");

            WarnningPopup.Create();
        }

        public void showJoinScene()
        {
            SoundMgr.Instance.PlayEffect("touch");

            StartCoroutine(onBlack(false, 0.5f, () =>
            {
                SceneMgr.Instance.UnLoadSceneAsync("Login", () =>
                {
                    SceneMgr.Instance.LoadSceneAsync("Join", LoadSceneMode.Additive);
                });
            }));
        }

        public void showGameScene()
        {
            SoundMgr.Instance.PlayEffect("touch");

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

             bool login = false;
            if (userList.users != null)
            {

                int count = userList.users.Length;

                for (int num = 0; num < count; num++)
                {
                    if (userList.users[num] == name)
                    {
                        DataMgr.Instance.UserName = name;

                        //SceneManager.LoadScene(bambong.E_SceneName.Speed_GameScene.ToString());

                        StartCoroutine(onBlack(false,0.5f,() =>
                        {
                          SoundMgr.Instance.StopBGM();

                          SceneMgr.Instance.UnLoadSceneAsync("Login",() =>
                           {
                              SceneMgr.Instance.LoadSceneAsync(bambong.E_SceneName.SelectGame.ToString(),LoadSceneMode.Additive);
                              //SceneMgr.Instance.LoadSceneAsync("Game",LoadSceneMode.Additive);
                          });
                        }));

                        login = true;
                        return;
                    }
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

                        StartCoroutine(onBlack(false, 0.5f, () =>
                        {
                            SceneMgr.Instance.UnLoadSceneAsync("Login", () =>
                            {
                                SceneMgr.Instance.LoadSceneAsync("Join", LoadSceneMode.Additive);
                            });
                        }));
                    })
                    .SetCancelAction((popup) =>
                    {
                        Destroy(popup.gameObject);
                    });
            }
        }
    }
}