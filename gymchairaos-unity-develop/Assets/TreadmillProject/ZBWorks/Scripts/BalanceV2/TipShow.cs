using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ZB.Balance2
{
    public class TipShow : MonoBehaviour
    {
        [SerializeField] BalacneTutorialGuide.GuideSpriteSets[] guideSets;
        [SerializeField] UiShadow2 shadow;
        [SerializeField] Transform tf_left;
        [SerializeField] Transform tf_right;
        [SerializeField] Transform tf_tmp;
        [SerializeField] Image img_left;
        [SerializeField] Image img_right;
        [SerializeField] TextMeshProUGUI tmp;

        bool appearing;

        public void OnBtnClick()
        {
            Active();
        }

        private void Active()
        {
            StartCoroutine(GuideAppearCycle());
        }

        IEnumerator GuideAppearCycle()
        {
            if (!appearing)
            {
                appearing = true;

                WaitForSecondsRealtime wfs = new WaitForSecondsRealtime(3.5f);
                WaitForSecondsRealtime wfs2 = new WaitForSecondsRealtime(0.5f);

                Time.timeScale = 0;

                shadow.SetActive(true);
                shadow.SetColor(Color.clear);
                shadow.SetAlpha(0.75f, true, 0.5f);

                tf_left.gameObject.SetActive(true);
                tf_right.gameObject.SetActive(true);
                tf_tmp.gameObject.SetActive(true);

                tf_left.localScale = Vector2.zero;
                tf_right.localScale = Vector2.zero;
                tf_tmp.localScale = Vector2.zero;

                yield return wfs2;

                for (int i = 0; i < guideSets.Length; i++)
                {
                    img_left.sprite = guideSets[i].leftSprite;
                    img_right.sprite = guideSets[i].rightSprite;
                    tmp.text = guideSets[i].text;

                    tf_left.DOScale(Vector2.one, 0.5f).SetEase(Ease.InOutQuart).SetUpdate(true);
                    tf_right.DOScale(Vector2.one, 0.5f).SetEase(Ease.InOutQuart).SetUpdate(true);
                    tf_tmp.DOScale(Vector2.one, 0.5f).SetEase(Ease.InOutQuart).SetUpdate(true);
                    yield return wfs;
                    tf_left.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InOutQuart).SetUpdate(true);
                    tf_right.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InOutQuart).SetUpdate(true);
                    tf_tmp.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InOutQuart).SetUpdate(true);
                    yield return wfs2;
                }

                shadow.SetAlpha(0, true, 0.5f, () =>
                {
                    shadow.SetActive(false);
                    Time.timeScale = 1;
                    appearing = false;
                });
            }
        }
    }
}