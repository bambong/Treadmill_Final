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
        [SerializeField] Rigidbody rb_player;
        [SerializeField] Transform resetPos;

        [SerializeField] UIControll uiControll;

        [SerializeField] RotateObj rotateObj;
        [SerializeField] RotateObjInput input;
        [SerializeField] TimeCounter timeCounter;

        [SerializeField] TextMeshProUGUI tmp_Ready;
        [SerializeField] Image img_Shadow;
        [SerializeField] Color color_littleShow;

        [SerializeField] RankingDataHolder rankingDataHolder;
        [SerializeField] int stage;

        public void GameStart()
        {
            StartCoroutine(StartCycle());
        }

        public void GameClear()
        {
            uiControll.PageActive_Result(true);
            timeCounter.CountPause();

            //클리어판정
            rankingDataHolder.rankingData.ranking_Balance.Add(RankingData.GetUserName(), RankingData.GetDate(), timeCounter.NowTime, stage);
            rankingDataHolder.Write();
        }

        public void GameOver()
        {
            rotateObj.ResetState();
            input.ResetState();
            input.Active(false);
            PlayerReset();
            timeCounter.CountPause();
        }

        public void Pause(bool active)
        {
            Time.timeScale = active ? 0 : 1;
            if (active) timeCounter.CountPause();
            else timeCounter.CountStart();
        }

        private void PlayerReset()
        {
            rb_player.position = resetPos.position;
            rb_player.velocity = Vector3.zero;
            rb_player.useGravity = false;
        }

        private void Start()
        {
            img_Shadow.color = Color.black;

            GameStart();
        }

        IEnumerator StartCycle()
        {
            input.Active(false);
            img_Shadow.gameObject.SetActive(true);
            Time.timeScale = 1;
            timeCounter.CountStop();
            img_Shadow.DOColor(Color.black, 0.5f);
            yield return new WaitForSeconds(0.5f);

            input.ResetState();
            rotateObj.ResetState();
            PlayerReset();
            img_Shadow.DOColor(color_littleShow, 0.75f);
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
            img_Shadow.DOColor(Color.clear, 0.5f).OnComplete(()=>img_Shadow.gameObject.SetActive(false));
            timeCounter.CountStart();
            rb_player.useGravity = true;
            input.Active(true);
        }
    }
}