using System;
using System.Collections.Generic;

[Serializable]
public class UserGymData
{
    public string gymDate; // 시작 일자 
    public float gymTime; // 운동 시간
    public float gymMeter; // 이동 거리
    public float gymCalorie; // 소모 칼로리 

    public float speed; // 평균 
    public float high_speed; // 최대
    public float rpm; // 평균
    public float low_rpm; // 최소 
    public float high_rpm; // 최대 
    public float bpm; // 평균 
    public float low_bpm; // 최소 
    public float high_bpm; // 최대 

    public GymchairData[] gymchairDatas;
}

