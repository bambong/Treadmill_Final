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
        
        //��ó���� �÷��̾� �ӵ����� �� �� ������
        if (GameSceneManager.Instance.GetCurDistanceRatio(transform) + distRatio < GameSceneManager.Instance.GetPlayerCurDistanceRatio())
        {
            //curMoveSpeed = GameSceneManager.Instance.Player.GetCurSpeed() * 1.3f;
            curMoveSpeed = curMoveSpeed + Time.deltaTime * 2 * (GameSceneManager.Instance.Player.GetCurSpeed() * speedMultiple - curMoveSpeed);
        }

        //�ռ����� �ּҼӵ���
        else
        {
            curMoveSpeed = minSpeed;
        }
    }
}
