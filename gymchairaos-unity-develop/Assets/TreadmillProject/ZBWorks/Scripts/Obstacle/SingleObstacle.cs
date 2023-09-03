using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SingleObstacle : MonoBehaviour
{
    public bool IsDynamic { get => isDynamic; }

    [SerializeField] ZB.BoostGuage boostGuage;
    ZB.ObjectsScrolling objectScrolling;
    [SerializeField] GameObject body;
    [SerializeField] float delayTime;
    protected bool isDynamic;

    bool unactiving;

    public virtual void OnTouched_WithPlayer()
    {
        if (boostGuage.NowState == ZB.BoostGuage.State.Boost)
        {
            objectScrolling.RemoveObj(transform);
            transform.DOMove(new Vector3(Random.Range(-3, 4), 2, Random.Range(-3, 4)), 0.5f).SetEase(Ease.OutQuart);
            transform.DOScale(Vector3.zero, 1);
            transform.DOShakeRotation(0.75f);
            if (gameObject.activeSelf && !unactiving)
                StartCoroutine(DelayNUnActiveC());
        }
        else
        {
            if (gameObject.activeSelf && !unactiving)
                StartCoroutine(DelayNUnActiveC());
        }
    }

    protected virtual void Enable()
    {

    }
    protected virtual void Disable()
    {

    }

    protected void Init()
    {
        boostGuage = FindObjectOfType<ZB.BoostGuage>();
        objectScrolling = FindObjectOfType<ZB.ObjectsScrolling>();
        DelayNUnActive_WFS = new WaitForSeconds(delayTime);
    }
    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        Enable();
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnDisable()
    {
        Disable();
        unactiving = false;
    }

    WaitForSeconds DelayNUnActive_WFS;
    IEnumerator DelayNUnActiveC()
    {
        unactiving = true;
        yield return DelayNUnActive_WFS;
        body.SetActive(false);
    }
}
