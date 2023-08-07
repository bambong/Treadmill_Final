using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSingleObstacle : SingleObstacle
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDir;

    public override void OnTouched_WithPlayer()
    {
        base.OnTouched_WithPlayer();
        StopCoroutine(MoveCycle_C);

    }
    protected override void Enable()
    {
        isDynamic = true;
        if (MoveCycle_C != null)
            StopCoroutine(MoveCycle_C);
        MoveCycle_C = MoveCycle();
        StartCoroutine(MoveCycle_C);
    }
    protected override void Disable()
    {
        StopCoroutine(MoveCycle_C);
    }

    IEnumerator MoveCycle_C;
    IEnumerator MoveCycle()
    {
        while(true)
        {
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    private void Awake()
    {
        Init();
    }
}
