using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    public static DelayedEvent instance;

    public void AddEvent(float waitTime, UnityAction unityAction)
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(unityAction);
        WaitForSeconds wfs = new WaitForSeconds(waitTime);
        StartCoroutine(DelayedEventCycle(wfs, unityEvent));
    }

    IEnumerator DelayedEventCycle(WaitForSeconds wfs, UnityEvent unityEvent)
    {
        yield return wfs;
        unityEvent.Invoke();
    }

    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
