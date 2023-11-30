using bambong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Rival : AI_CharController
{
    private readonly float minSpeed = 7f;
    private readonly float distRatio = 0.02f;
    private readonly float speedMultiple = 1.3f;

    protected override void ChangeSpeed()
    {
        //curMoveSpeed =  Mathf.Max(GameSceneManager.Instance.Player.GetCurSpeed() * Random.Range(0.8f,0.95f),minSpeed);
        
        //뒤처지면 플레이어 속도보다 좀 더 빠르게
        if (GameSceneManager.Instance.GetCurDistanceRatio(transform) + distRatio < GameSceneManager.Instance.GetPlayerCurDistanceRatio())
        {
            //curMoveSpeed = GameSceneManager.Instance.Player.GetCurSpeed() * 1.3f;
            curMoveSpeed = curMoveSpeed + Time.deltaTime * 2 * (GameSceneManager.Instance.Player.GetCurSpeed() * speedMultiple - curMoveSpeed);
        }

        //앞서가면 최소속도로
        else
        {
            curMoveSpeed = minSpeed;
        }
    }
}
