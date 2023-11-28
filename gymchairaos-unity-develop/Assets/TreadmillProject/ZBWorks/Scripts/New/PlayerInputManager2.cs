using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;

namespace ZB
{
    public class PlayerInputManager2 : MonoBehaviour
    {
        public float FocusPower { get => linearDeceleration.value; }

        [SerializeField] ObjectsScrolling objectScroll;
        [SerializeField] BoostGuage boostGuage;
        [SerializeField] PlayerHP hp;
        [SerializeField] Transform tf;

        [SerializeField] bool leftReceived;
        [SerializeField] bool rightReceived;

        [SerializeField] float leftRpm;
        [SerializeField] float rightRpm;
        [SerializeField] Vector3 resetPos;

        [SerializeField] bool checking;
        [SerializeField] List<ParticleSystem> wheelEffects;

        [Space]
        [Header("좌우이동관련")]
        [SerializeField] float minInterval; //좌우 이동이 일어나는 최소간격
        [SerializeField] float intervalMultiple; 
        [SerializeField] float moveMultiple;
        [SerializeField] float maxMove;
        [SerializeField] float move;
        float intervalAbs { get => Mathf.Abs(rightRpm - leftRpm); }

        [Space]
        [SerializeField] float outPos_left;
        [SerializeField] float outPos_right;

        [Space]
        [Header("전후이동관련")]
        [SerializeField] float minPower;
        [SerializeField] float maxPower;
        [SerializeField] float decel_multiple;
        [SerializeField] float power;

        [Space]
        [Header("회전관련")]
        [SerializeField] float rotMultiple;
        float currentRotTarget;
        float processMove;
        [SerializeField] float processTime;
        float lastProcessTime;

        [Space]
        [Header("이펙트")]
        [SerializeField] ParticleSystem par_slowAlarm;
        [SerializeField] float appearSpeed;

        [Space]
        [SerializeField] float minSpeedMinusHp;
        [SerializeField] float time_minSpeedMinusHp;
        WaitForSeconds wfs_minSpeedMinusHp;
        WaitForSeconds wfs_plusHp;
        bool minSpeedMinusHpCounting;

        [Space(30)]
        [Header("임시인풋값변경")]
        [SerializeField] TMP_InputField ver_minPower;
        [SerializeField] TMP_InputField ver_maxPower;
        [SerializeField] TMP_InputField ver_Power;
        [Space]
        [SerializeField] TMP_InputField hor_minInterval;
        [SerializeField] TMP_InputField hor_moveMultiple;
        [SerializeField] TMP_InputField hor_intervalMultiple;
        [SerializeField] TMP_InputField processTimeField;

        private LinearDeceleration linearDeceleration;

        TokenInputManager token { get => Managers.Token; }

        public void ResetState()
        {
            tf.rotation = Quaternion.Euler(0, 0, 0);

            tf.DOKill();
            tf.DOMoveX(-0.6f, 1).SetEase(Ease.OutQuart).SetDelay(1.5f);
            leftReceived = true;
            rightReceived = false;

            Managers.Token.Save_left_speed = 0;
            Managers.Token.Save_right_speed = 0;
            currentRotTarget = 0;
            move = 0;
        }
        public void AddLeftToken()
        {
            Managers.Token.Save_left_speed += 500;
        }
        public void AddRightToken()
        {
            Managers.Token.Save_right_speed += 500;
        }
        public void CheckActive(bool active)
        {
            checking = active;
            if (!active)
            {
                if (minSpeedMinusHpCycle_C != null)
                    StopCoroutine(minSpeedMinusHpCycle_C);
                minSpeedMinusHpCounting = false;
            }
        }
        public void EnableWheelEffect(bool acitve) 
        {
            for(int i = 0; i < wheelEffects.Count; ++i) 
            {
                if (acitve) 
                {
                    wheelEffects[i].Play();
                }
                else 
                {
                    wheelEffects[i].Stop();
                }
            }
        }

        public IEnumerator DecreaseToken()
        {
            while (gameObject != null)
            {
                Managers.Token.Save_left_speed = Mathf.Max(0, Managers.Token.Save_left_speed - 1);
                Managers.Token.Save_right_speed = Mathf.Max(0, Managers.Token.Save_right_speed - 1);
                yield return new WaitForSeconds(0.2f);
            }
        }

        void Start()
        {
            linearDeceleration = new LinearDeceleration(minPower, maxPower, decel_multiple);
            wfs_minSpeedMinusHp = new WaitForSeconds(time_minSpeedMinusHp);
            wfs_plusHp = new WaitForSeconds(5);
            resetPos = tf.position;
            TestInputField();

//#if UNITY_EDITOR
//            Managers.Token.AddLeftTokenEvent(AddLeftToken);
//            Managers.Token.AddRightTokenEvent(AddRightToken);
//            StartCoroutine(DecreaseToken());
//#endif
            Managers.Token.ReceivedEvent += SideMove;
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A)) leftRpm += 250;
            if (Input.GetKeyDown(KeyCode.D)) rightRpm += 250;
            if (Input.GetKeyDown(KeyCode.S)) linearDeceleration.ValueUpdate(linearDeceleration.value + 1);
            if (Input.GetKeyDown(KeyCode.R)) { token.Save_left_speed = 0; token.Save_right_speed = 0; linearDeceleration.ValueReset(); }
            move = (rightRpm - leftRpm) * moveMultiple;
#endif
#if !UNITY_EDITOR
            linearDeceleration.ValueUpdate((float)Managers.Token.CurSpeedMeterPerSec * power);
#endif
            //이동
            if (tf.position.x <= outPos_left && move < 0)
            {
                tf.position = new Vector3(outPos_left, tf.position.y, tf.position.z);
            }
            else if (tf.position.x >= outPos_right && move > 0)
            {
                tf.position = new Vector3(outPos_right, tf.position.y, tf.position.z);
            }
            else
            {
                tf.position += new Vector3(move * Time.deltaTime, 0, 0);
            }

