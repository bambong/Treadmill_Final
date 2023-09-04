 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ZB.Balance
{
    public class WindowInteract : MonoBehaviour
    {
        [SerializeField] UiShadow m_uiShadow;
        [SerializeField] CountDown m_countDown;
        [SerializeField] TimeCount m_timeCount;
        [SerializeField] DistanceCount m_distCount;
        [Space]
        [SerializeField] RectTransform m_rtf_body_Pause;
        [SerializeField] RectTransform m_rtf_body_Result;
        [SerializeField] TextMeshProUGUI m_tmp_time;
        [SerializeField] TextMeshProUGUI m_tmp_dist;
        [SerializeField] GameObject m_obj_interactBlock_Pause;
        [SerializeField] GameObject m_obj_interactBlock_Result;
        [SerializeField] Image m_img_interactBlock_PauseSmall;
        [SerializeField] private bool m_pausing = false;

        public void OnBtnClicked_Pause()
        {
            //일시정지 시작
            if (!m_pausing)
            {
                SoundLocator.Instance.PlaySfx("touch");

                m_uiShadow.Active(true);
                m_obj_interactBlock_Pause.SetActive(false);

                m_pausing = true;
                m_rtf_body_Pause.transform.DOKill();
                m_rtf_body_Pause.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);

                PauseBtnBlock(true);

                Time.timeScale = 0;
            }

            //일시정지 해제
            else
            {
                SoundLocator.Instance.PlaySfx("touch");

                m_uiShadow.Active(false);

                m_pausing = false;
                m_rtf_body_Pause.transform.DOKill();
                m_obj_interactBlock_Pause.SetActive(true);

                m_rtf_body_Pause.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InQuart).SetUpdate(true).OnComplete(()=>
                {
                    transform.DOLocalMove(Vector2.zero, 0.5f).SetUpdate(true).OnComplete(() => { Time.timeScale = 1; });
                    PauseBtnBlock(false);
                });
            }
        }

        public void OnBtnClicked_Main()
        {
            SoundLocator.Instance.PlaySfx("touch");

            m_uiShadow.Active(false);

            Time.timeScale = 1;
            Managers.Scene.LoadScene(E_SceneName.SelectGame);
     
            m_obj_interactBlock_Pause.SetActive(true);
            m_obj_interactBlock_Result.SetActive(true);
        }

        public void OnBtnClicked_Retry()
        {
            SoundLocator.Instance.PlaySfx("touch");

            m_uiShadow.Active(false);

            m_obj_interactBlock_Result.SetActive(true);
            m_rtf_body_Result.transform.DOKill();
            m_rtf_body_Result.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InQuart).SetUpdate(true).OnComplete(() =>
            {
                m_obj_interactBlock_Result.SetActive(false);
                transform.DOLocalMove(Vector2.zero, 0.5f).SetUpdate(true).OnComplete(() =>
                {
                    m_countDown.CountStart(3);
                    PauseBtnBlock(false);
                });
            });
        }

        public void WindowAppear_Result()
        {
            m_uiShadow.Active(true);

            m_tmp_time.text = (m_timeCount.m_EarnedObjects * 30).ToString();
            m_tmp_dist.text = ((int)m_distCount.m_NowDistance).ToString();

            m_rtf_body_Result.transform.DOKill();
            m_rtf_body_Result.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);
            m_obj_interactBlock_Result.SetActive(false);

            PauseBtnBlock(true);
        }

        public void PauseBtnBlock(bool active)
        {
            if (active)
            {
                m_img_interactBlock_PauseSmall.gameObject.SetActive(true);
                m_img_interactBlock_PauseSmall.color = Color.clear;
                m_img_interactBlock_PauseSmall.DOKill();
                m_img_interactBlock_PauseSmall.DOColor(Color.white, 0.3f).SetUpdate(true);
            }
            else
            {
                m_img_interactBlock_PauseSmall.DOKill();
                m_img_interactBlock_PauseSmall.DOColor(Color.clear, 0.3f).SetUpdate(true).OnComplete(()=>
                {
                    m_img_interactBlock_PauseSmall.gameObject.SetActive(false);
                });
            }
        }
        private void Awake()
        {
            m_rtf_body_Pause.gameObject.SetActive(true);
            m_rtf_body_Result.gameObject.SetActive(true);
            m_img_interactBlock_PauseSmall.gameObject.SetActive(false);
            m_img_interactBlock_PauseSmall.color = Color.clear;
            m_rtf_body_Pause.transform.localScale = Vector2.zero;
            m_rtf_body_Result.transform.localScale = Vector2.zero;
        }
    }
}