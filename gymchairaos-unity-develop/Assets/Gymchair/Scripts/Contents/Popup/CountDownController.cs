using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownController : MonoBehaviour
{
    Action _finish;

    [SerializeField] Image _img;
    [SerializeField] Sprite[] _countdown;

    public static CountDownController Create(Action finish = null)
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/CountDownPopup");
        GameObject popup = Instantiate(obj);
        CountDownController script = popup.GetComponent<CountDownController>();
        script._finish = finish;
        return script;
    }

    // Start is called before the first frame update
    void Start()
    {
        Managers.Sound.PlayEffect("count");
        StartCoroutine(onAnimation());
    }

    IEnumerator onAnimation()
    {
        int count = _countdown.Length;

        for (int num = 0; num < count; num++)
        {
            yield return new WaitForSeconds(0.1f);
            _img.sprite = _countdown[num];
        }

        yield return new WaitForSeconds(0.5f);

        _finish?.Invoke();
        Destroy(gameObject);
    }
}