            //이동 정도에 따른 차체 회전
            if (currentRotTarget != move * rotMultiple)
            {
                currentRotTarget = move * rotMultiple;
                tf.DOKill();
                tf.DORotate(new Vector3(tf.eulerAngles.x, move * rotMultiple, tf.eulerAngles.z), 0.5f);
            }

            //현재 속도에 따른 스크롤 속도 조정
            linearDeceleration.Update();
            if (!(boostGuage.NowState == BoostGuage.State.Boost || boostGuage.NowState == BoostGuage.State.BoostBreak))
            {
                objectScroll.ScrollSpeedChange(linearDeceleration.value);
            }

            if (checking)
            {
                if (Managers.Token.CurSpeedMeterPerSec < appearSpeed &&
                    !par_slowAlarm.isPlaying)
                    par_slowAlarm.Play();

                else if (Managers.Token.CurSpeedMeterPerSec >= appearSpeed &&
                    par_slowAlarm.isPlaying)
                    par_slowAlarm.Stop();

                //최소속도 못넘김
                //체력감소 카운트 시작
                if (linearDeceleration.value < minSpeedMinusHp &&
                    !minSpeedMinusHpCounting)
                {
                    if (minSpeedMinusHpCycle_C != null)
                        StopCoroutine(minSpeedMinusHpCycle_C);
                    minSpeedMinusHpCycle_C = minSpeedMinusHpCycle();
                    StartCoroutine(minSpeedMinusHpCycle_C);

                    if (plusHpCycle != null)
                        StopCoroutine(plusHpCycle);
                }

                //최소속도 못넘기다가, 최소속도 넘김
                else if (linearDeceleration.value >= minSpeedMinusHp &&
                    minSpeedMinusHpCounting)
                {
                    if (minSpeedMinusHpCycle_C != null)
                        StopCoroutine(minSpeedMinusHpCycle_C);
                    minSpeedMinusHpCounting = false;

                    if (plusHpCycle != null)
                        StopCoroutine(plusHpCycle);
                    plusHpCycle = PlusHpCycle();
                    StartCoroutine(plusHpCycle);
                }
            }
        }

        void SideMove()
        {
            if (checking)
            {
                if (Mathf.Abs(leftRpm - token.Save_left_speed) > processTime || Mathf.Abs(rightRpm - token.Save_right_speed) > processTime)
                {
                    return;
                }

                if (token.Save_left_speed < -1990 || token.Save_right_speed < -1990) 
                {
                    return;
                }

                leftRpm = token.Save_left_speed;
                rightRpm = token.Save_right_speed;

                if (intervalAbs < minInterval)
                {
                    move = 0;
                    return;
                }
                //프레임마다 이동할 정도 구함

                move = -(rightRpm - leftRpm) * moveMultiple;
                move = Mathf.Clamp(move, -maxMove, maxMove);
            }
        }

        public void MoveBack_OnDie()
        {
            tf.DOMoveZ(-8, 1).SetEase(Ease.OutQuart);
        }
        public void MoveFront_OnRebirth()
        {
            tf.position = resetPos + new Vector3(0, 0, -2.5f);
            tf.DOMove(resetPos, 1.6f).SetEase(Ease.OutQuart);
        }

        public void TestInputField()
        {
            //float result = 0;
            //if (float.TryParse(ver_minPower.text, out result))
            //    minPower = result;
            //if (float.TryParse(ver_maxPower.text, out result))
            //    maxPower = result;
            //if (float.TryParse(ver_Power.text, out result))
            //    power = result;

            //if (float.TryParse(hor_minInterval.text, out result))
            //    minInterval = result;
            //if (float.TryParse(hor_moveMultiple.text, out result))
            //    moveMultiple = result;
            //if (float.TryParse(hor_intervalMultiple.text, out result))
            //    intervalMultiple = result;
            //if (float.TryParse(processTimeField.text, out result))
            //    processTime = result;
        }

        IEnumerator minSpeedMinusHpCycle_C;
        IEnumerator minSpeedMinusHpCycle()
        {
            minSpeedMinusHpCounting = true;
            yield return wfs_minSpeedMinusHp;
            hp.MinusHP(1);
            minSpeedMinusHpCounting = false;
        }

        IEnumerator plusHpCycle;
        IEnumerator PlusHpCycle()
        {
            while (true)
            {
                yield return wfs_plusHp;
                hp.PlusHP(1);
            }
        }
    }
}