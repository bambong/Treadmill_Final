using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

[System.Serializable]
public class Ranking_Balance
{
    public SingleData[] datas;

    public Ranking_Balance Copy()
    {
        Ranking_Balance result = new Ranking_Balance();
        result.datas = new SingleData[this.datas.Length];

        for (int i = 0; i < this.datas.Length; i++)
        {
            result.datas[i].name = this.datas[i].name;
            result.datas[i].date = this.datas[i].date;
            result.datas[i].time = this.datas[i].time;
        }

        return result;
    }

    [System.Serializable]
    public struct SingleData
    {
        public string name;
        public float date;
        public float time;
    }
}
[System.Serializable]
public class Ranking_Speed
{
    public SingleData[] datas;

    public Ranking_Speed Copy()
    {
        Ranking_Speed result = new Ranking_Speed();
        result.datas = new SingleData[this.datas.Length];

        for (int i = 0; i < this.datas.Length; i++)
        {
            result.datas[i].name = this.datas[i].name;
            result.datas[i].date = this.datas[i].date;
            result.datas[i].time = this.datas[i].time;
        }

        return result;
    }

    [System.Serializable]
    public struct SingleData
    {
        public string name;
        public float date;
        public float time;
    }
}
[System.Serializable]
public class Ranking_Obstacle
{
    public SingleData[] datas;

    public Ranking_Obstacle Copy()
    {
        Ranking_Obstacle result = new Ranking_Obstacle();
        result.datas = new SingleData[this.datas.Length];

        for (int i = 0; i < this.datas.Length; i++)
        {
            result.datas[i].name = this.datas[i].name;
            result.datas[i].date = this.datas[i].date;
            result.datas[i].time = this.datas[i].time;
        }

        return result;
    }

    [System.Serializable]
    public struct SingleData
    {
        public string name;
        public float date;
        public float time;
    }
}