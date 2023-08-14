using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingShow : MonoBehaviour
{
    [SerializeField] RankingDataHolder rankingDataHolder;
    [SerializeField] SingleBar original_singleBar;

    [SerializeField] Transform fold_Balance;
    [SerializeField] Transform fold_Speed;
    [SerializeField] Transform fold_Obstacle;

    [SerializeField] Ranking_Balance sortedData_Balance;
    [SerializeField] Ranking_Speed sortedData_Speed;
    [SerializeField] Ranking_Obstacle sortedData_Obstacle;

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
        //밸런스
        for (int i = 0; i < sortedData_Balance.datas.Length; i++)
        {
            singleBar = Instantiate(original_singleBar);
            singleBar.gameObject.SetActive(true);
            singleBar.transform.parent = fold_Balance;
            singleBar.transform.localScale = Vector3.one;
            singleBar.RTF.anchoredPosition = new Vector3(singleBar.RTF.anchoredPosition.x, singleBar.RTF.anchoredPosition.y, 0);
            singleBar.InfoUpdate
                (sortedData_Balance.datas[i].name,
                sortedData_Balance.datas[i].time.ToString(),
                sortedData_Balance.datas[i].date.ToString());
        }
        //스피드
        for (int i = 0; i < sortedData_Speed.datas.Length; i++)
        {
            singleBar = Instantiate(original_singleBar);
            singleBar.gameObject.SetActive(true);
            singleBar.transform.parent = fold_Speed;
            singleBar.transform.localScale = Vector3.one;
            singleBar.RTF.anchoredPosition = new Vector3(singleBar.RTF.anchoredPosition.x, singleBar.RTF.anchoredPosition.y, 0);
            singleBar.InfoUpdate
                (sortedData_Speed.datas[i].name,
                sortedData_Speed.datas[i].time.ToString(),
                sortedData_Speed.datas[i].date.ToString());
        }
        //장애물
        for (int i = 0; i < sortedData_Obstacle.datas.Length; i++)
        {
            singleBar = Instantiate(original_singleBar);
            singleBar.gameObject.SetActive(true);
            singleBar.transform.parent = fold_Obstacle;
            singleBar.transform.localScale = Vector3.one;
            singleBar.RTF.anchoredPosition = new Vector3(singleBar.RTF.anchoredPosition.x, singleBar.RTF.anchoredPosition.y, 0);
            singleBar.InfoUpdate
                (sortedData_Obstacle.datas[i].name,
                sortedData_Obstacle.datas[i].time.ToString(),
                sortedData_Obstacle.datas[i].date.ToString());
        }
    }
}
