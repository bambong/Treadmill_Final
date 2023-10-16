using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gymchair.Core.Mgr;
using UnityEngine.SceneManagement;

namespace Gymchair.Contents.Result
{
    public class ResultController : MonoBehaviour
    {
        UserGymData _gymData;
        GymchairData[] _gymTickData;

        [Header("운동 시간")]
        [SerializeField] Text _textTime;

        [Header("이동 시간")]
        [SerializeField] Text _textMeter;

        [Header("소모 칼로리")]
        [SerializeField] Text _textkcal;

        [Header("산소 포화도")]
        [SerializeField] Text _textCO2;
        [SerializeField] Image _imgCO2;

        [SerializeField] Sprite[] _spriteCO2s;

        [Header("속도")]
        [SerializeField] Text _textSpeed;
        [SerializeField] Text _textBestSpeed;

        [Header("회전수")]
        [SerializeField] Text _textRpm;
        [SerializeField] Text _textLowRpm;
        [SerializeField] Text _textHighRpm;

        [Header("심박수")]
        [SerializeField] Text _textBpm;
        [SerializeField] Text _textLowBpm;
        [SerializeField] Text _textHighBpm;

        [Header("차트")]
        [SerializeField] ChartUtil.Chart _charSpeed;
        [SerializeField] ChartUtil.Chart _charLeftSpeed;
        [SerializeField] ChartUtil.Chart _charRightSpeed;
        [SerializeField] ChartUtil.Chart _charRPM;
        [SerializeField] ChartUtil.Chart _charBPM;


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

            Managers.Sound.PlayBGM("back");

            _gymData = Managers.Data.GetLastGymData();
            _gymTickData = Managers.Data.GetLastGymTickData();

            int minute = (int)_gymData.gymTime / 60;
            int second = (int)_gymData.gymTime % 60;

            _textTime.text = string.Format("{0:D2}:{1:D2}", minute, second);
            _textMeter.text = $"{(int)_gymData.gymMeter} <size=90>m</size>";
            _textkcal.text = string.Format("{0} <size=75>kcal</size>", (int)_gymData.gymCalorie);
            _textSpeed.text = string.Format("{0:0.#}", _gymData.speed);
            _textBestSpeed.text = string.Format("{0:0.#}", _gymData.high_speed);

            _textBpm.text = string.Format("{0:0.#}", _gymData.bpm);
            _textLowBpm.text = string.Format("{0:0.#}", _gymData.low_bpm);
            _textHighBpm.text = string.Format("{0:0.#}", _gymData.high_bpm);

            float co2 = 5.189f + (2.768f * (_gymData.gymMeter / 1000.0f));
            _textCO2.text = string.Format("{0:0.0}", co2);

            if (co2 >= 30.0f)
                _imgCO2.sprite = _spriteCO2s[0];
            else if (co2 >= 27.9f)
                _imgCO2.sprite = _spriteCO2s[1];
            else if (co2 >= 25.9f)
                _imgCO2.sprite = _spriteCO2s[2];
            else if (co2 >= 23.9f)
                _imgCO2.sprite = _spriteCO2s[3];
            else if (co2 >= 21.9f)
                _imgCO2.sprite = _spriteCO2s[4];
            else
                _imgCO2.sprite = _spriteCO2s[5];

            _charSpeed.chartData.series[0].data.Clear();
            _charSpeed.chartData.categories.Clear();

            _charLeftSpeed.chartData.series[0].data.Clear();
            _charLeftSpeed.chartData.categories.Clear();

            _charRightSpeed.chartData.series[0].data.Clear();
            _charRightSpeed.chartData.categories.Clear();

            _charRPM.chartData.series[0].data.Clear();
            _charRPM.chartData.categories.Clear();

            _charBPM.chartData.series[0].data.Clear();
            _charBPM.chartData.categories.Clear();

            List<string> categorys = new List<string>();

            for (int num = 0; num < _gymTickData.Length; num += 10)
            {
                categorys.Add("");
            }

            _charSpeed.chartData.categories = categorys;
            _charLeftSpeed.chartData.categories = categorys;
            _charRightSpeed.chartData.categories = categorys;
            _charRPM.chartData.categories = categorys;
            _charBPM.chartData.categories = categorys;

            for (int num = 0; num < _gymTickData.Length; num += 10)
            {
                var gym = _gymTickData[num];

                ChartUtil.Data speedData = new ChartUtil.Data();
                speedData.value = gym.speed;
                speedData.show = true;

                ChartUtil.Data leftSpeedData = new ChartUtil.Data();
                leftSpeedData.value = gym.left_speed;
                leftSpeedData.show = true;

                ChartUtil.Data rightSpeedData = new ChartUtil.Data();
                rightSpeedData.value = gym.right_speed;
                rightSpeedData.show = true;


                ChartUtil.Data bpmData = new ChartUtil.Data();
                bpmData.value = gym.bpm;
                bpmData.show = true;

                _charSpeed.chartData.series[0].data.Add(speedData);
                _charLeftSpeed.chartData.series[0].data.Add(leftSpeedData);
                _charRightSpeed.chartData.series[0].data.Add(rightSpeedData);
                _charBPM.chartData.series[0].data.Add(bpmData);
            }

            _charSpeed.UpdateChart();
            _charLeftSpeed.UpdateChart();
            _charRightSpeed.UpdateChart();
            _charRPM.UpdateChart();
            _charBPM.UpdateChart();
        }

        public void OnResultExitButton()
        {
            Managers.Sound.PlayTouchEffect();
            Managers.Scene.LoadScene(E_SceneName.Record);
        }
    }
}