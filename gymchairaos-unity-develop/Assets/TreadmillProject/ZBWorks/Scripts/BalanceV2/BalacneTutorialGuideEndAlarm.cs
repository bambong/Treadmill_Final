using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace ZB.Balance2
{
    public class BalacneTutorialGuideEndAlarm : MonoBehaviour
    {
        [SerializeField] UiShadow2 shadow;
        [SerializeField] Transform tf_tmp;
        [SerializeField] TextMeshProUGUI tmp;

        public void Alarm()
        {
            shadow.SetActive(true);
            shadow.SetAlpha(0.75f, true, 0.75f, () =>
            {
                tmp.gameObject.SetActive(true);
                tmp.text = "축하합니다!\n이제 튜토리얼을 완료하였습니다!";
                tf_tmp.localScale = Vector3.zero;
                tf_tmp.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuart).OnComplete(()=>
                {
                    tf_tmp.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuart).SetDelay(2.25f).OnComplete(() =>
                    {
                        tmp.text = "이제 본격적으로 시작해볼까요?";
                        tf_tmp.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuart).OnComplete(() =>
                        {
                            tf_tmp.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuart).OnComplete(() =>
                            {
                                bambong.TransitionManager.Instance.SceneTransition(bambong.E_SceneName.Balance_GameScene_1.ToString());
                            });
                        });
                    });
                });
            });
        }
    }
}