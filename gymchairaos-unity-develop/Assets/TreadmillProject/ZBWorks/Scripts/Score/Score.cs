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
        //ÇöÀçÁ¡¼ö
        [SerializeField] float score;
        //ÀÚµ¿Á¡¼öÈ¹µæ °Å¸®
        [SerializeField] float scoreGainDistance;
        //ÀÚµ¿Á¡¼öÈ¹µæ Á¡¼ö·®
        [SerializeField] float scoreGainValue;
        float currentCheckedDistance = 0;

        //°Å¸®ºñ·Ê ÀÚµ¿Á¡¼ö È¹µæ ½ÃÀÛ
        [ContextMenu("ÀÚµ¿Á¡¼öÈ¹µæ ½ÃÀÛ")]
        public void StartAutoScoreUpByDistance()
        {
            if (AutorScoreUpByDistance_C != null)
            {
                StopCoroutine(AutorScoreUpByDistance_C);
            }
            AutorScoreUpByDistance_C = AutorScoreUpByDistance();
            StartCoroutine(AutorScoreUpByDistance_C);
        }

        //°Å¸®ºñ·Ê ÀÚµ¿Á¡¼ö È¹µæ Á¾·á
        [ContextMenu("ÀÚµ¿Á¡¼öÈ¹µæ Á¾·á")]
        public void StopAutoScoreUpByDistance()
        {
            currentCheckedDistance = 0;
            if (AutorScoreUpByDistance_C != null)
            {
                StopCoroutine(AutorScoreUpByDistance_C);
            }
        }

        //ÇöÀç Á¡¼ö ÃÊ±âÈ­
        [ContextMenu("Á¡¼ö ÃÊ±âÈ­")]
        public void ResetScore()
        {
            score = 0;
            ui.OnScoreChanged();
        }

        //Á¡¼öÈ¹µæ (µ¿Àü ¸ÔÀ½)
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