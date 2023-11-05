using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ZB.Balance2
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] Transform resetPos;

        [SerializeField] UIControll uiControll;

        [SerializeField] RotateObj rotateObj;
        [SerializeField] RotateObjInput input;
        [SerializeField] BallControl ballControl;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] HeartRate heartRate;

        [SerializeField] TextMeshProUGUI tmp_Ready;
        [SerializeField] Image img_Shadow;
        [SerializeField] Color color_littleShow;

        [SerializeField] RankingDataHolder rankingDataHolder;
        [SerializeField] int stage;

        [SerializeField] ParticleSystem par_ShowPos;

        bool cleared;

        public void GameStart()
        {
            StartCoroutine(StartCycle());
        }

        public void GameClear()
        {
            if (!cleared)
            {
                cleared = true;
                heartRate.AverageCheckStop();
                uiControll.SetTextInfo(stage.ToString(), timeCounter.NowTime, ((int)heartRate.Average).ToString());
                timeCounter.CountPause();

                //클리어판정
                rankingDataHolder.rankingData.ranking_Balance.Add(RankingData.GetUserName(), RankingData.GetDate(), timeCounter.NowTime, stage);
                rankingDataHolder.Write();
                Managers.Sound.PlayEffect("sfx_controlSuccuess");

                StartCoroutine(delayNActiveResult());
            }
        }

        public void GameOver()
        {
            if (!cleared)
            {
                rotateObj.ResetState();
                input.ResetState();
                input.Active(false);
                ballControl.Active(false);
                timeCounter.CountPause();
                Managers.Sound.PlayEffect("sfx_controlFail");
            }
        }

        public void Pause(bool active)
        {
            Time.timeScale = active ? 0 : 1;
            if (active) timeCounter.CountPause();
            else timeCounter.CountStart();
        }

        private void Start()
        {
            img_Shadow.color = Color.black;

            GameStart();
        }

        IEnumerator StartCycle()
        {
            yield return null;
            Managers.Sound.PlayBGM("bgm_Control");

            input.Active(false);
            img_Shadow.gameObject.SetActive(true);
            Time.timeScale = 1;
            timeCounter.CountStop();
            img_Shadow.DOColor(Color.black, 0.5f);
            yield return new WaitForSeconds(0.5f);

            input.ResetState();
            rotateObj.ResetState();
            img_Shadow.DOColor(color_littleShow, 0.75f);
            ballControl.Active(false);
            ballControl.SetPosition(resetPos.position);
            yield return new WaitForSeconds(1);

            tmp_Ready.gameObject.SetActive(true);
            tmp_Ready.text = "READY";
            tmp_Ready.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                tmp_Ready.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuart);
            });
            yield return new WaitForSeconds(1.1f);
            tmp_Ready.text = "START";
            tmp_Ready.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                tmp_Ready.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuart);
            });
            yield return new WaitForSeconds(1.5f);
            img_Shadow.DOColor(Color.clear, 0.5f).OnComplete(()=>
            {
                img_Shadow.gameObject.SetActive(false);
            });
            timeCounter.CountStart();
            heartRate.AverageCheckStart();

            ballControl.Active(true);
            input.Active(true);
            par_ShowPos.Stop();
        }

        WaitForSeconds delayNActiveResult_WFS = new WaitForSeconds(2);
        IEnumerator delayNActiveResult()
        {
            yield return delayNActiveResult_WFS;
            Time.timeScale = 0;
            uiControll.PageActive_Result(true);
        }
    }
}