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
 
    //* ������ �ʱ�ȭ
    private void TryRead()
    {
        //���� ���� �������, �ҷ�����
        if (TXTUtility.FileExists(path, AccessStyle.PersistentDataPath))
        {
            string jsonData = TXTUtility.Read(path, AccessStyle.PersistentDataPath);
            rankingData = TXTUtility.Json_2_T<RankingData>(jsonData);
        }

        //���� ���� �������, ���θ����
        else
        {
            rankingData = new RankingData();
            string jsonData = TXTUtility.T_2_Json(rankingData);
            TXTUtility.Write(path, jsonData, AccessStyle.PersistentDataPath);
        }
    }
}
