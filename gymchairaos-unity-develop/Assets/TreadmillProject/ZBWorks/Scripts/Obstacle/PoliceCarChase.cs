using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PoliceCarChase : MonoBehaviour
{
    [SerializeField] Transform[] moveTargets;
    [SerializeField] Transform body;

    [Space]
    [SerializeField] float[] ui_MoveTargets;
    [SerializeField] RectTransform ui_Body;
    [SerializeField] float[] ui_MoveTargets_Player;
    [SerializeField] RectTransform ui_Body_Player;

    public void Move(int hpInfo)
    {
        if (hpInfo < 3) 
            Managers.Sound.PlayEffect("sfx_obstacleSiren");

        if (hpInfo == 0)
        {
            ui_Body_Player.DOKill();
            ui_Body_Player.DOAnchorPosY(ui_MoveTargets_Player[0], 0.75f);
        }
        else if (hpInfo == 3)
        {
            ui_Body_Player.DOKill();
            ui_Body_Player.DOAnchorPosY(ui_MoveTargets_Player[1], 0.75f);
        }

        switch(hpInfo)
        {
            //초기위치, 사망
            case 0:
            case 3:
                body.DOKill();
                body.DOMove(moveTargets[0].position, 0.75f);
                ui_Body.DOKill();
                ui_Body.DOAnchorPosY(ui_MoveTargets[0], 0.75f);
                break;

            //1회 피격
            case 2:
                body.DOKill();
                body.DOMove(moveTargets[1].position, 0.75f);
                ui_Body.DOKill();
                ui_Body.DOAnchorPosY(ui_MoveTargets[1], 0.75f);
                break;

            //2회 피격
            case 1:
                body.DOKill();
                body.DOMove(moveTargets[2].position, 0.75f);
                ui_Body.DOKill();
                ui_Body.DOAnchorPosY(ui_MoveTargets[2], 0.75f);
                break;
        }
    }
}
