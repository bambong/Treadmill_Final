using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using bambong;

public class TutorialStartAsk : MonoBehaviour
{
    [SerializeField] UiShadow2 shadow;
    [SerializeField] Transform[] tf_ask;

    public void TutorialEnter(bool enter)
    {
        for (int i = 0; i < tf_ask.Length; i++)
        {
            tf_ask[i].DOKill();
            tf_ask[i].DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuart);
        }

        if (enter)
        {
            TransitionManager.Instance.SceneTransition(E_SceneName.Balance_GameScene_Tutorial.ToString());
        }
        else
        {
            TransitionManager.Instance.SceneTransition(E_SceneName.Balance_GameScene_1.ToString());
        }
    }

    private void StartAsk()
    {
        shadow.SetActive(true);
        shadow.SetColor(Color.black);
        shadow.SetAlpha(0.75f, true, 0.3f);

        for (int i = 0; i < tf_ask.Length; i++)
        {
            tf_ask[i].DOKill();
            tf_ask[i].localScale = Vector3.zero;
            tf_ask[i].DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuart);
        }
    }

    private void Awake()
    {
        StartAsk();
    }
}
