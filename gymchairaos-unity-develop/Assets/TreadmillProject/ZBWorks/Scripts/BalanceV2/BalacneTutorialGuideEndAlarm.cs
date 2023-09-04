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
                tmp.text = "�����մϴ�!\n���� Ʃ�丮���� �Ϸ��Ͽ����ϴ�!";
                tf_tmp.localScale = Vector3.zero;
                tf_tmp.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuart).OnComplete(()=>
                {
                    tf_tmp.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuart).SetDelay(2.25f).OnComplete(() =>
                    {
                        tmp.text = "���� ���������� �����غ����?";
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