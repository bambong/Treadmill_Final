using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;
using DG.Tweening;

namespace ZB
{
    public class PlayerInputManager2 : MonoBehaviour
    {
        [SerializeField] Transform tf;

        [SerializeField] bool leftReceived;
        [SerializeField] bool rightReceived;

        [SerializeField] float leftRpm;
        [SerializeField] float rightRpm;
        [SerializeField] Vector3 resetPos;

        [SerializeField] bool checking;
        [SerializeField] List<ParticleSystem> wheelEffects;

        [Space]
        [Header("이동관련")]
        [SerializeField] float minInterval; //좌우 이동이 일어나는 최소간격
        [SerializeField] float moveMultiple;
        [SerializeField] float maxMove;
        [SerializeField] float move;
        float intervalAbs { get => Mathf.Abs(rightRpm - leftRpm); }

        [Space]
        [SerializeField] float outPos_left;
        [SerializeField] float outPos_right;

        [Space]
        [Header("회전관련")]
        [SerializeField] float rotMultiple;
        float currentRotTarget;

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
            Managers.Token.Save_left_speed += 50;
        }
        public void AddRightToken()
        {
            Managers.Token.Save_right_speed += 50;
        }
        public void CheckActive(bool active)
        {
            checking = active;
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
            resetPos = tf.position;

#if UNITY_EDITOR
            Managers.Token.AddLeftTokenEvent(AddLeftToken);
            Managers.Token.AddRightTokenEvent(AddRightToken);
            StartCoroutine(DecreaseToken());
#endif
            Managers.Token.ReceivedEvent += SideMove;
        }
        private void Update()
        {
#if UNITY_EDITOR
            SideMove();
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

            if (Input.GetKeyDown(KeyCode.R))
            { token.Save_left_speed = 0; token.Save_right_speed = 0; }
        }

        void SideMove()
        {
            if (checking)
            {

                leftRpm = token.Save_left_speed;
                rightRpm = token.Save_right_speed;

                //프레임마다 이동할 정도 구함
                if (intervalAbs < minInterval)
                    move = 0;
                else
                {
                    move = (rightRpm - leftRpm) * moveMultiple;

                    if (rightRpm > leftRpm &&
                        move > maxMove)
                        move = maxMove;
                    else if (move <= -maxMove)
                        move = -maxMove;
                }

                ////이동
                //if (tf.position.x <= outPos_left && move < 0)
                //{
                //    tf.position = new Vector3(outPos_left, tf.position.y, tf.position.z);
                //}
                //else if (tf.position.x >= outPos_right && move > 0)
                //{
                //    tf.position = new Vector3(outPos_right, tf.position.y, tf.position.z);
                //}
                //else
                //{
                //    tf.position += new Vector3(move * Time.deltaTime, 0, 0);
                //}


                ////이동 정도에 따른 차체 회전
                //if (currentRotTarget != move * rotMultiple)
                //{
                //    currentRotTarget = move * rotMultiple;
                //    tf.DOKill();
                //    tf.DORotate(new Vector3(tf.eulerAngles.x, move * rotMultiple, tf.eulerAngles.z), 0.5f);
                //}
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
    }
}