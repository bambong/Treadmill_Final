using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceImageGuide : MonoBehaviour
{
    [SerializeField] private ZB.Balance2.RotateObj rotateObj;
    [SerializeField] private ImageGuide imageGuide;
    [SerializeField] private ZB.Balance2.StageManager stageManager;
    [SerializeField] private GameObject clearArea;

    private void Awake()
    {
        imageGuide.AddCondition(0, ()=>rotateObj.IsRotatingPitchMinus(-20));
        imageGuide.AddCondition(1, ()=>rotateObj.IsRotatingPitchPlus(20));
        imageGuide.AddCondition(2, ()=>rotateObj.IsRotatingRollMinus(-20));
        imageGuide.AddCondition(3, ()=>rotateObj.IsRotatingRollPlus(20));

        imageGuide.AddGuideEndAction(stageManager.GameStart);
        imageGuide.AddGuideEndAction(()=>StartCoroutine(DelayNClearAreaActive()));

        imageGuide.InitNStart();
    }

    IEnumerator DelayNClearAreaActive()
    {
        yield return new WaitForSeconds(1);
        clearArea.SetActive(true);
    }
}