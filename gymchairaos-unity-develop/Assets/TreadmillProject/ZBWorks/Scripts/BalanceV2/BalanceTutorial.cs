using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using bambong;

namespace ZB.Balance2
{
    public class BalanceTutorial : MonoBehaviour
    {
        public enum State { none, tutorialReady, tutorialDoing, celebrate }

        [SerializeField] UiShadow2 uiShadow2;

        [SerializeField] GameObject obj_startAsk;
        [SerializeField] Transform[] tfs_startAsk;

        [SerializeField] Image leftImg;
        [SerializeField] Image rightImg;
        [SerializeField] TextMeshProUGUI guideText;
        [SerializeField] RotateObjInput rotateObjInput;
        [SerializeField] TutorialPhase[] tutorialPhases;
        [SerializeField] StageManager stageManager;

        [SerializeField] State nowState;

        [SerializeField] int index;
        TutorialPhase nowPhase { get => tutorialPhases[index]; }
        RectTransform rtf_guideText;

        bool iconDisappearing;

        public void StartAsk(bool askActive)
        {
            //ù��° ���������϶��� askActive�� true�Ͽ� Ʃ�丮�����డ��
            if (askActive)
            {
                uiShadow2.SetActive(true);
                uiShadow2.SetAlpha(0, false);
                uiShadow2.SetAlpha(0.8f, true, 0.5f);

                obj_startAsk.SetActive(true);
                for (int i = 0; i < tfs_startAsk.Length; i++)
                {
                    tfs_startAsk[i].localScale = Vector3.zero;
                    tfs_startAsk[i].DOScale(Vector3.one, 0.65f);
                }

                transform.DOScale(Vector3.one, 2).OnComplete(() =>
                {
                    TutorialActive(true);
                });
            }
        }

        //Ʃ�丮���� �����Ͻðڽ��ϱ�? ��ư ��ȣ�ۿ�
        public void TutorialActive(bool active)
        {
            if (nowState == State.none)
            {
                if (active)
                {
                    nowState = State.tutorialReady;
                    transform.DOKill();

                    for (int i = 0; i < tfs_startAsk.Length; i++)
                    {
                        tfs_startAsk[i].DOKill();
                        tfs_startAsk[i].DOScale(Vector3.zero, 0.65f).SetEase(Ease.OutQuart);
                    }
                    transform.DOScale(Vector3.one, 0.65f).OnComplete(() =>
                    {
                        nowState = State.tutorialDoing;
                        index = 0;
                        tutorialPhases[index].Appear(rotateObjInput, leftImg, rightImg, guideText, uiShadow2);
                    });
                }
                else
                {
                    uiShadow2.SetAlpha(0, true, 0.5f);
                    SceneMove_NoTutorial_1Stage();
                }
            }
        }

        public void SceneMove_Tutorial_1Stage()
        {
            Managers.Scene.LoadScene(E_SceneName.Balance_GameScene_1_WithTutorial);
        }

