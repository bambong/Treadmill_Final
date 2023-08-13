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

    [System.Serializable]
    public class SingleData
    {
        public string name;
        public float date;
    }
}
[System.Serializable]
public class Ranking_Speed
{
    public SingleData[] datas;

    [System.Serializable]
    public class SingleData
    {
        public string name;
        public float date;
    }
}
[System.Serializable]
public class Ranking_Obstacle
{
    public SingleData[] datas;

    [System.Serializable]
    public class SingleData
    {
        public string name;
        public float date;
    }
}