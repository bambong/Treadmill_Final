using bambong;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FansAnimController : MonoBehaviour
{
    private Vector3 startPos;
    private readonly float JUMP_MAX_HEIGHT = 2f;
    private readonly float JUMP_MAX_TIME = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        GameSceneManager.Instance.OnGameStart += OnGameStart;
        GameSceneManager.Instance.OnGameClear += OnGameClear;
    }
    public void OnGameClear() 
    {
        StopAllCoroutines();
    }  
    public void OnGameStart() 
    {
        StartCoroutine(JumpAnim());
    }

    IEnumerator JumpAnim()
    {
        while (true) 
        {
            var randH = Random.Range(0.5f, JUMP_MAX_HEIGHT);
            var time = (randH / JUMP_MAX_HEIGHT) * JUMP_MAX_TIME;
            transform.DOLocalMoveY(startPos.y + randH, time).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(time*2 + Random.Range(0, 2f));
        }
    }

}