        private void Awake()
        {
            for (int i = 0; i < tutorialPhases.Length; i++)
            {
                tutorialPhases[i].Init(rotateObjInput);
            }
            uiShadow2.SetActive(false);
            leftImg.gameObject.SetActive(false);
            rightImg.gameObject.SetActive(false);
            guideText.gameObject.SetActive(false);

            guideText.TryGetComponent(out rtf_guideText);
    }
        private void Start()
        {
            StartAsk(true);
        }
        private void Update()
        {
            if (nowState == State.tutorialDoing)
            {
                //����Ʃ�丮��� �Ѿ ���� ����
                if (nowPhase.phaseEndCheck(nowPhase.condition) && !iconDisappearing) 
                {
                    iconDisappearing = true;
                    nowPhase.Disappear(rotateObjInput, leftImg, rightImg, guideText, () =>
                    {
                        iconDisappearing = false;

                        index++;

                        //���� Ʃ�丮��� �Ѿ��
                        if (index < tutorialPhases.Length)
                        {
                            tutorialPhases[index].Appear(rotateObjInput, leftImg, rightImg, guideText, uiShadow2);
                        }

                        //Ʃ�丮�� ���� ���ϸ޽���
                        else
                        {
                            nowState = State.celebrate;

                            guideText.fontSize = 100;
                            guideText.text = $"<color=#12D19C>�����մϴ�!</color>\n <color=#018BBF>Ʃ�丮���� �Ϸ��߽��ϴ�!</color>";

                            //Ȯ��
                            rtf_guideText.gameObject.SetActive(true);
                            rtf_guideText.anchoredPosition = Vector2.zero;
                            rtf_guideText.transform.localScale = Vector3.zero;
                            rtf_guideText.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.InOutQuart);

                            //��� �� ���
                            rtf_guideText.transform.DOScale(Vector3.zero, 0.75f).SetEase(Ease.InOutQuart).SetDelay(3).OnComplete(() =>
                            {
                                guideText.text = $"���� ���������� ������ �����?";

                                //Ȯ��
                                rtf_guideText.gameObject.SetActive(true);
                                rtf_guideText.anchoredPosition = Vector2.zero;
                                rtf_guideText.transform.localScale = Vector3.zero;
                                rtf_guideText.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.InOutQuart);

                                //��� �� ���
                                rtf_guideText.transform.DOScale(Vector3.zero, 0.75f).SetEase(Ease.InOutQuart).SetDelay(3).OnComplete(() =>
                                {
                                    //TODO ���⼭ ������ ����
                                    nowState = State.none;
                                    SceneMove_NoTutorial_1Stage();
                                });
                            });
                        }
                    });
                }
            }
        }
        private void SceneMove_NoTutorial_1Stage()
        {
            Managers.Scene.LoadScene(E_SceneName.Balance_GameScene_1);
        }

        [System.Serializable]
        public class TutorialPhase
        {
            public delegate bool PhaseEndCheck(RotateObjInput.RotDir rotDir);

            public PhaseEndCheck phaseEndCheck;
            public RotateObjInput.RotDir condition;
            public Sprite leftSprite;
            public Sprite rightSprite;
            public string guideText;

            float iconSize_emphasis = 2;
            float iconSize_normal = 1.75f;
            Vector2 iconPos_emphasis = new Vector2(300,0);
            Vector2 iconPos_normal = new Vector2(682,-85);
            Vector2 textPosition = new Vector2(0, -415);

            float duration_emphasisAppear = 0.75f;
            float duration_gotoNormal = 0.75f;

            public void Init(RotateObjInput rotateObjInput)
            {
                phaseEndCheck = rotateObjInput.RotDirCheck_Front;
            }
            public void Appear(RotateObjInput input, Image leftImg, Image rightImg, TextMeshProUGUI tmp, 
                UiShadow2 shadow)
            {
                RectTransform rtf_leftImg;
                RectTransform rtf_rightImg;
                RectTransform rtf_tmp;

                leftImg.TryGetComponent(out rtf_leftImg);
                rightImg.TryGetComponent(out rtf_rightImg);
                tmp.TryGetComponent(out rtf_tmp);

                shadow.SetActive(true);
                shadow.SetAlpha(0.8f, true, 0.5f);
                leftImg.gameObject.SetActive(true);
                rightImg.gameObject.SetActive(true);
                tmp.gameObject.SetActive(true);

                leftImg.sprite = leftSprite;
                rightImg.sprite = rightSprite;
                tmp.fontSize = 40;
                tmp.text = guideText;

                leftImg.transform.localScale = Vector3.zero;
                rightImg.transform.localScale = Vector3.zero;
                tmp.transform.localScale = Vector3.zero;

                rtf_leftImg.anchoredPosition = new Vector2(-iconPos_emphasis.x, iconPos_emphasis.y);
                rtf_rightImg.anchoredPosition = iconPos_emphasis;
                rtf_tmp.anchoredPosition = textPosition;

                leftImg.transform.DOScale(Vector3.one * iconSize_emphasis, duration_emphasisAppear).SetEase(Ease.InOutQuart);
                rightImg.transform.DOScale(Vector3.one * iconSize_emphasis, duration_emphasisAppear).SetEase(Ease.InOutQuart);
                tmp.transform.DOScale(Vector3.one * iconSize_emphasis, duration_emphasisAppear).SetEase(Ease.InOutQuart).OnComplete(()=>
                {
                    leftImg.transform.DOScale(Vector3.one * iconSize_normal, duration_gotoNormal).SetEase(Ease.InOutQuart).SetDelay(0.75f);
                    rightImg.transform.DOScale(Vector3.one * iconSize_normal, duration_gotoNormal).SetEase(Ease.InOutQuart).SetDelay(0.75f);

                    rtf_leftImg.DOAnchorPos(new Vector2(-iconPos_normal.x, iconPos_normal.y), duration_gotoNormal).SetEase(Ease.InOutQuart).SetDelay(0.75f);
                    rtf_rightImg.DOAnchorPos(iconPos_normal, duration_gotoNormal).SetEase(Ease.InOutQuart).SetDelay(0.75f).OnComplete(()=>
                    {
                        shadow.SetAlpha(0, true, 0.5f, ()=>shadow.SetActive(false));
                        input.Active(true);
                    });
                });
            }
            public void Disappear(RotateObjInput input, Image leftImg, Image rightImg, TextMeshProUGUI tmp, UnityAction OnDisappearEnd)
            {
                leftImg.transform.DOKill();
                rightImg.transform.DOKill();
                tmp.transform.DOKill();
                leftImg.transform.DOScale(Vector3.zero, 0.75f).SetEase(Ease.InOutQuart);
                rightImg.transform.DOScale(Vector3.zero, 0.75f).SetEase(Ease.InOutQuart);
                tmp.transform.DOScale(Vector3.zero, 0.75f).SetEase(Ease.InOutQuart).OnComplete(()=>
                {
                    UnityEvent unityEvent = new UnityEvent();
                    unityEvent.AddListener(OnDisappearEnd);
                    unityEvent.Invoke();
                    input.Active(false);
                });
            }
        }
    }
}