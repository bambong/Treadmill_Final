using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExerciseCalculation
{
    public static double GetCo2(double curMeter) // 최대 산소 섭취량 공식
    {
        double result = 5.189f + (2.768f * (curMeter * 0.001f));
        return Math.Round(result, 1);
    }
    
    public static float GetCo2(float curMeter) // 최대 산소 섭취량 공식
    {
        double result = 5.189f + (2.768f * (curMeter * 0.001f));
        return (float)Math.Round(result,1);
    }

    public static float GetCalorie(double co2 , float time ) // 칼로리 계산 공식 기존 버전
    {
        double result = ((co2 * Managers.Data.UserData.weight * (time / 60.0f)) * 0.001f) * 5.0f;
        return (float)Math.Ceiling(result);
    }

    public static float GetCalorie(float time) // 칼로리 게산 공식 새로운 버전
    {
        return time *(0.0175f * Managers.Data.UserData.weight)/60f;
    }
}
