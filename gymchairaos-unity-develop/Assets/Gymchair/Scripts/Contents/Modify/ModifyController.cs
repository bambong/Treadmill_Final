using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gymchair.Core.Mgr;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Gymchair.Contents.Modify
{
    public class ModifyController : MonoBehaviour
    {
        [SerializeField] Color _colorSelectBackground;
        [SerializeField] Color _colorSelectText;
        [SerializeField] Color _colorSelectOutline;

        [SerializeField] Color _colorNormalBackground;
        [SerializeField] Color _colorNormalText;
        [SerializeField] Color _colorNormalOutline;

        [SerializeField] TMP_InputField _inputNick;
        [SerializeField] TMP_InputField _inputName;

        [SerializeField] GameObject _objMale;
        [SerializeField] GameObject _objFemale;

        [SerializeField] TMP_Dropdown _dropBirthYear;
        [SerializeField] TMP_Dropdown _dropBirthMonth;
        [SerializeField] TMP_Dropdown _dropBirthDay;

        [SerializeField] Text _textCm;
        [SerializeField] Text _textKg;
        [SerializeField] Text _textBmi;

        [SerializeField] Text _textGymName;
        [SerializeField] Text _textIllness;

        string _strGymNames = "";
        string _strIllness = "";

        [SerializeField] GameObject _objScrollObstacle;
        [SerializeField] GameObject _objScrollObstacleContent;
        [SerializeField] GameObject _objObstaclePanelPrefabs;

        [SerializeField] Text _textObstacle;

        [SerializeField] InputField _inputText;

        [SerializeField] Button _btnDelete;

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

                _imgBlckPanel.color = new Color(0, 0, 0, (fadein) ? 1.0f - (t / time) : t / time);
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

        private void Start()
        {
            _btnDelete.enabled = true;

            initBirth();
            loadData();
        }

        void loadData()
        {
            string text = PlayerPrefs.GetString(DataMgr.Instance.UserName);
            UserData.UserData userData = JsonUtility.FromJson<UserData.UserData>(text);

            _inputNick.text = userData.nick;
            _inputName.text = userData.name;

            _dropBirthYear.transform.GetChild(0).GetComponent<TMP_Text>().text = userData.birth_year.ToString();
            _dropBirthMonth.transform.GetChild(0).GetComponent<TMP_Text>().text = userData.birth_month.ToString();
            _dropBirthDay.transform.GetChild(0).GetComponent<TMP_Text>().text = userData.birth_day.ToString();

            _textCm.text = userData.height.ToString();
            _textKg.text = userData.weight.ToString();
            _textBmi.text = userData.bmi.ToString();

            _strGymNames = userData.gym_info;
            _strIllness = userData.disabled_info;

            if (_strGymNames.Length != 0)
            {
                int count = _strGymNames.Split("/").Length;
                _textGymName.text = $"{count} 가지";
            }
            else
            {
                _textGymName.text = "없음";
            }

            if (_strIllness.Length != 0)
                _textIllness.text = _strIllness.Split("/")[0];
            else
            {
                _textIllness.text = "없음";
            }

            if (userData.sex == 0)
            {
                SetSelectButton(_objMale);
                SetNormalButton(_objFemale);
            }
            else
            {
                SetSelectButton(_objFemale);
                SetNormalButton(_objMale);
            }

            if (userData.disease_list.Length == 0)
            {
                _textObstacle.text = "비장애";
                _inputText.text = userData.note;
                _inputText.gameObject.SetActive(true);
                _objScrollObstacle.SetActive(false);

                var children = _objScrollObstacleContent.transform.Cast<Transform>().ToArray();

                foreach (var child in children)
                {
                    Destroy(child.gameObject);
                }

                _btnDelete.enabled = false;
            }
            else
            {
                foreach (var disease in userData.disease_list)
                {
                    GameObject obj = Instantiate(_objObstaclePanelPrefabs, _objScrollObstacleContent.transform);
                    var objPanel = obj.GetComponent<ObstaclePanel>();
                    objPanel.SetInfo(disease);
                    objPanel.SetButtonClick((btScript) =>
                    {
                        btScript.SetCheck(true);

                        int childCount = _objScrollObstacleContent.transform.childCount;

                        for (int iNum = 0; iNum < childCount; iNum++)
                        {
                            var child = _objScrollObstacleContent.transform.GetChild(iNum);
                            var script = child.GetComponent<ObstaclePanel>();

                            if (script != btScript)
                            {
                                script.SetCheck(false);
                            }
                        }
                    });
                }

                _textObstacle.text = "장애";

                _inputText.gameObject.SetActive(false);
                _objScrollObstacle.SetActive(true);
            }

        }

        void initBirth()
        {
            _dropBirthYear.ClearOptions();
            _dropBirthMonth.ClearOptions();
            _dropBirthDay.ClearOptions();

            List<string> listYear = new List<string>();
            List<string> listMonth = new List<string>();
            List<string> listDay = new List<string>();

            for (int year = 2022; year >= 1900; year--)
            {
                listYear.Add($"{year}");
            }

            for (int month = 1; month <= 12; month++)
            {
                listMonth.Add($"{month}");
            }

            for (int day = 1; day <= 31; day++)
            {
                listDay.Add($"{day}");
            }
            _dropBirthYear.AddOptions(listYear);
            _dropBirthMonth.AddOptions(listMonth);
            _dropBirthDay.AddOptions(listDay);
        }

        void SetSelectButton(GameObject obj)
        {
            obj.GetComponent<Image>().color = _colorSelectBackground;
            obj.GetComponent<Outline>().effectColor = _colorSelectOutline;
            obj.transform.GetChild(0).GetComponent<TMP_Text>().color = _colorSelectText;
        }

        void SetNormalButton(GameObject obj)
        {
            obj.GetComponent<Image>().color = _colorNormalBackground;
            obj.GetComponent<Outline>().effectColor = _colorNormalOutline;
            obj.transform.GetChild(0).GetComponent<TMP_Text>().color = _colorNormalText;
        }

        bool CheckSelectButton(GameObject obj)
        {
            if (obj.GetComponent<Image>().color == _colorSelectBackground)
                return true;

            return false;
        }

        public void OnBackButton()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

            StartCoroutine(onBlack(false, 0.5f, () =>
            {
                SceneMgr.Instance.UnLoadSceneAsync("Modify", () =>
                {
                    SceneMgr.Instance.LoadSceneAsync("Record", LoadSceneMode.Additive);
                });
            }));
        }

        public void OnJoinButton()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

            string nick = _inputNick.text;
            string name = _inputName.text;

            float cm = float.Parse(_textCm.text);
            float kg = float.Parse(_textKg.text);
            float bmi = float.Parse(_textBmi.text);

            if (nick.Length == 0)
            {
                Popup.MessagePopup.Create()
                    .SetText("닉네임을 입력하세요.")
                    .ShowOKButton()
                    .HideCancelButton()
                    .SetOKAction((script) => { Destroy(script.gameObject); });
                return;
            }

            if (name.Length == 0)
            {
                Popup.MessagePopup.Create()
                    .SetText("이름을 입력하세요.")
                    .ShowOKButton()
                    .HideCancelButton()
                    .SetOKAction((script) => { Destroy(script.gameObject); });
                return;
            }

            if (cm == 0.0f || kg == 0.0f || bmi == 0.0f)
            {
                Popup.MessagePopup.Create()
                    .SetText("키 / 몸무게를 입력하세요.")
                    .ShowOKButton()
                    .HideCancelButton()
                    .SetOKAction((script) => { Destroy(script.gameObject); });
                return;
            }

            UserData.UserData userData = new UserData.UserData();
            userData.nick = nick;
            userData.name = name;
            userData.sex = CheckSelectButton(_objMale) ? 0 : 1;

            userData.birth_year = int.Parse(_dropBirthYear.transform.GetChild(0).GetComponent<TMP_Text>().text);
            userData.birth_month = int.Parse(_dropBirthMonth.transform.GetChild(0).GetComponent<TMP_Text>().text);
            userData.birth_day = int.Parse(_dropBirthDay.transform.GetChild(0).GetComponent<TMP_Text>().text);

            userData.tutorial = 0;
            userData.play_count = 0;

            userData.height = float.Parse(_textCm.text);
            userData.weight = float.Parse(_textKg.text);
            userData.bmi = float.Parse(_textBmi.text);

            userData.gym_info = _strGymNames;
            userData.disabled_info = _strIllness;

            int childCount = _objScrollObstacleContent.transform.childCount;

            userData.disease_list = new string[childCount];


            for (int iNum = 0; iNum < childCount; iNum++)
            {
                var child = _objScrollObstacleContent.transform.GetChild(iNum);
                var script = child.GetComponent<ObstaclePanel>();
                userData.disease_list[iNum] = script.GetInfo();
            }

            if (childCount == 0)
                userData.note = _inputText.text;

            PlayerPrefs.SetString(name, JsonUtility.ToJson(userData));
            PlayerPrefs.Save();

            StartCoroutine(onBlack(false, 0.5f, () =>
            {
                SceneMgr.Instance.UnLoadSceneAsync("Modify", () =>
                {
                    SceneMgr.Instance.LoadSceneAsync("Record", LoadSceneMode.Additive);
                });
            }));
        }

        public void OnDeleteUser()
        {
            DeleteUserPopup.Create(() =>
            {
                UserData.UserList userList = null;

                if (PlayerPrefs.HasKey("users"))
                {
                    string strText = PlayerPrefs.GetString("users");
                    userList = JsonUtility.FromJson<UserData.UserList>(strText);
                }

                if (userList != null)
                {
                    List<string> users = null;

                    if (userList.users != null)
                    {
                        string nick = _inputNick.text;
                        users = new List<string>(userList.users);
                        users.Remove(nick);

                        userList.users = users.ToArray();
                    }

                    PlayerPrefs.SetString("users", JsonUtility.ToJson(userList));
                    PlayerPrefs.DeleteKey(name);
                    PlayerPrefs.Save();
                }

                StartCoroutine(onBlack(false, 0.5f, () =>
                {
                    SceneMgr.Instance.UnLoadSceneAsync("Modify", () =>
                    {
                        SceneMgr.Instance.LoadSceneAsync("Login", LoadSceneMode.Additive);
                    });
                }));
            }, () =>
            {

            });
        }

        public void OnNickClear()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            _inputNick.text = "";
        }

        public void OnMaleButton()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            SetSelectButton(_objMale);
            SetNormalButton(_objFemale);
        }

        public void OnFemaleButton()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");
            SetSelectButton(_objFemale);
            SetNormalButton(_objMale);
        }

        public void ShowPersonalInformationPopup()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

            float fCm = float.Parse(_textCm.text);
            float fKg = float.Parse(_textKg.text);

            int cm = (int)(fCm * 10.0f);
            int kg = (int)(fKg * 10.0f);

            Popup.PersonalInformationPopup.Create((cm == 0) ? 1600 : cm, (kg == 0) ? 600 : kg, (cm, kg, bmi) =>
            {
                float fCm = (float)cm / 10.0f;
                float fKg = (float)kg / 10.0f;

                _textCm.text = string.Format("{0:0.0}", fCm);
                _textKg.text = string.Format("{0:0.0}", fKg);
                _textBmi.text = bmi.ToString();
            }, () =>
            {
            });
        }

        public void ShowGymInfo()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

            GymInfoPopup.Create(_strGymNames, () =>
            {
                _strGymNames = "";
                _textGymName.text = "없음";
            },
            (gymName) =>
            {
                if (gymName.Length != 0)
                {
                    int count = gymName.Split("/").Length;
                    _strGymNames = gymName;
                    _textGymName.text = $"{count} 가지";
                }
                else
                {
                    _strGymNames = "";
                    _textGymName.text = "없음";
                }
            },
            () =>
            {
            });
        }

        public void ShowIllness()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

            IllnessPopup.Create(_strIllness, () => {
                _textIllness.text = "없음";
                _strIllness = "";
            }, (text) =>
            {
                if (text.Length != 0)
                {
                    _textIllness.text = text.Split("/")[0];
                    _strIllness = text;
                }
                else
                {
                    _textIllness.text = "없음";
                    _strIllness = "";
                }
            }, () =>
            {
            });
        }

        public void ShowObstaclePopup()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

            ObstaclePopup.Create((kind, text) => {
                if (kind == 0)
                {
                    _textObstacle.text = "비장애";
                    _inputText.text = text;
                    _inputText.gameObject.SetActive(true);
                    _objScrollObstacle.SetActive(false);

                    var children = _objScrollObstacleContent.transform.Cast<Transform>().ToArray();

                    foreach (var child in children)
                    {
                        Destroy(child.gameObject);
                    }

                    _btnDelete.enabled = false;
                }
                else if (kind == 1)
                {
                    _textObstacle.text = "장애";

                    _inputText.gameObject.SetActive(false);
                    _objScrollObstacle.SetActive(true);

                    int childCount = _objScrollObstacleContent.transform.childCount;

                    for (int iNum = 0; iNum < childCount; iNum++)
                    {
                        var child = _objScrollObstacleContent.transform.GetChild(iNum);
                        var script = child.GetComponent<ObstaclePanel>();

                        if (script.GetInfo() == text)
                        {
                            return;
                        }
                    }

                    GameObject obj = Instantiate(_objObstaclePanelPrefabs, _objScrollObstacleContent.transform);
                    var objPanel = obj.GetComponent<ObstaclePanel>();
                    objPanel.SetInfo(text);
                    objPanel.SetButtonClick((btScript) =>
                    {
                        btScript.SetCheck(true);

                        int childCount = _objScrollObstacleContent.transform.childCount;

                        for (int iNum = 0; iNum < childCount; iNum++)
                        {
                            var child = _objScrollObstacleContent.transform.GetChild(iNum);
                            var script = child.GetComponent<ObstaclePanel>();

                            if (script != btScript)
                            {
                                script.SetCheck(false);
                            }
                        }
                    });

                    _btnDelete.enabled = true;
                }
            }
            , () => {
            });
        }

        public void OnObstacleDeleteItem()
        {
            Gymchair.Core.Mgr.SoundMgr.Instance.PlayEffect("touch");

            int childCount = _objScrollObstacleContent.transform.childCount;

            for (int iNum = 0; iNum < childCount; iNum++)
            {
                var child = _objScrollObstacleContent.transform.GetChild(iNum);
                var script = child.GetComponent<ObstaclePanel>();

                if (script.GetCheck())
                {
                    Destroy(child.gameObject);
                    return;
                }
            }
        }
    }
}