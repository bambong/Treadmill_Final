using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;
using DG.Tweening;

namespace ZB
{
    public class PlayerInputManager2 : MonoBehaviour
    {
        [SerializeField] Transform playerBody;

        [SerializeField] bool leftReceived;
        [SerializeField] bool rightReceived;
        [SerializeField] float leftPos;
        [SerializeField] float rightPos;
        [SerializeField] float sideMoveDuration;

        [SerializeField] float leftRpm;
        [SerializeField] float rightRpm;
        [SerializeField] Vector3 resetPos;

        [SerializeField] bool checking;

        public void ResetState()
        {
            playerBody.transform.DOMoveX(-0.6f, 1).SetEase(Ease.OutQuart).SetDelay(1.5f);
            leftReceived = false;
            rightReceived = false;
        }
        public void AddLeftToken()
        {
            TokenInputManager.Instance.Save_left_rpm += 50;
        }
        public void AddRightToken()
        {
            TokenInputManager.Instance.Save_right_rpm += 50;
        }
        public void CheckActive(bool active)
        {
            checking = active;
        }

        public IEnumerator DecreaseToken()
        {
            while (gameObject != null)
            {
                TokenInputManager.Instance.Save_left_rpm = Mathf.Max(0, TokenInputManager.Instance.Save_left_rpm - 1);
                TokenInputManager.Instance.Save_right_rpm = Mathf.Max(0, TokenInputManager.Instance.Save_right_rpm - 1);
                yield return new WaitForSeconds(0.2f);
            }
        }

        void Awake()
        {
            resetPos = playerBody.position;

#if UNITY_EDITOR
            TokenInputManager.Instance.AddLeftTokenEvent(AddLeftToken);
            TokenInputManager.Instance.AddRightTokenEvent(AddRightToken);
            StartCoroutine(DecreaseToken());
#endif
            TokenInputManager.Instance.ReceivedEvent += SideMove;
        }
        private void Update()
        {
#if UNITY_EDITOR
            SideMove();
#endif
        }

        void SideMove()
        {
            if (checking)
            {
                leftRpm = TokenInputManager.Instance.Save_left_rpm;
                rightRpm = TokenInputManager.Instance.Save_right_rpm;
                if (leftRpm > 800 && leftRpm > rightRpm && !leftReceived)
                {
                    Debug.Log("LEFTMOVE");
                    playerBody.DOKill();
                    playerBody.DOMoveX(leftPos, sideMoveDuration).SetEase(Ease.InQuart);
                    playerBody.DORotate(new Vector3(0, -15, 0), 0.75f).SetEase(Ease.OutQuart);
                    playerBody.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InOutBack).SetDelay(0.75f);

                    leftReceived = true;
                    rightReceived = false;
                }
                if (rightRpm > 800 && rightRpm > leftRpm && !rightReceived)
                {
                    Debug.Log("RightMove");
                    playerBody.DOKill();
                    playerBody.DOMoveX(rightPos, sideMoveDuration).SetEase(Ease.InQuart);
                    playerBody.DORotate(new Vector3(0, 15, 0), 0.75f).SetEase(Ease.OutQuart);
                    playerBody.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InOutBack).SetDelay(0.75f);

                    rightReceived = true;
                    leftReceived = false;
                }
            }
        }

        public void MoveBack_OnDie()
        {
            playerBody.DOMoveZ(-8, 1).SetEase(Ease.OutQuart);
        }
        public void MoveFront_OnRebirth()
        {
            playerBody.transform.position = new Vector3(resetPos.x , resetPos.y, -8);
            playerBody.DOMove(resetPos, 1.6f).SetEase(Ease.OutQuart);
        }
    }
}