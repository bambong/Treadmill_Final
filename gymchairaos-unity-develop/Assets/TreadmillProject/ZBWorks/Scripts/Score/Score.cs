using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;

namespace ZB
{
    public class Score : MonoBehaviour
    {
        [SerializeField] DistanceRecord distance;
        [SerializeField] ScoreUI ui;

        public float NowScore { get { return score; } }
        [Space]
        //��������
        [SerializeField] float score;
        //�ڵ�����ȹ�� �Ÿ�
        [SerializeField] float scoreGainDistance;
        //�ڵ�����ȹ�� ������
        [SerializeField] float scoreGainValue;
        float currentCheckedDistance = 0;

        //�Ÿ���� �ڵ����� ȹ�� ����
        [ContextMenu("�ڵ�����ȹ�� ����")]
        public void StartAutoScoreUpByDistance()
        {
            if (AutorScoreUpByDistance_C != null)
            {
                StopCoroutine(AutorScoreUpByDistance_C);
            }
            AutorScoreUpByDistance_C = AutorScoreUpByDistance();
            StartCoroutine(AutorScoreUpByDistance_C);
        }

        //�Ÿ���� �ڵ����� ȹ�� ����
        [ContextMenu("�ڵ�����ȹ�� ����")]
        public void StopAutoScoreUpByDistance()
        {
            currentCheckedDistance = 0;
            if (AutorScoreUpByDistance_C != null)
            {
                StopCoroutine(AutorScoreUpByDistance_C);
            }
        }

        //���� ���� �ʱ�ȭ
        [ContextMenu("���� �ʱ�ȭ")]
        public void ResetScore()
        {
            score = 0;
            ui.OnScoreChanged();
        }

        //����ȹ�� (���� ����)
        public void PlusScore(int value)
        {
            score += value;
            ui.OnScoreChanged();
        }

        IEnumerator AutorScoreUpByDistance_C;
        IEnumerator AutorScoreUpByDistance()
        {
            while (true)
            {
                if (distance.NowRecord > currentCheckedDistance + scoreGainDistance)
                {
                    currentCheckedDistance += scoreGainDistance;
                    score += scoreGainValue;
                    ui.OnScoreChanged();
                }


                yield return null;
            }
        }
    }
}