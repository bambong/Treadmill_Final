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
using Unity.VisualScripting;

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

    
        float curMeter = 0.0f;
        float _time = 0.0f;

        float _maxSpeed = 0.0f;

        bool _isPlay;

        string _description = "";

        List<GymchairData> _listData = new List<GymchairData>();

        string _today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        bool _connect = false;

        bool _show_warnning = false;
        List<float> _left_values = new List<float>();
        List<float> _right_values = new List<float>();

        float _save_to_rotateZ = -999.0f;
        float _save_to_leftRotateZ = -999.0f;
        float _save_to_rightRotateZ = -999.0f;


        private void Awake()
        {
         
        }

        private void Start()
        {
            string[] gyms = Managers.Data.GetGymList();

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
            _listData.Clear();
            OnAttemptConnectToServer();
            StartCoroutine(UpdateAnimation());
            StartCoroutine(UpdateGymchair());

        }

        public void OnConnected()
        {
            if (!this._connect)
            {
                this._connect = true;
                StartCoroutine(UpdateGymchair());
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
                    Managers.Sound.PlayTouchEffect();
                   
                }, () => {
                    Managers.Sound.PlayTouchEffect();
                 
                    Managers.Scene.LoadScene(E_SceneName.Login);
                });
            }
        }

   
     
        public void OnExitButton()
        {
            Managers.Sound.PlayTouchEffect();

            if (_gameCanvas.activeSelf)
            {
                _isPlay = false;
                string[] gyms = Managers.Data.GetGymList();

                if (gyms == null || gyms.Length == 0)
                {
                    TestStopPopup.Create(() =>
                    {
                        StopDescriptionPopup.Create((description) =>
                        {
                            _description = description;

                            WarnningSuccessPopup.Create(() =>
                            {
                                Managers.Sound.StopBGM();
                                Managers.Sound.PlayTouchEffect();
                               OnGymEnd();
                            });
                        });
                    }, () =>
                    {
                        _isPlay = true;
                    });
                }
                else
                {
                    StopPopup.Create(() =>
                    {
                        StopDescriptionPopup.Create((description) =>
                        {
                            _description = description;

                            WarnningSuccessPopup.Create(() =>
                            {
                                Managers.Sound.StopBGM();
                                Managers.Sound.PlayTouchEffect();
                                OnGymEnd();
                            });
                        });
                    }, () =>
                    {
                        _isPlay = true;
                    });
                }
            }
        }

        public void OnGymEnd()
        {
            UserGymData user = new UserGymData();
            user.gymDate = _today;

            foreach (var gym in _listData)
            {
                // user.gymMeter += gym.meter;
                user.gymTime += gym.time;
                // user.gymCalorie += gym.calorie;

                user.speed += gym.speed;
                user.bpm += gym.bpm;

                // 속도 
                if (user.high_speed == 0)
                    user.high_speed = gym.speed;

                if (user.high_speed < gym.speed)
                    user.high_speed = gym.speed;

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

            user.gymMeter = curMeter;
            Debug.Log("1 단계");
            float ml = 5.189f + (2.768f * (curMeter * 0.001f));
            user.gymCalorie = ((ml * Managers.Data.UserData.weight * (user.gymTime / 60.0f)) * 0.001f) * 5.0f;
            Debug.Log("2 단계");
            user.speed /= _listData.Count;
            user.bpm /= _listData.Count;
            user.description = _description;
            user.allow = true;

            Managers.Data.AddGymData(user, _listData.ToArray());
            Debug.Log("3 단계");
            Managers.Scene.LoadScene(E_SceneName.Result);
          
        }

        public void OnAttemptConnectToServer()
        {
            //최초측정 판단
            //bool isFirstAssessment = Managers.Data.GetLastGymData() != null;
            bool isFirstAssessment = true;

            //최초 측정일 경우
            if (isFirstAssessment)
            {
                StartTestPopup.Create(() =>
                {
                    WarnningTestPopup.Create(() =>
                    {
                        Managers.Sound.PlayTouchEffect();
                        OnResetData();
                    });
                });
            }
            //최초 측정 아닐경우
            else
            {
                StartTestPopup_HaveRecord.Create(() =>
                {
                    //왼쪽버튼 클릭이벤트
                },
                () =>
                {
                    //오른쪽버튼 클릭이벤트
                });
            }
        }



        IEnumerator UpdateAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (_isPlay)
                {
                    float posX = UnityEngine.Random.Range(-40.0f, 40.0f);
                    float posY = UnityEngine.Random.Range(-100.0f, 0.0f);
                    _imageBackground.transform.localPosition = new Vector3(posX, posY, 0.0f);
                }
            }
        }

     

    
        IEnumerator UpdateGymchair()
        {
            // yield return new WaitForSeconds(1.0f);
            while (!_isPlay) yield return null;
            while (true)
            {
                yield return null;

                UpdateRPM(Managers.Token.Bpm);
            }
        }

   
        void OnResetData()
        {
            CountDownController.Create(() =>
            {
                Managers.Sound.PlayTouchEffect();
                _gameCanvas.SetActive(true);
                StartCoroutine(OnEndResetData());
            });
        }

        IEnumerator OnEndResetData()
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

        }

    

        private void Update()
        {
            if (_isPlay)
            {
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Managers.Token.Save_right_speed += 10f;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Managers.Token.Save_left_speed += 10f;
                }
                //Managers.Token.Save_left_rpm -= Time.deltaTime;
                //Managers.Token.Save_left_rpm = Mathf.Lerp(Managers.Token.Save_left_rpm, 0, Time.deltaTime);
                //Managers.Token.Save_right_rpm = Mathf.Lerp(Managers.Token.Save_right_rpm, 0, Time.deltaTime);
#endif
                _time += Time.deltaTime;
                _sliderTimeGage.value = (int)_time;

                int minute = (int)(_time / 60);
                int second = (int)_time % 60;

                _textTimeGage.text = string.Format("{0:D2}:{1:D2}", minute, second);

                string[] gyms = Managers.Data.GetGymList();

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

        void UpdateRPM(float bpm)
        {
            if (!_isPlay)
                return;

            float speed = Managers.Token.CurSpeedMeterPerSec;
            curMeter += speed * Time.deltaTime;

            if (_maxSpeed < speed)
                _maxSpeed = speed;

            GymchairData gymchairData = new GymchairData();
            gymchairData.time = Time.deltaTime;
            gymchairData.bpm = bpm;
            gymchairData.speed = speed;
            gymchairData.left_speed = Managers.Token.CurLeftSpeedMPS;
            gymchairData.right_speed = Managers.Token.CurRightSpeedMPS;
            
            
            _listData.Add(gymchairData);


            #region VisulRotate
            float rotateValue = speed;

            if (rotateValue > 4.8f)
                rotateValue = 4.8f;

            float rotateZ = 250 * ((rotateValue >= 4.8f)? 1.0f : rotateValue / 4.8f);
            rotateZ = 150 - rotateZ;
            
            if (_save_to_rotateZ < 0.0f)
                _save_to_rotateZ = rotateZ;

            _save_to_rotateZ = Mathf.Lerp(_save_to_rotateZ, rotateZ, 0.3f);
            _imageGageBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, _save_to_rotateZ);

            float leftPercent = Managers.Token.Save_left_speed;
            leftPercent = (leftPercent > 4.8f) ? 1.0f : leftPercent / 4.8f;
            float leftRotateZ = 180 * leftPercent;
            leftRotateZ = 90 - leftRotateZ;

            if (_save_to_leftRotateZ < 0.0f)
                _save_to_leftRotateZ = leftRotateZ;

            _save_to_leftRotateZ = Mathf.Lerp(_save_to_leftRotateZ, leftRotateZ, 0.3f);

            float rightPercent = Managers.Token.Save_right_speed;
            rightPercent = (rightPercent > 4.8f) ? 1.0f : rightPercent / 4.8f;
            float rightRotateZ = 180 * rightPercent;
            rightRotateZ = 90 - rightRotateZ;

            if (_save_to_rightRotateZ < 0.0f)
                _save_to_rightRotateZ = rightRotateZ;
            
            _save_to_rightRotateZ = Mathf.Lerp(_save_to_rightRotateZ, rightRotateZ, 0.3f);

            _imageLeftGageBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, _save_to_leftRotateZ);
            _imageRightGageBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, _save_to_rightRotateZ);


            if (_save_to_leftRotateZ >= 0.6f)
            {
                _imageLeftGage.sprite = _spriteLeftRightAccentGage;
            }
            else if (_save_to_leftRotateZ != 0.0f)
            {
                _imageLeftGage.sprite = _spriteLeftRightActiveGage;
            }
            else
            {
                _imageLeftGage.sprite = _spriteLeftRightNormalGage;
            }

            if (_save_to_rightRotateZ >= 0.6f)
            {
                _imageRightGage.sprite = _spriteLeftRightAccentGage;
            }
            else if (_save_to_rightRotateZ != 0.0f)
            {
                _imageRightGage.sprite = _spriteLeftRightActiveGage;
            }
            else
            {
                _imageRightGage.sprite = _spriteLeftRightNormalGage;
            }

            #endregion VisulRotate

            #region VisualBG
            Sprite background = null;

            Sprite gage1 = _spriteNormalGage1;
            Sprite gage2 = _spriteNormalGage2;
            Sprite gage3 = _spriteNormalGage3;
            Sprite gage4 = _spriteNormalGage4;
            Sprite gage5 = _spriteNormalGage5;


            if (_save_to_rotateZ <= 145)
            {
                background = _spriteBackground1;
                gage1 = _spriteActiveGage1;

            }

            if (_save_to_rotateZ <= 93)
            {
                background = _spriteBackground2;
                gage2 = _spriteActiveGage2;
            }

            if (_save_to_rotateZ <= 43)
            {
                background = _spriteBackground3;
                gage3 = _spriteActiveGage3;
            }

            if (_save_to_rotateZ <= 0)
            {
                gage4 = _spriteActiveGage4;
            }

            if (_save_to_rotateZ <= -45)
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

            #endregion VisualBG


            ChartUtil.Data rpmData = new ChartUtil.Data();
            rpmData.value = (bpm == 0.0f)? 0.00001f:bpm;
            rpmData.show = true;

            ChartUtil.Data speedData = new ChartUtil.Data();
            speedData.value = (speed == 0.0f) ? 0.00001f : speed;
            speedData.show = true;

            ChartUtil.Data leftSpeedData = new ChartUtil.Data();
            leftSpeedData.value = Managers.Token.CurLeftSpeedMPS;
            leftSpeedData.show = true;

            ChartUtil.Data rightSpeedData = new ChartUtil.Data();
            rightSpeedData.value = Managers.Token.CurRightSpeedMPS;
                //(Managers.Token.Save_right_speed <= 0.0f) ? 0.00001f : Managers.Token.Save_right_speed;
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
            _textMeter.text = string.Format("{0:0.#}", curMeter);

            _charRPM.UpdateChart();
            _charSpeed.UpdateChart();
            _charLeftSpeed.UpdateChart();
            _charRightSpeed.UpdateChart();


            _left_values.Add(Managers.Token.CurLeftSpeedMPS);
            _right_values.Add(Managers.Token.CurRightSpeedMPS);

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