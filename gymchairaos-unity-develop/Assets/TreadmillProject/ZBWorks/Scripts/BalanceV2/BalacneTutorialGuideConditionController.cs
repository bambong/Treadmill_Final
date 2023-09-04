using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.Balance2
{
    public class BalacneTutorialGuideConditionController : MonoBehaviour
    {
        [SerializeField] BalacneTutorialGuideEndAlarm endAlarm;
        [SerializeField] BalacneTutorialGuide tutorial;
        [SerializeField] RotateObjInput rotateObjInput;
        [SerializeField] RotateObj rotateObj;

        [SerializeField] bool rightSlantTrigger;

        public void OnRightSlantTriggerEnter()
        {
            rightSlantTrigger = true;
        }

        private void TutorialActive(int index, BalacneTutorialGuide.GuideEndCondition condition, UnityAction onEndCycleEnd)
        {
            StartCoroutine(DelayNActive(index, condition, onEndCycleEnd));
        }
        private bool RightSlantTutorialCondition()
        {
            if (rightSlantTrigger)
            {
                rightSlantTrigger = false;
                return true;
            }
            return false;
        }
        private void Start()
        {
            //��
            TutorialActive(0, ()=>rotateObjInput.RotDirCheck_Front(RotateObjInput.RotDir.back), () =>
            {
                //��
                TutorialActive(1, () => rotateObjInput.RotDirCheck_Front(RotateObjInput.RotDir.front), () =>
                {
                    //��
                    TutorialActive(2, () => rotateObjInput.RotDirCheck_Front(RotateObjInput.RotDir.left), () =>
                    {
                        //��
                        TutorialActive(3, () => RightSlantTutorialCondition(), () =>
                        {
                            endAlarm.Alarm();
                        });
                    });
                });
            });
        }

        IEnumerator DelayNActive(int index, BalacneTutorialGuide.GuideEndCondition condition, UnityAction onEndCycleEnd)
        {
            yield return null;

            Time.timeScale = 0;
            tutorial.Active(index, condition,
                () => { Time.timeScale = 1; },
                () => { Time.timeScale = 0; },
                () =>
                {
                    Time.timeScale = 1;

                    //��, �� ȸ���� �ʱ�ȭ
                    rotateObj.SetVelocity(Vector3.zero);

                    UnityEvent unityEvent = new UnityEvent();
                    unityEvent.AddListener(onEndCycleEnd);
                    unityEvent.Invoke();
                });
        }
    }
}