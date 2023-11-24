using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExerciseCalculation
{
    public static double GetCo2(double curMeter) // �ִ� ��� ���뷮 ����
    {
        double result = 5.189f + (2.768f * (curMeter * 0.001f));
        return Math.Round(result, 1);
    }
    
    public static float GetCo2(float curMeter) // �ִ� ��� ���뷮 ����
    {
        double result = 5.189f + (2.768f * (curMeter * 0.001f));
        return (float)Math.Round(result,1);
    }

    public static float GetCalorie(double co2 , float time ) // Į�θ� ��� ���� ���� ����
    {
        double result = ((co2 * Managers.Data.UserData.weight * (time / 60.0f)) * 0.001f) * 5.0f;
        return (float)Math.Ceiling(result);
    }

    public static float GetCalorie(float time) // Į�θ� �Ի� ���� ���ο� ����
    {
        return time *(0.0175f * Managers.Data.UserData.weight)/60f;
    }
}
