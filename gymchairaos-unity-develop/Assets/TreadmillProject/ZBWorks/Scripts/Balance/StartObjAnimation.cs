using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartObjAnimation : MonoBehaviour
{
    [SerializeField] Vector3 p3_front;
    [SerializeField] Vector3 p3_back;
    [SerializeField] Vector3 p3_original;
    [SerializeField] float duration_toFront;
    [SerializeField] float duration_toBack;

    [ContextMenu("Front")]
    public void Ani_ToFront()
    {
        transform.DOKill();
        transform.position = p3_front;
        transform.DOMove(p3_original, duration_toFront).SetEase(Ease.InOutQuart);
    }
    [ContextMenu("Back")]
    public void Ani_ToBack()
    {
        transform.DOKill();
        transform.DOMove(p3_back, duration_toBack).SetEase(Ease.InOutQuart);
    }
}
