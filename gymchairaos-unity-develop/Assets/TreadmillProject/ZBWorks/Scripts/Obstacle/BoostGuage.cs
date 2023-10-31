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
            None, Charging, Decreasing, Boost, BoostBreak
        }

        public State NowState { get => nowState; }
        public bool Boosting { get; private set; }

        [SerializeField] private PlayerInputManager2 playerInput;
        [SerializeField] private ObjectsScrolling objectsScrolling;
        [SerializeField] private UIGauge uiGuage;
        [SerializeField] private UnityEvent uEvent_BoostStart;
        [SerializeField] private UnityEvent uEvent_BoostEnd;
        [SerializeField] private float nowGuage;
        [SerializeField] private float boostSpeed;
        [SerializeField] private float boostTime;
        [SerializeField] private float decreaseTime;
        [SerializeField] private float guageIncreaseByTime;
        [SerializeField] private float guageIncreaseBySpeed;
        [Space]
        [SerializeField] private State nowState;
        private float maxGuage;
        private float decreaseGuageValue;
        private bool decreaseSignal;
        private float boostDuration;

        WaitForSeconds wfs_boostTime;
        WaitForSeconds wfs_boostBreak = new WaitForSeconds(1.5f);
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
                    nowGuage += Time.deltaTime * guageIncreaseByTime;
                    nowGuage += Managers.Token.CurSpeedMeterPerSec * guageIncreaseBySpeed;
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
                    Managers.Sound.PlayEffect("sfx_obstacleBoostStart");
                    yield return wfs_boostTime;

                    nowState = State.BoostBreak;
                    Managers.Sound.PlayEffect("sfx_obstacleBoostBreak");
                    uEvent_BoostEnd.Invoke();
                    yield return wfs_boostBreak;

                    Boosting = false;
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
            boostDuration = 1.5f;
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
                objectsScrolling.ScrollSpeedChangeLinear(playerInput.FocusPower, boostDuration);
            });
        }
    }
}