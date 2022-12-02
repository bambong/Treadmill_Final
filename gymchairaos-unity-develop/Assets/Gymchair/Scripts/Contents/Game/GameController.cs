using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Gymchair.Core.Mgr;
using Gymchair.Contents.Popup;
using System.Net;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Gymchair.Contents.Game
{
    public class GameController : MonoBehaviour
    {
        [Header("게임 화면")]
        [SerializeField] GameObject _gameCanvas;

        [Header("배경")]
        [SerializeField] Image _imageBackground;
        [SerializeField] Sprite _spriteBackground1;
        [SerializeField] Sprite _spriteBackground2;
        [SerializeField] Sprite _spriteBackground3;

        [Header("시간 게이지")]
        [SerializeField] GameObject _objTimeIcon;
        [SerializeField] Slider _sliderTimeGage;
        [SerializeField] TMP_Text _textTimeGage;
        [SerializeField] GameObject _objTimeGageBackground;

        [Header("속도 계기판")]
        [SerializeField] Image _imageGage1;
        [SerializeField] Image _imageGage2;
        [SerializeField] Image _imageGage3;
        [SerializeField] Image _imageGage4;
        [SerializeField] Image _imageGage5;

        [SerializeField] Sprite _spriteNormalGage1;
        [SerializeField] Sprite _spriteNormalGage2;
        [SerializeField] Sprite _spriteNormalGage3;
        [SerializeField] Sprite _spriteNormalGage4;
        [SerializeField] Sprite _spriteNormalGage5;

        [SerializeField] Sprite _spriteActiveGage1;
        [SerializeField] Sprite _spriteActiveGage2;
        [SerializeField] Sprite _spriteActiveGage3;
        [SerializeField] Sprite _spriteActiveGage4;
        [SerializeField] Sprite _spriteActiveGage5;

        [SerializeField] Sprite _spriteAccentGage1;
        [SerializeField] Sprite _spriteAccentGage2;
        [SerializeField] Sprite _spriteAccentGage3;
        [SerializeField] Sprite _spriteAccentGage4;
        [SerializeField] Sprite _spriteAccentGage5;

        [SerializeField] TMP_Text _textSpeed;

        [SerializeField] Image _imageLeftGage;
        [SerializeField] Image _imageRightGage;

        [SerializeField] Sprite _spriteLeftRightNormalGage;
        [SerializeField] Sprite _spriteLeftRightActiveGage;
        [SerializeField] Sprite _spriteLeftRightAccentGage;

        [Header("속도 계기판 바늘")]
        [SerializeField] Image _imageGageBar;

        [SerializeField] Image _imageLeftGageBar;
        [SerializeField] Image _imageRightGageBar;

        [Header("팁")]
        [SerializeField] Image _imageTip;

        [SerializeField] Sprite _imageTipNormal;
        [SerializeField] Sprite _imageTipWarnning;
        [SerializeField] Sprite _imageTipDanger;

        [Header("이동 거리")]
        [SerializeField] TMP_Text _textMeter;

        [Header("챠트")]
        [SerializeField] TMP_Text _textChartRPM;
        [SerializeField] TMP_Text _textChartSpeed;
        
        [SerializeField] ChartUtil.Chart _charRPM;
        [SerializeField] ChartUtil.Chart _charSpeed;

        [SerializeField] ChartUtil.Chart _charLeftSpeed;
        [SerializeField] ChartUtil.Chart _charRightSpeed;

        GymchairConnectPopupController _popup;

        float _meter = 0.0f;
        float _time = 0.0f;

        float _maxSpeed = 0.0f;

        bool _testMode;

        bool _isPlay;
        float _deltaTime;
        string _targetAddress = "DC:A6:32:EC:1C:2C";

        List<GymchairData> _listData = new List<GymchairData>();

        string _today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        bool _connect = false;

        float _save_bpm = 0.0f;
        float _save_left_rpm = 0.0f;
        float _save_right_rpm = 0.0f;

        bool _show_warnning = false;
        List<float> _left_values = new List<float>();
        List<float> _right_values = new List<float>();

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
            StartCoroutine(onBlack(true, 0.5f, () =>
            {
                StartCoroutine(connectServer());

                //StartCoroutine(testCorutine());
            }));
        }

        private void Start()
        {
            string[] gyms = DataMgr.Instance.GetGymList();

            if (gyms == null || gyms.Length == 0)
            {
                //this._objTimeIcon.SetActive(true);
                this._sliderTimeGage.gameObject.SetActive(true);
                //this._textTimeGage.gameObject.SetActive(true);
                this._objTimeGageBackground.SetActive(false);
                this._textTimeGage.gameObject.transform.localPosition = new Vector3(-611, 466, 0);
            }
            else
            {
                //this._objTimeIcon.SetActive(false);
                this._sliderTimeGage.gameObject.SetActive(false);
                //this._textTimeGage.gameObject.SetActive(false);
                this._objTimeGageBackground.SetActive(true);
                this._textTimeGage.gameObject.transform.localPosition = new Vector3(-517, 466, 0);
            }

            _gameCanvas.SetActive(false);
            _isPlay = false;
            _deltaTime = 0.0f;
            _listData.Clear();

            StartCoroutine(updateAnimation());

            _popup = GymchairConnectPopupController.Create();
        }

        private void OnEnable()
        {
            if (BluetoothMgr.Instance)
            {
                BluetoothMgr.Instance._actionConnect += OnConnected;
                BluetoothMgr.Instance._actionReceivedMessage += OnReceivedMessage;
                BluetoothMgr.Instance._actionDisconnect += OnDisconnect;
            }
        }
        private void OnDisable()
        {
            if (BluetoothMgr.Instance)
            {
                BluetoothMgr.Instance._actionConnect -= OnConnected;
                BluetoothMgr.Instance._actionReceivedMessage -= OnReceivedMessage;
                BluetoothMgr.Instance._actionDisconnect -= OnDisconnect;
            }

            Debug.Log("OnDisable");
            BluetoothMgr.Instance.Disconnect();
        }

        public void OnConnected()
        {
            if (!this._connect)
            {
                this._connect = true;
                StartCoroutine(updateGymchair());
            }
        }

        public void OnDisconnect()
        {
            if (this._connect)
            {
                this._connect = false;

                this._isPlay = false;

                Information01Popup.Create(() =>
                {
                    SoundMgr.Instance.PlayEffect("touch");
                    _popup = GymchairConnectPopupController.Create();
                    StartCoroutine(ReConnectServer());
                }, () => {
                    SoundMgr.Instance.PlayEffect("touch");
                    Destroy(_popup.gameObject);

                    StartCoroutine(onBlack(false, 0.5f, () =>
                    {
                        SceneMgr.Instance.UnLoadSceneAsync("Game", () =>
                        {
                            SceneMgr.Instance.LoadSceneAsync("Login", LoadSceneMode.Additive, () =>
                            {
                                SoundMgr.Instance.PlayBGM("back");
                            });
                        });
                    }));
                });
            }
        }

        public void OnReceivedMessage(string message)
        {
            if (!this._connect)
            {
                this._connect = true;
                StartCoroutine(updateGymchair());
            }

            try
            {
                int count = message.Length;

                if (message.Contains("bpm_"))
                {
                    string str = message.Substring("bpm_".Length, count - "bpm_".Length);
                    str = str.Replace("\n", "");
                    _save_bpm = float.Parse(str);
                }
                else if (message.Contains("left_rpm_"))
                {
                    string str = message.Substring("left_rpm_".Length, count - "left_rpm_".Length);
                    str = str.Replace("\n", "");
                    _save_left_rpm = float.Parse(str);
                }
                else if (message.Contains("right_rpm_"))
                {
                    string str = message.Substring("right_rpm_".Length, count - "right_rpm_".Length);
                    str = str.Replace("\n", "");
                    _save_right_rpm = float.Parse(str);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void OnExitButton()
        {
            SoundMgr.Instance.PlayEffect("touch");

            if (_gameCanvas.activeSelf)
            {
                _isPlay = false;
                string[] gyms = DataMgr.Instance.GetGymList();

                if (gyms == null || gyms.Length == 0)
                {
                    TestStopPopup.Create(() =>
                    {
                        WarnningSuccessPopup.Create(() =>
                        {
                            SoundMgr.Instance.StopBGM();
                            SoundMgr.Instance.PlayEffect("touch");

                            StartCoroutine(onBlack(false, 0.5f, () =>
                            {
                                SceneMgr.Instance.UnLoadSceneAsync("Game", () =>
                                {
                                    SceneMgr.Instance.LoadSceneAsync("Login", LoadSceneMode.Additive, () =>
                                    {
                                        SoundMgr.Instance.PlayBGM("back");
                                    });
                                });
                            }));
                        });
                    }, () =>
                    {
                        _isPlay = true;
                    });
                }
                else
                {
                    MessagePopup.Create()
                    .SetText("운동을 종료 하시겠습니?")
                    .SetOKAction((script) =>
                    {
                        Destroy(script.gameObject);
                        WarnningSuccessPopup.Create(() =>
                        {
                            SoundMgr.Instance.StopBGM();
                            SoundMgr.Instance.PlayEffect("touch");
                            OnGymEnd();
                        });
                    })
                    .SetCancelAction((script) =>
                    {
                        Destroy(script.gameObject);
                        _isPlay = true;
                    });
                }
            }
        }

        public void OnGymEnd()
        {
            UserGymData user = new UserGymData();
            user.gymDate = _today;
            user.gymchairDatas = _listData.ToArray();

            foreach (var gym in _listData)
            {
                user.gymMeter += gym.meter;
                user.gymTime += gym.time;
                user.gymCalorie += gym.calorie;

                user.speed += gym.speed;
                user.bpm += gym.bpm;
                user.rpm += gym.rpm;

                // 속도 
                if (user.high_speed == 0)
                    user.high_speed = gym.speed;

                if (user.high_speed < gym.speed)
                    user.high_speed = gym.speed;

                // RPM
                if (user.low_rpm == 0)
                    user.low_rpm = gym.rpm;

                if (user.high_rpm == 0)
                    user.high_rpm = gym.rpm;

                if (user.low_rpm > gym.rpm)
                    user.low_rpm = gym.rpm;

                if (user.high_rpm < gym.rpm)
                    user.high_rpm = gym.rpm;

                // BPM(심박수)
                if (user.low_bpm == 0)
                    user.low_bpm = gym.bpm;

                if (user.high_bpm == 0)
                    user.high_bpm = gym.bpm;

                if (user.low_bpm > gym.bpm)
                    user.low_bpm = gym.bpm;

                if (user.high_bpm < gym.bpm)
                    user.high_bpm = gym.bpm;
            }
            user.speed /= _listData.Count;
            user.bpm /= _listData.Count;
            user.rpm /= _listData.Count;

            DataMgr.Instance.AddGymData(user);

            StartCoroutine(onBlack(false, 0.5f, () =>
            {
                SceneMgr.Instance.UnLoadSceneAsync("Game", () =>
                {
                    SceneMgr.Instance.LoadSceneAsync("Result", LoadSceneMode.Additive);
                });
            }));
        }

        public void OnAttemptConnectToServer()
        {
            Debug.Log("OnAttemptConnectToServer");
            Destroy(_popup.gameObject);
            _popup = null;

            if ( DataMgr.Instance.UserData.tutorial == 0 )
            {
                _testMode = true;

                StartTestPopup.Create(() =>
                {
                    WarnningTestPopup.Create(() =>
                    {
                        SoundMgr.Instance.PlayEffect("touch");
                        OnResetData();
                    });
                });
            }
            else
            {
                _testMode = false;
            }
        }

        public void OnFailConnectToServer()
        {
            Debug.Log("OnFailConnectToServer");
            StartCoroutine(connectServer());
        }

        IEnumerator updateAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.05f);

                if (_isPlay)
                {
                    float posX = UnityEngine.Random.Range(-50.0f, 50.0f);
                    float posY = UnityEngine.Random.Range(-200.0f, 0.0f);
                    _imageBackground.transform.localPosition = new Vector3(posX, posY, 0.0f);
                }
            }
        }

        IEnumerator ReConnectServer()
        {
            BluetoothMgr.Instance.Connect("wheelchair");

            int count = 0;

            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (this._connect)
                    break;

                count++;

                if (count > 10)
                {
                    _popup.gameObject.SetActive(false);

                    Information01Popup.Create(() =>
                    {
                        SoundMgr.Instance.PlayEffect("touch");
                        _popup.gameObject.SetActive(true);
                        StartCoroutine(connectServer());
                    }, () => {
                        SoundMgr.Instance.StopBGM();
                        SoundMgr.Instance.PlayEffect("touch");
                        Destroy(_popup.gameObject);

                        StartCoroutine(onBlack(false, 0.5f, () =>
                        {
                            SceneMgr.Instance.UnLoadSceneAsync("Game", () =>
                                                    {
                                           SceneMgr.Instance.LoadSceneAsync("Login", LoadSceneMode.Additive, () =>
                                {
                                    SoundMgr.Instance.PlayBGM("back");
                                });
                            });
                        }));

                    });
                    yield break;
                }
            }

            _popup.ShowSuccess();
            yield return new WaitForSeconds(1.0f);
            Destroy(_popup.gameObject);

            this._isPlay = true;
        }

        IEnumerator connectServer()
        {
            if (!BluetoothMgr.Instance.isConnect())
                BluetoothMgr.Instance.Connect("wheelchair");

            int count = 0;
            
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (this._connect)
                    break;

                count++;

                if (count > 10)
                {
                    _popup.gameObject.SetActive(false);

                    Information01Popup.Create(() =>
                    {
                        _popup.gameObject.SetActive(true);
                        StartCoroutine(connectServer());
                    }, () => {
                        Destroy(_popup.gameObject);

                        StartCoroutine(onBlack(false, 0.5f, () =>
                        {
                            SceneMgr.Instance.UnLoadSceneAsync("Game", () =>
                            {
                                SceneMgr.Instance.LoadSceneAsync("Login", LoadSceneMode.Additive, () =>
                                {
                                    SoundMgr.Instance.PlayBGM("back");
                                });
                            });
                        }));
                    });
                    yield break;
                }
            }
            
            _popup.ShowSuccess();
            yield return new WaitForSeconds(1.0f);
            Destroy(_popup.gameObject);

            string[] gyms = DataMgr.Instance.GetGymList();

            if (gyms == null || gyms.Length == 0)
            {
                WarnningTestPopup.Create(() =>
                {
                    OnResetData();
                });
            }
            else
            {
                WarnningStartWaitPopup.Create(() =>
                {
                    OnResetData();
                });
            }
        }

        IEnumerator updateGymchair()
        {
            yield return new WaitForSeconds(1.0f);

            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                float rpm = (_save_left_rpm + _save_right_rpm) * 0.5f;
                
               
                float speed = rpm * 240.0f / 60000.0f;
                
                //float speed = 3.14f * 1.89f * rpm;
                //speed /= 60.0f;
                //speed *= 10.0f;

                UpdateRPM(_save_bpm, speed, rpm);
            }
        }

        IEnumerator testCorutine()
        {
            yield return new WaitForSeconds(3.0f);
            _popup.ShowSuccess();
            yield return new WaitForSeconds(1.0f);


            Destroy(_popup.gameObject);

            WarnningStartWaitPopup.Create(() =>
            {
                SoundMgr.Instance.PlayEffect("touch");
                OnResetData(true);
            });
        }

        void OnResetData(bool test = false)
        {
            CountDownController.Create(() =>
            {
                SoundMgr.Instance.PlayBGM("play");
                _gameCanvas.SetActive(true);
                StartCoroutine(OnEndResetData(test));
            });
        }

        IEnumerator OnEndResetData(bool test = false)
        {
            yield return new WaitForEndOfFrame();

            _imageBackground.gameObject.SetActive(false);
            _sliderTimeGage.minValue = 0;
            _sliderTimeGage.maxValue = 60 * 6;
            _sliderTimeGage.value = 0;
            _textTimeGage.text = "00:00";
            _textSpeed.text = "0.0";
            _textMeter.text = "0";

            _textChartRPM.text = "0";
            _textChartSpeed.text = "0";

            _charRPM.chartData.series[0].data.Clear();
            _charRPM.chartData.categories.Clear();

            _charSpeed.chartData.series[0].data.Clear();
            _charSpeed.chartData.categories.Clear();

            _charLeftSpeed.chartData.series[0].data.Clear();
            _charLeftSpeed.chartData.categories.Clear();

            _charRightSpeed.chartData.series[0].data.Clear();
            _charRightSpeed.chartData.categories.Clear();

            List<string> categorys = new List<string>();

            for (int num = 0; num < 100; num++)
            {
                categorys.Add("");
            }

            _charRPM.chartData.categories = categorys;
            _charSpeed.chartData.categories = categorys;
            _charLeftSpeed.chartData.categories = categorys;
            _charRightSpeed.chartData.categories = categorys;

            _charRPM.UpdateChart();
            _charSpeed.UpdateChart();
            _charLeftSpeed.UpdateChart();
            _charRightSpeed.UpdateChart();

            yield return null;
            yield return null;
            yield return null;

            _isPlay = true;

            if (test)
            {
                StartCoroutine(onTestLoop());
            }
        }

        IEnumerator onTestLoop()
        {
            while (true)
            {
                if (_isPlay)
                {
                    _save_left_rpm = UnityEngine.Random.Range(7200, 9600) * 0.1f;
                    _save_right_rpm = UnityEngine.Random.Range(7200, 9600) * 0.1f;

                    float bpm = UnityEngine.Random.Range(80, 120);
                    float rpm = (_save_left_rpm + _save_right_rpm) * 0.5f;
                    float speed = rpm * 240.0f / 60000.0f;

                    try
                    {
                        UpdateRPM(bpm, speed, rpm);
                        //UpdateRPM(0.0f, 0.0f, 0.0f);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Update()
        {
            if (_isPlay)
            {
                _time += Time.deltaTime;
                _sliderTimeGage.value = (int)_time;

                int minute = (int)(_time / 60);
                int second = (int)_time % 60;

                _textTimeGage.text = string.Format("{0:D2}:{1:D2}", minute, second);

                string[] gyms = DataMgr.Instance.GetGymList();

                if ((gyms == null || gyms.Length == 0) && minute == 6)
                {
                    _isPlay = false;
                    
                    WarnningSuccessPopup.Create(() =>
                    {
                        OnGymEnd();
                    });
                }
            }
        }

        void UpdateRPM(float bpm, float speed, float rpm)
        {
            if (!_isPlay)
                return;

            if (_deltaTime == 0.0f)
            {
                _deltaTime = Time.time;
            }
            float deltatime = Time.time - _deltaTime;
            _deltaTime = Time.time;

            // 150 : 0, 145 : 1, 93 : 2, 43 : 3, 0.0 : 4, -45 : 5
            // 데이터 수정 예정
            float meter = speed * deltatime;
            _meter += meter;

            if (_maxSpeed < speed)
                _maxSpeed = speed;

            GymchairData gymchairData = new GymchairData();
            gymchairData.time = deltatime;
            gymchairData.bpm = bpm;
            gymchairData.rpm = rpm;
            gymchairData.speed = speed;
            gymchairData.left_speed = _save_left_rpm;
            gymchairData.right_speed = _save_right_rpm;
            gymchairData.meter = meter;
            gymchairData.calorie = ((DataMgr.Instance.UserData.weight * 0.0175f) / 60.0f) * deltatime;

            _listData.Add(gymchairData);

            float rotateValue = speed;

            if (rotateValue > 4.8f)
                rotateValue = 4.8f;

            float rotateZ = 250 * ((rotateValue >= 4.8f)? 1.0f : rotateValue / 4.8f);
            rotateZ = 150 - rotateZ;

            _imageGageBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotateZ);

            float leftPercent = _save_left_rpm * 240.0f / 60000.0f;
            leftPercent = (leftPercent > 4.8f) ? 1.0f : leftPercent / 4.8f;
            float leftRotateZ = 180 * leftPercent;
            leftRotateZ = 90 - leftRotateZ;

            float rightPercent = _save_right_rpm *  240.0f / 60000.0f;
            rightPercent = (rightPercent > 4.8f) ? 1.0f : rightPercent / 4.8f;
            float rightRotateZ = 180 * rightPercent;
            rightRotateZ = 90 - rightRotateZ;

            _imageLeftGageBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, leftRotateZ);
            _imageRightGageBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rightRotateZ);


            if (leftPercent >= 0.6f)
            {
                _imageLeftGage.sprite = _spriteLeftRightAccentGage;
            }
            else if (leftPercent != 0.0f)
            {
                _imageLeftGage.sprite = _spriteLeftRightActiveGage;
            }
            else
            {
                _imageLeftGage.sprite = _spriteLeftRightNormalGage;
            }

            if (rightPercent >= 0.6f)
            {
                _imageRightGage.sprite = _spriteLeftRightAccentGage;
            }
            else if (rightPercent != 0.0f)
            {
                _imageRightGage.sprite = _spriteLeftRightActiveGage;
            }
            else
            {
                _imageRightGage.sprite = _spriteLeftRightNormalGage;
            }

            Sprite background = null;

            Sprite gage1 = _spriteNormalGage1;
            Sprite gage2 = _spriteNormalGage2;
            Sprite gage3 = _spriteNormalGage3;
            Sprite gage4 = _spriteNormalGage4;
            Sprite gage5 = _spriteNormalGage5;


            if (rotateZ <= 145)
            {
                background = _spriteBackground1;
                gage1 = _spriteActiveGage1;

            }

            if (rotateZ <= 93)
            {
                background = _spriteBackground2;
                gage2 = _spriteActiveGage2;
            }

            if (rotateZ <= 43)
            {
                background = _spriteBackground3;
                gage3 = _spriteActiveGage3;
            }

            if (rotateZ <= 0)
            {
                gage4 = _spriteActiveGage4;
            }

            if (rotateZ <= -45)
            {
                gage5 = _spriteActiveGage5;
            }

            if (background == null)
            {
                _imageBackground.gameObject.SetActive(false);
            }
            else
            {
                _imageBackground.gameObject.SetActive(true);
                _imageBackground.sprite = background;
            }

            _imageGage1.sprite = gage1;
            _imageGage2.sprite = gage2;
            _imageGage3.sprite = gage3;
            _imageGage4.sprite = gage4;
            _imageGage5.sprite = gage5;

            Debug.Log($"{bpm}, {speed}, {_save_left_rpm}, {_save_right_rpm}");

            ChartUtil.Data rpmData = new ChartUtil.Data();
            rpmData.value = (bpm == 0.0f)? 0.00001f:bpm;
            rpmData.show = true;

            ChartUtil.Data speedData = new ChartUtil.Data();
            speedData.value = (speed == 0.0f) ? 0.00001f : speed;
            speedData.show = true;

            ChartUtil.Data leftSpeedData = new ChartUtil.Data();
            leftSpeedData.value = (_save_left_rpm == 0.0f) ? 0.00001f : _save_left_rpm;
            leftSpeedData.show = true;

            ChartUtil.Data rightSpeedData = new ChartUtil.Data();
            rightSpeedData.value = (_save_right_rpm == 0.0f) ? 0.00001f : _save_right_rpm;
            rightSpeedData.show = true;

            _charRPM.chartData.series[0].data.Add(rpmData);
            _charSpeed.chartData.series[0].data.Add(speedData);

            _charLeftSpeed.chartData.series[0].data.Add(leftSpeedData);
            _charRightSpeed.chartData.series[0].data.Add(rightSpeedData);

            if (_charRPM.chartData.series[0].data.Count > 100)
            {
                _charRPM.chartData.series[0].data.RemoveAt(0);
            }

            if (_charSpeed.chartData.series[0].data.Count > 100)
            {
                _charSpeed.chartData.series[0].data.RemoveAt(0);
            }

            if (_charLeftSpeed.chartData.series[0].data.Count > 100)
            {
                _charLeftSpeed.chartData.series[0].data.RemoveAt(0);
            }

            if (_charRightSpeed.chartData.series[0].data.Count > 100)
            {
                _charRightSpeed.chartData.series[0].data.RemoveAt(0);
            }

            _textChartRPM.text = string.Format("{0:0.#}", bpm);
            _textChartSpeed.text = string.Format("{0:0.#}", _maxSpeed);

            _textSpeed.text = string.Format("{0:0.#}", speed);
            _textMeter.text = string.Format("{0:0.#}", _meter);

            _charRPM.UpdateChart();
            _charSpeed.UpdateChart();
            _charLeftSpeed.UpdateChart();
            _charRightSpeed.UpdateChart();


            _left_values.Add(_save_left_rpm);
            _right_values.Add(_save_right_rpm);

            if (_left_values.Count > 150)
                _left_values.RemoveAt(0);

            if (_right_values.Count > 150)
                _right_values.RemoveAt(0);


            if (_left_values.Count >= 150 && _right_values.Count >= 150 && _show_warnning == false)
            {
                float sumLeft = 0.0f;
                float sumRight = 0.0f;

                foreach (var value in _left_values)
                {
                    sumLeft += value;
                }
                foreach (var value in _right_values)
                {
                    sumRight += value;
                }

                sumLeft /= (float)_left_values.Count;
                sumRight /= (float)_right_values.Count;
                
                float per;

                if (sumLeft < sumRight)
                {
                    per = 1.0f - (sumLeft / sumRight);
                }
                else
                {
                    per = 1.0f - (sumRight / sumLeft);
                }

                if (per >= 0.15f)
                {
                    _show_warnning = true;
                    _imageTip.sprite = _imageTipDanger;

                    StartCoroutine(OnEndTip());
                }
                else if (per >= 0.1f)
                {
                    _show_warnning = true;
                    _imageTip.sprite = _imageTipWarnning;
                    
                    StartCoroutine(OnEndTip());
                }
            }
        }
        
        IEnumerator OnEndTip()
        {
            yield return new WaitForSeconds(3.0f);
            _imageTip.sprite = _imageTipNormal;
            yield return new WaitForSeconds(10.0f);
            _show_warnning = false;
        }
    }
}