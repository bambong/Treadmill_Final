using Gymchair.Core.Mgr;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Gymchair.Contents.Record
{
    public class RecordController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI tmp_cheer;

        [Header("기본 정보")]
        [SerializeField] Text _textNickName;
        [SerializeField] Text _textAge;
        [SerializeField] Text _textHeight;
        [SerializeField] Text _textWeight;
        [SerializeField] Image _imageSex;

        [SerializeField] Sprite _spriteMale;
        [SerializeField] Sprite _spriteFemale;

        [Header("산소 포화도")]
        [SerializeField] Text _textCO2;
        [SerializeField] Image _imgCO2;

        [SerializeField] Sprite[] _spriteCO2s;

        [Header("운동 정보")]
        [SerializeField] TextMeshProUGUI _textGymCount;
        [SerializeField] Text _textGymTime;
        [SerializeField] Text _textGymMonthTime;
        [SerializeField] Text _textGymKcal;
        [SerializeField] Text _textGymMeter;
        [SerializeField] Text _textGymSpeed;

        [Header("최근 운동 정보")]
        [SerializeField] GameObject _objContent;
        [SerializeField] GameObject _objRecordPrefab;

        int _page = 0;
        int _count = 10;

        int _maxCount = 0;

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

            DateTime dateMonth = DateTime.Now;

            int year = dateMonth.Year;
            int month = dateMonth.Month;

            int nextYear = year;
            int nextMonth = month + 1;

            if (nextMonth > 12)
            {
                nextYear += 1;
                nextMonth = 1;
            }

            DateTime startMonth = DateTime.ParseExact(string.Format("{0:D4}-{1:D2}-01", year, month), "yyyy-MM-dd", null);
            DateTime endMonth = DateTime.ParseExact(string.Format("{0:D4}-{1:D2}-01", nextYear, nextMonth), "yyyy-MM-dd", null);
            endMonth = endMonth.AddDays(-1);

            DateTime weekStart = dateMonth.AddDays(-((int)dateMonth.DayOfWeek));
            weekStart = new DateTime(weekStart.Year, weekStart.Month, weekStart.Day, 0, 0, 0);

            UserData.UserData userData = null;

            string text = PlayerPrefs.GetString(Managers.Data.UserName);
            userData = JsonUtility.FromJson<UserData.UserData>(text);

            string birthDay = string.Format("{0:D4}-{1:D2}-{2:D2}", userData.birth_year, userData.birth_month, userData.birth_day);
            var ageDate = DateTime.Now - DateTime.ParseExact(birthDay, "yyyy-MM-dd", null);
            var iAge = (int)ageDate.TotalDays / 365;

            string[] gyms = Managers.Data.GetGymList();
            _maxCount = gyms.Length;

            float gymWeekTime = 0.0f;
            float gymMonthTime = 0.0f;

            float gymTime = 0.0f;
            float gymKcal = 0.0f;
            float gymMeter = 0.0f;
            float gymHighSpeed = 0.0f;
            float gymCo2 = 0.0f;

            for (int num = _maxCount - 1; num >= 0; num--)
            {
                string date = gyms[num];
                UserGymData userGymData = Managers.Data.GetGymData(num);
                var gymDateArray = userGymData.gymDate.Split(" ");

                DateTime gymDateTime = DateTime.ParseExact(gymDateArray[0], "yyyy-MM-dd", null);

                if (weekStart <= gymDateTime && dateMonth >= gymDateTime)
                    gymWeekTime += userGymData.gymTime;
                if (startMonth <= gymDateTime && endMonth >= gymDateTime)
                    gymMonthTime += userGymData.gymTime;

                gymTime += userGymData.gymTime;
                gymKcal += userGymData.gymCalorie;
                gymMeter += userGymData.gymMeter;

                float co2 = 5.189f + (2.768f * (userGymData.gymMeter / 1000.0f));

                if (co2 > gymCo2)
                    gymCo2 = co2;

                if (userGymData.high_speed > gymHighSpeed)
                    gymHighSpeed = userGymData.high_speed;

                Transform tf;
                GameObject obj = Instantiate(_objRecordPrefab);
                obj.TryGetComponent(out tf);
                tf.position = new Vector3(tf.position.x, tf.position.y, 161.062f);
                obj.transform.parent = _objContent.transform;
                obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                int sec = (int)userGymData.gymTime % 60;
                int min = (int)userGymData.gymTime / 60;

                var gymDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", null);

                RecordPrefabController script = obj.GetComponent<RecordPrefabController>();
                script._textTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
                script._textMeter.text = $"{(int)userGymData.gymMeter} m";
                script._textHighSpeed.text = string.Format("{0:0.#} m/s", userGymData.high_speed);
                script._textBPM.text = $"{(int)userGymData.bpm} BPM";
                script._textkcal.text = $"{(int)userGymData.gymCalorie} kcal";
                script._textDate.text = gymDate.ToString("yyyy년 MM월 dd일 tt hh시 mm분");
                script._keyNumber = num;
                script.SetBG_Green(num % 2 == 0);
                script._actionClick = (keyNumber) =>
                {
                    Managers.Data.SetKeyNumber(keyNumber);
                    Managers.Scene.LoadScene(E_SceneName.TargetResult);
                };
            }

            int second = (int)gymWeekTime % 60;
            int minute = ((int)gymWeekTime / 60) % 60;
            int hour = ((int)gymWeekTime / 60) / 60;
            _textGymTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);

            second = (int)gymMonthTime % 60;
            minute = ((int)gymMonthTime / 60) % 60;
            hour = ((int)gymMonthTime / 60) / 60;
            _textGymMonthTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);

            _textGymCount.text = $"{_maxCount.ToString()}<color=black>회!</color>";
            _textGymKcal.text = $"{(int)gymKcal}";
            _textGymMeter.text = $"{(int)gymMeter}";
            _textGymSpeed.text = $"{(int)gymHighSpeed}";

            _textNickName.text = Managers.Data.UserName;
            tmp_cheer.text = $"{Managers.Data.UserName} 님 오늘도 힘내볼까요!";
            _textAge.text = $"{iAge}";
            _textHeight.text = $"{userData.height}";
            _textWeight.text = $"{userData.weight}";

            if (userData.sex == 0)
            {
                _imageSex.sprite = _spriteMale;
            }
            else
            {
                _imageSex.sprite = _spriteFemale;
            }

            _textCO2.text = string.Format("{0:0.0}", gymCo2);

            if (gymCo2 >= 30.0f)
                _imgCO2.sprite = _spriteCO2s[0];
            else if (gymCo2 >= 27.9f)
                _imgCO2.sprite = _spriteCO2s[1];
            else if (gymCo2 >= 25.9f)
                _imgCO2.sprite = _spriteCO2s[2];
            else if (gymCo2 >= 23.9f)
                _imgCO2.sprite = _spriteCO2s[3];
            else if (gymCo2 >= 21.9f)
                _imgCO2.sprite = _spriteCO2s[4];
            else
                _imgCO2.sprite = _spriteCO2s[5];
        }

        public void OnModifyUserName()
        {
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.Modify);

            //Popup.MessagePopup.Create()
            //    .SetText("수정 기능을 아직 제공하지 않습니다.")
            //    .HideCancelButton()
            //    .ShowOKButton()
            //    .SetOKAction((script) =>
            //    {
            //        Destroy(script.gameObject);
            //    });
        }

        public void OnGameSceneButton()
        {
            Managers.Sound.StopBGM();
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.Assessment);
        }

        public void OnMainSceneButton()
        {
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.Login);
        }
    }
}