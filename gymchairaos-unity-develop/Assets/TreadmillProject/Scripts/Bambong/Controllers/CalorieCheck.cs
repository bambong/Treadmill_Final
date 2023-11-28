using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalorieCheck
{
    public float CurCalorie { get => curCalorie; }

    private float curCalorie = 0;
    private float curCalorieTime = 0;
    private long totalBPM = 0;
    private int bpmCount = 0;

    public CalorieCheck() { }

    public void Update()
    {
        curCalorieTime += Time.deltaTime;
        totalBPM += (long)Managers.Token.Bpm;
        ++bpmCount;

        if (curCalorieTime > 60)
        {
            curCalorieTime = 0;

            curCalorie += ExerciseCalculation.GetCalorieMin((totalBPM / (float)bpmCount));

            bpmCount = 0;
            totalBPM = 0;
        }
    }
    public string GetString()
    {
        return string.Format("{0:0.00}", CurCalorie);
    }
}
