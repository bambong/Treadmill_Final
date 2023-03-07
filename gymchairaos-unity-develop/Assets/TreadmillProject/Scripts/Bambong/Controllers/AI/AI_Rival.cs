using bambong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Rival : AI_CharController
{
    private readonly float minSpeed = 5f;
    protected override void ChangeSpeed()
    {
        curMoveSpeed =  Mathf.Max(GameSceneManager.Instance.Player.GetCurSpeed() * Random.Range(0.8f,0.95f),minSpeed);
    }
}
