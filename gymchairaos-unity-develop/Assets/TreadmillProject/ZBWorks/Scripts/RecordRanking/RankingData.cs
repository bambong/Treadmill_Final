using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gymchair.Core.Mgr.Behaviour;
using Gymchair.Core.Mgr;

[System.Serializable]
public class RankingData
{
    public Ranking_Balance ranking_Balance;
    public Ranking_Speed ranking_Speed;
    public Ranking_Obstacle ranking_Obstacle;

    public RankingData()
    {
        ranking_Balance = new Ranking_Balance();
        ranking_Speed = new Ranking_Speed();
        ranking_Obstacle = new Ranking_Obstacle();
    }

    public static string GetUserName()
    {
        return Managers.Data.UserName;
    }

    public static string GetDate()
    {
        string year = DateTime.Now.Year.ToString("00");
        string month = DateTime.Now.Month.ToString("00");
        string day = DateTime.Now.Day.ToString("00");

        return $"{year}.{month}.{day}";
    }
}

[System.Serializable]
public class Ranking_Balance
{
    public SingleData[] datas;

    public void Add(string name, string date, float time, int stage)
    {
        SingleData[] newDatas = new SingleData[datas.Length + 1];
        for (int i = 0; i < this.datas.Length; i++)
        {
            newDatas[i].name = this.datas[i].name;
            newDatas[i].date = this.datas[i].date;
            newDatas[i].time = this.datas[i].time;
            newDatas[i].stage = this.datas[i].stage;
        }
        newDatas[newDatas.Length - 1].name = name;
        newDatas[newDatas.Length - 1].date = date;
        newDatas[newDatas.Length - 1].time = time;
        newDatas[newDatas.Length - 1].stage = stage;

        datas = newDatas;
    }
    public Ranking_Balance Copy()
    {
        Ranking_Balance result = new Ranking_Balance();
        result.datas = new SingleData[this.datas.Length];

        for (int i = 0; i < this.datas.Length; i++)
        {
            result.datas[i].name = this.datas[i].name;
            result.datas[i].date = this.datas[i].date;
            result.datas[i].time = this.datas[i].time;
            result.datas[i].stage = this.datas[i].stage;
        }

        return result;
    }
    public static string[] RankingShowData(SingleData singleData)
    {
        string[] result = new string[4];
        result[0] = singleData.name;
        result[1] = TimeCounter.FormatTime(singleData.time);
        result[2] = $"{singleData.stage.ToString()}´Ü°è";
        result[3] = singleData.date;
        return result;
    }

    [System.Serializable]
    public struct SingleData
    {
        public string name;
        public string date;
        public float time;
        public float stage;
    }
}
[System.Serializable]
public class Ranking_Speed
{
    public SingleData[] datas;

    public void Add(string name, string date, float time, int stage)
    {
        SingleData[] newDatas = new SingleData[datas.Length + 1];
        for (int i = 0; i < this.datas.Length; i++)
        {
            newDatas[i].name = this.datas[i].name;
            newDatas[i].date = this.datas[i].date;
            newDatas[i].time = this.datas[i].time;
            newDatas[i].stage = this.datas[i].stage;
        }
        newDatas[newDatas.Length - 1].name = name;
        newDatas[newDatas.Length - 1].date = date;
        newDatas[newDatas.Length - 1].time = time;
        newDatas[newDatas.Length - 1].stage = stage;

        datas = newDatas;
    }
    public Ranking_Speed Copy()
    {
        Ranking_Speed result = new Ranking_Speed();
        result.datas = new SingleData[this.datas.Length];

        for (int i = 0; i < this.datas.Length; i++)
        {
            result.datas[i].name = this.datas[i].name;
            result.datas[i].date = this.datas[i].date;
            result.datas[i].time = this.datas[i].time;
            result.datas[i].stage = this.datas[i].stage;
        }

        return result;
    }
    public static string[] RankingShowData(SingleData singleData)
    {
        string[] result = new string[4];
        result[0] = singleData.name;
        result[1] = TimeCounter.FormatTime(singleData.time);
        result[2] = $"{singleData.stage.ToString()}M";
        result[3] = singleData.date;
        return result;
    }

    [System.Serializable]
    public struct SingleData
    {
        public string name;
        public string date;
        public int stage;
        public float time;
    }
}
[System.Serializable]
public class Ranking_Obstacle
{
    public SingleData[] datas;

    public void Add(string name, string date, float time, float dist)
    {
        SingleData[] newDatas = new SingleData[datas.Length + 1];
        for (int i = 0; i < this.datas.Length; i++)
        {
            newDatas[i].name = this.datas[i].name;
            newDatas[i].date = this.datas[i].date;
            newDatas[i].time = this.datas[i].time;
            newDatas[i].dist = this.datas[i].dist;
        }
        newDatas[newDatas.Length - 1].name = name;
        newDatas[newDatas.Length - 1].date = date;
        newDatas[newDatas.Length - 1].time = time;
        newDatas[newDatas.Length - 1].dist = dist;

        datas = newDatas;
    }
    public Ranking_Obstacle Copy()
    {
        Ranking_Obstacle result = new Ranking_Obstacle();
        result.datas = new SingleData[this.datas.Length];

        for (int i = 0; i < this.datas.Length; i++)
        {
            result.datas[i].name = this.datas[i].name;
            result.datas[i].date = this.datas[i].date;
            result.datas[i].time = this.datas[i].time;
            result.datas[i].dist = this.datas[i].dist;
        }

        return result;
    }
    public static string[] RankingShowData(SingleData singleData)
    {
        string[] result = new string[4];
        result[0] = singleData.name;
        result[1] = TimeCounter.FormatTime(singleData.time);
        result[2] = $"{((int)singleData.dist).ToString()}M";
        result[3] = singleData.date;
        return result;
    }

    [System.Serializable]
    public struct SingleData
    {
        public string name;
        public string date;
        public float time;
        public float dist;
    }
}