using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankingShow : MonoBehaviour
{
    public enum Page { speed, obstacle, balance}

    [SerializeField] GameObject obj_window;

    [SerializeField] RankingDataHolder rankingDataHolder;
    [SerializeField] SingleBar original_singleBar;

    [SerializeField] Transform fold_Balance;
    [SerializeField] Transform fold_Speed;
    [SerializeField] Transform fold_Obstacle;

    [SerializeField] RectTransform[] rtf_pages;
    [SerializeField] RectTransform[] rtf_btns;
    [SerializeField] Image[] img_btnsClicked;

    [SerializeField] Ranking_Balance sortedData_Balance;
    [SerializeField] Ranking_Speed sortedData_Speed;
    [SerializeField] Ranking_Obstacle sortedData_Obstacle;

    Page nowPage;

    public void OnBtnClick(int type)
    {
        rtf_btns[type].transform.DOKill();
        rtf_btns[type].localScale = Vector3.one;
        rtf_btns[type].DOShakeScale(0.2f, 0.3f);
        if (nowPage != (Page)type)
        {
            rtf_pages[(int)nowPage].gameObject.SetActive(false);
            rtf_pages[type].gameObject.SetActive(true);

            img_btnsClicked[(int)nowPage].DOKill();
            img_btnsClicked[(int)nowPage].DOColor(new Color(1, 1, 1, 0), 0.2f);
            img_btnsClicked[type].DOKill();
            img_btnsClicked[type].DOColor(Color.white, 0.5f);

            nowPage = (Page)type;
        }
    }
    public void OnBtnClick_Active(bool active)
    {
        obj_window.gameObject.SetActive(active);
    }

    private void Start()
    {
        RankingData rankingData = rankingDataHolder.rankingData;

        //* 정렬
        //밸런스
        Ranking_Balance.SingleData temp_Balance;
        sortedData_Balance = rankingDataHolder.rankingData.ranking_Balance.Copy();
        for (int i = sortedData_Balance.datas.Length - 1; i > 0; i--) 
        {
            for (int j = 0; j < i; j++)
            {
                if (sortedData_Balance.datas[j].time < sortedData_Balance.datas[j + 1].time)
                {
                    temp_Balance = sortedData_Balance.datas[j];
                    sortedData_Balance.datas[j] = sortedData_Balance.datas[j + 1];
                    sortedData_Balance.datas[j + 1] = temp_Balance;
                }
            }
        }
        //스피드
        Ranking_Speed.SingleData temp_Speed;
        sortedData_Speed = rankingDataHolder.rankingData.ranking_Speed.Copy();
        for (int i = sortedData_Speed.datas.Length - 1; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                if (sortedData_Speed.datas[j].time < sortedData_Speed.datas[j + 1].time)
                {
                    temp_Speed = sortedData_Speed.datas[j];
                    sortedData_Speed.datas[j] = sortedData_Speed.datas[j + 1];
                    sortedData_Speed.datas[j + 1] = temp_Speed;
                }
            }
        }
        //장애물
        Ranking_Obstacle.SingleData temp_Obstacle;
        sortedData_Obstacle = rankingDataHolder.rankingData.ranking_Obstacle.Copy();
        for (int i = sortedData_Obstacle.datas.Length - 1; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                if (sortedData_Obstacle.datas[j].time < sortedData_Obstacle.datas[j + 1].time)
                {
                    temp_Obstacle = sortedData_Obstacle.datas[j];
                    sortedData_Obstacle.datas[j] = sortedData_Obstacle.datas[j + 1];
                    sortedData_Obstacle.datas[j + 1] = temp_Obstacle;
                }
            }
        }

        //* 정보 보여주기
        SingleBar singleBar;
        string[] strings = new string[4];
        //밸런스
        for (int i = 0; i < sortedData_Balance.datas.Length; i++)
        {
            singleBar = Instantiate(original_singleBar);
            singleBar.gameObject.SetActive(true);
            singleBar.transform.parent = fold_Balance;
            singleBar.transform.localScale = Vector3.one;
            singleBar.RTF.localPosition = Vector3.zero;

            singleBar.InfoUpdate(Ranking_Balance.RankingShowData(sortedData_Balance.datas[i]));
        }
        //스피드
        for (int i = 0; i < sortedData_Speed.datas.Length; i++)
        {
            singleBar = Instantiate(original_singleBar);
            singleBar.gameObject.SetActive(true);
            singleBar.transform.parent = fold_Speed;
            singleBar.transform.localScale = Vector3.one;
            singleBar.RTF.localPosition = Vector3.zero;

            singleBar.InfoUpdate(Ranking_Speed.RankingShowData(sortedData_Speed.datas[i]));
        }
        //장애물
        for (int i = 0; i < sortedData_Obstacle.datas.Length; i++)
        {
            singleBar = Instantiate(original_singleBar);
            singleBar.gameObject.SetActive(true);
            singleBar.transform.parent = fold_Obstacle;
            singleBar.transform.localScale = Vector3.one;
            singleBar.RTF.localPosition = Vector3.zero;

            singleBar.InfoUpdate(Ranking_Obstacle.RankingShowData(sortedData_Obstacle.datas[i]));
        }
    }
}
