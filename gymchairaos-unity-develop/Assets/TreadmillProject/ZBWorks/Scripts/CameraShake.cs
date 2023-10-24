using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Transform cameraObj;

    [SerializeField] float duration;
    [SerializeField] float strength;
    [SerializeField] int vibrato;

    Vector3 resetPos;

    [ContextMenu("!")]
    public void Shake()
    {
        cameraObj.DOKill();
        cameraObj.transform.position = resetPos;
        cameraObj.transform.DOShakePosition(duration, strength, vibrato);
    }

    private void Awake()
    {
        resetPos = cameraObj.position;
    }
}
