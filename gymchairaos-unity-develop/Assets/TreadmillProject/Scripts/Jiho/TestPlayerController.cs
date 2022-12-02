using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jiho
{
    public class TestPlayerController : MonoBehaviour
    {
        [SerializeField]
        private Animator playerAnimator;

        [SerializeField]
        private float moveSpeed = 3f;
        [SerializeField]
        private float rotateSpeed = 10f;

        [SerializeField]
        private float decreaseGauageAmount = 0.1f;
        [SerializeField]
        private float increaseGauageAmount = 1f;
        [SerializeField]
        private float speedRatio = 1f;
        
        [SerializeField]
        private Image speedGauageImage;

        [SerializeField]
        private float maximumGauage = 100f;

        [SerializeField]
        private Vector3 fixedDir = Vector3.zero;

        [SerializeField]
        private float rotateProcessTime = 0.2f;

        // 밸런스가 안 맞을때 얼만큼 각도가 돌아갈지 
        [SerializeField]
        private int rotateAmount = 3;

        [SerializeField]
        private float animSpeed = 90f;
        private float curGauage = 0;

        private bool isMoving = false;

        // left right 의 차를 저장할 변수
        private int tokenDiff;

        private float curRotateProcessTime;
        private readonly float CLOSE_ZERO_GAUAGE = 1f;

        private float desireRotateAngle;
        // left right 회전 수의 차를 얼마만큼 허용해줄지 
        private readonly int BUFFER_ROTATE_DIFF = 0;
        // 최대 회전 각도 
        private readonly float ROTATE_LIMIT = 90f;
        // 몇 초동안 입력이 없으면 멈출것 인지
        private readonly float INPUT_ANIMATE_STOP_TIME = 1f;

        private readonly string MOVE_CHECK_BOOL = "IsRun";
        private readonly string NO_INPUT_CHECK_BOOL = "IsNoInput";


        // Start is called before the first frame update
        private void Awake()
        {
            //SpeedGauageFillUpdate();

            TokenInputManager.Instance.AddRightTokenEvent(IncreaseGauage);
            TokenInputManager.Instance.AddRightTokenEvent(() =>
            {
                if (!isMoving) 
                {
                    return;
                }
                tokenDiff = (int)(TokenInputManager.Instance.Save_left_rpm - TokenInputManager.Instance.Save_right_rpm); 
            });

            TokenInputManager.Instance.AddLeftTokenEvent(IncreaseGauage);
            TokenInputManager.Instance.AddLeftTokenEvent(() => 
            {
                if (!isMoving) 
                {
                    return;
                }
                tokenDiff = (int)(TokenInputManager.Instance.Save_left_rpm - TokenInputManager.Instance.Save_right_rpm);
            });
        }
        //private void SpeedGauageFillUpdate()
        //{
        //    speedGauageImage.fillAmount = curGauage / maximumGauage;
        //}
        private void IncreaseGauage()
        {
            if (!isMoving) 
            {
                return;
            }
            curGauage = Mathf.Min(curGauage + increaseGauageAmount, maximumGauage);
        }

        

        private void DecreaseGauage()
        {
            var decGauage = Mathf.Lerp(curGauage, 0, decreaseGauageAmount * Time.deltaTime);
            if (decGauage <= CLOSE_ZERO_GAUAGE)
            {
                curGauage = 0;
            }
            else
            {
                curGauage = decGauage;
            }
            //SpeedGauageFillUpdate();
        }

        void Update()
        {
            //Debug.Log($" Right Term : {TokenInputManager.Instacne.RightTokenTerm}");
            //Debug.Log($" Left Term : {TokenInputManager.Instacne.LeftTokenTerm}");

            DecreaseGauage();
            if(isMoving)
            {
                PlayerMoveUpdate();
                RotateInput();
                RotateMoveUpdate();
                PlayerInputCheckForStop();
            }
            else if(!isMoving)
            {
                return;
            }
        }

        private void RotateMoveUpdate()
        {

            var desireRotate = (Quaternion.Euler(fixedDir) * Quaternion.Euler(new Vector3(0, desireRotateAngle, 0)));

            //if (desireRotateAngle == 0)
            //{
                transform.rotation = desireRotate;
            //    return;
            //}

            transform.rotation = Quaternion.Slerp(transform.rotation, desireRotate, rotateSpeed * Time.deltaTime);

        }
        private void RotateInput()
        {
            curRotateProcessTime += Time.deltaTime;
            //일정 시간마다 입력을 모아서 처리하기위해 프로세스 타임 설정
            if (curRotateProcessTime < rotateProcessTime)
            {
                return;
            }
            curRotateProcessTime = 0;

            // 현재 어느 방향으로 돌아가 있는지
            var rotateDir = Vector3.Dot(Vector3.right, transform.forward) < 0 ? -1 : 1;

            if (tokenDiff <= BUFFER_ROTATE_DIFF && -BUFFER_ROTATE_DIFF <= tokenDiff)
            {

                var curRotateAngle = Quaternion.Angle(Quaternion.Euler(fixedDir), transform.rotation);

                if (curRotateAngle < rotateAmount)
                {
                    desireRotateAngle = 0;
                }
                else
                {
                    desireRotateAngle = curRotateAngle * rotateDir + (rotateAmount * -rotateDir);
                }

                tokenDiff = 0;

            }
            else
            {

                var curRotateAngle = Quaternion.Angle(Quaternion.Euler(fixedDir), transform.rotation);
                desireRotateAngle = Mathf.Clamp(curRotateAngle * rotateDir + (rotateAmount * tokenDiff * 0.01f), -ROTATE_LIMIT, ROTATE_LIMIT);

                tokenDiff = 0;
            }

        }
        private void PlayerMoveUpdate()
        {
            if (curGauage <= 0)
            {
                PlayerAnimateStop();
                return;
            }
            var speed = (curGauage / speedRatio) * moveSpeed * Time.deltaTime;
            PlayerAnimateMove();
            var pos = transform.position;
            pos += transform.forward * speed;
            transform.position = pos;
        }

        private void PlayerInputCheckForStop()
        {
            if (TokenInputManager.Instance.LastEventTime >= INPUT_ANIMATE_STOP_TIME && !playerAnimator.GetBool(NO_INPUT_CHECK_BOOL))
            {
                playerAnimator.SetBool(NO_INPUT_CHECK_BOOL, true);
            }
            else if (TokenInputManager.Instance.LastEventTime < INPUT_ANIMATE_STOP_TIME && playerAnimator.GetBool(NO_INPUT_CHECK_BOOL))
            {
                playerAnimator.SetBool(NO_INPUT_CHECK_BOOL, false);
            }
        }
        private void PlayerAnimateMove()
        {
            playerAnimator.SetBool(MOVE_CHECK_BOOL, true);
        }
        private void PlayerAnimateStop()
        {
            playerAnimator.SetBool(MOVE_CHECK_BOOL, false);
        }

        public void IsMoving(bool _move)
        {
            isMoving = _move;
        }

    }
}


