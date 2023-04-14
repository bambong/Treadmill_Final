using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObstacle : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] float delayTime;

    bool unactiving;

    public void DelayNUnActive()
    {
        if (gameObject.activeSelf) 
            if (!unactiving) 
                StartCoroutine(DelayNUnActiveC());
    }

    void Awake()
    {
        DelayNUnActive_WFS = new WaitForSeconds(delayTime);
    }
    void OnDisable()
    {
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
