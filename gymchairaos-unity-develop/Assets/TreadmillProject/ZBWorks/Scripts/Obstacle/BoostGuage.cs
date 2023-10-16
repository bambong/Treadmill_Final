using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB
{
    public class BoostGuage : MonoBehaviour
    {
        public enum State
        {
            None, Charging, Decreasing, Boost
        }

        public State NowState { get => nowState; }
        public bool Boosting { get; private set; }

        [SerializeField] private ObjectsScrolling objectsScrolling;
        [SerializeField] private UIGauge uiGuage;
        [SerializeField] private UnityEvent uEvent_BoostStart;
        [SerializeField] private UnityEvent uEvent_BoostEnd;
        [SerializeField] private float nowGuage;
        [SerializeField] private float boostSpeed;
        [SerializeField] private float boostTime;
        [SerializeField] private float decreaseTime;
        [Space]
        [SerializeField] private State nowState;
        private float maxGuage;
        private float decreaseGuageValue;
        private float guageIncreasePerSecond;
        private bool decreaseSignal;
        private float normalSpeed;
        private float boostDuration;

        WaitForSeconds wfs_boostTime;
        WaitForSeconds wfs_boostTime_BackDelay = new WaitForSeconds(0.75f);
        WaitForSeconds wfs_decreaseTime;

        [ContextMenu("START")]
        public void ChargeStart()
        {
            if (nowState != State.None)
                ChargeStop();
            ChargeCycle_C = ChargeCycle();
            StartCoroutine(ChargeCycle_C);
        }
        [ContextMenu("STOP")]
        public void ChargeStop()
        {
            nowState = State.None;
            StopCoroutine(ChargeCycle_C);
            nowGuage = 0;
            decreaseSignal = false;
        }
        [ContextMenu("DECREASE")]
        public void ChargeDecrease()
        {
            if (nowState != State.Decreasing && nowState != State.Boost)
                decreaseSignal = true;
        }

        IEnumerator ChargeCycle_C;
        IEnumerator ChargeCycle()
        {
            nowState = State.Charging;
            while (true)
            {
                //Charging일때 : 게이지 증가
                if (nowState == State.Charging)
                {
                    nowGuage += Time.deltaTime * guageIncreasePerSecond;
                    uiGuage.ChangeRatio(nowGuage / maxGuage);
                }

                //게이지감소
                if (decreaseSignal)
                {
                    nowState = State.Decreasing;
                    decreaseSignal = false;
                    nowGuage = nowGuage - decreaseGuageValue < 0 ? 0 : nowGuage - decreaseGuageValue;
                    uiGuage.ChangeRatioWithTweening(nowGuage / maxGuage);
                    yield return wfs_decreaseTime;
                    nowState = State.Charging;
                }

                //다 찼을경우 이벤트실행
                if (nowGuage >= maxGuage)
                {
                    nowState = State.Boost;
                    nowGuage = maxGuage;
                    Boosting = true;
                    uEvent_BoostStart.Invoke();
                    yield return wfs_boostTime;
                    Boosting = false;
                    uEvent_BoostEnd.Invoke();
                    nowState = State.Charging;
                    nowGuage = 0;
                    uiGuage.ChangeRatioWithTweening(nowGuage / maxGuage);
                    yield return wfs_boostTime_BackDelay;
                }
                yield return null;
            }
        }
        private void Awake()
        {
            uiGuage.ChangeRatio(0);
            maxGuage = 100;
            decreaseGuageValue = 20;
            guageIncreasePerSecond = 6.25f;
            normalSpeed = objectsScrolling.ScrollSpeed;
            boostDuration = 1;
            decreaseSignal = false;
            wfs_decreaseTime = new WaitForSeconds(decreaseTime);
            wfs_boostTime = new WaitForSeconds(boostTime);

            uEvent_BoostStart.AddListener(() =>
            {
                //속도변화
                objectsScrolling.ScrollSpeedChangeLinear(boostSpeed, boostDuration);
            });

            uEvent_BoostEnd.AddListener(() =>
            {
                //속도변화
                objectsScrolling.ScrollSpeedChangeLinear(normalSpeed, boostDuration);
            });
        }
    }
}