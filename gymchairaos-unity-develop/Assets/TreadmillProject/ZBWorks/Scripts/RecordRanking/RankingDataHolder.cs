using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB.TextFile;

public class RankingDataHolder : MonoBehaviour
{
    public RankingData rankingData;

    [SerializeField] private string path;

    public void Write()
    {
        string jsonData = TXTUtility.T_2_Json(rankingData);
        TXTUtility.Write(path, jsonData, AccessStyle.PersistentDataPath);
    }

    private void Awake()
    {
        TryRead();
    }
 
    //* 데이터 초기화
    private void TryRead()
    {
        //기존 파일 있을경우, 불러오기
        if (TXTUtility.FileExists(path, AccessStyle.PersistentDataPath))
        {
            string jsonData = TXTUtility.Read(path, AccessStyle.PersistentDataPath);
            rankingData = TXTUtility.Json_2_T<RankingData>(jsonData);
        }

        //기존 파일 없을경우, 새로만들기
        else
        {
            rankingData = new RankingData();
            string jsonData = TXTUtility.T_2_Json(rankingData);
            TXTUtility.Write(path, jsonData, AccessStyle.PersistentDataPath);
        }
    }
}
