using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ZB.Balance
{
    public class CountDown : MonoBehaviour
    {
        [SerializeField] UiShadow m_uiShadow;
        [SerializeField] UnityEvent m_uEvent_OnCountStart;
        [SerializeField] UnityEvent m_uEvent_OnCountEnd;
        [SerializeField] TextMeshProUGUI m_tmp_countText;

        /// <summary>
        /// ī��Ʈ ����, ���� �� ���ִ� �̺�Ʈ Invoke
        /// </summary>
        /// <param name="time"></param>
        [ContextMenu("Start")]
        public void TestCountStart()
        {
            CountStart(3);
        }

        public void CountStart(int time)
        {
            m_tmp_countText.gameObject.SetActive(true);

            if (Count_C != null)
                StopCoroutine(Count_C);
            Count_C = Count(time);
            StartCoroutine(Count_C);
        }

        WaitForSeconds Count_WFS_Duration_FadeUp = new WaitForSeconds(0.5f);
        WaitForSeconds Count_WFS_Duration_FadeDown = new WaitForSeconds(0.5f);
        IEnumerator Count_C;
        IEnumerator Count(int countTime)
        {
            m_uiShadow.Active(true);

            m_uEvent_OnCountStart.Invoke();

            m_tmp_countText.transform.localScale = Vector2.zero;
            while (countTime > 0)
            {
                //���⼭ �����ִ� ����
                m_tmp_countText.text = countTime.ToString();

                m_tmp_countText.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutQuart);
                yield return Count_WFS_Duration_FadeUp;

                m_tmp_countText.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.OutQuart);
                yield return Count_WFS_Duration_FadeDown;
                countTime -= 1;
            }

            m_uiShadow.Active(false);

            m_tmp_countText.gameObject.SetActive(false);
            m_uEvent_OnCountEnd.Invoke();
        }

        private void Start()
        {
            m_tmp_countText.transform.localScale = Vector2.zero;
            m_tmp_countText.gameObject.SetActive(false);

            CountStart(3);
        }
    }
}