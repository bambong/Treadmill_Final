using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearDeceleration
{
    public float value { get; private set; }

    private float multiple;
    private float minValue;
    private float maxValue;

    public void Update()
    {
        value -= Time.deltaTime * multiple;
        value = Mathf.Clamp(value, minValue, maxValue);
    }
    public void ValueUpdate(float value)
    {
        if (value > this.value)
        {
            this.value = value;
        }
    }
    public void ValueReset()
    {
        this.value = minValue;
    }

    public LinearDeceleration(float minValue, float maxValue, float multiple)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.multiple = multiple;
    }
}
