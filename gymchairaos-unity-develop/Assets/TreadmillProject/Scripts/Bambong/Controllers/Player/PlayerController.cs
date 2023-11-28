using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bambong
{

    public class PlayerController : MonoBehaviour
    {
        public float NowSpeed { get => linearDeceleration.value; }

        #region SerializeField
        [SerializeField]
        private Animator playerAnimator;
        
        [SerializeField]
        private float moveSpeed = 1f;
     
        [SerializeField]
        private float speedRatio = 1f;

        [SerializeField]
        private float speedForEditor;

        [SerializeField]
        private Transform arrowTrs;

        #endregion SerializeField
        
        #region NonSerializeField
        private PlayerStateController stateController;
       
        private WheelChairAnimateController animateController;
        private float arrowStartY;
        #endregion NonSerializeField
        
        #region Proprtty
        public PlayerStateController PlayerStateController { get => stateController; }
        public float MoveSpeed { get => moveSpeed; }
        public float SpeedRatio { get => speedRatio; }
        
        #endregion Property
        
        #region ReadOnly
        // 몇 초동안 입력이 없으면 멈출것 인지
        private readonly float INPUT_ANIMATE_STOP_TIME = 2f;
        #endregion ReadOnly

        private LinearDeceleration linearDeceleration;

        private void Awake()
        {
            stateController = new PlayerStateController(this);
            animateController = new WheelChairAnimateController(playerAnimator);
            linearDeceleration = new LinearDeceleration(0, 10, 2);
            arrowStartY = arrowTrs.transform.localPosition.y;
#if UNITY_EDITOR
            Managers.Token.AddLeftTokenEvent(AddLeftToken);
            Managers.Token.AddRightTokenEvent(AddRightToken);
#endif
        }
        public void ArrowAnimStart()
        {
            arrowTrs.DOLocalMoveY(arrowStartY + 0.5f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
            arrowTrs.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        void Update()
        {
            stateController.Update();
            linearDeceleration.Update();
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.S))
            {
                linearDeceleration.ValueUpdate(linearDeceleration.value + 10);
            }
#endif
        }

        public void PlayerMoveUpdate()
        {
            var speed = GetCurSpeed() * Time.deltaTime;
            //var speed = Managers.Token.CurRpm * 0.02f * Time.deltaTime;
            var pos = transform.position;
            pos += transform.forward * speed; 
            transform.position = pos;
            //animateController.SetMoveAnimSpeed(GameSceneManager.Instance.GetGaugeRatio());
            animateController.SetMoveAnimSpeed(NowSpeed / 50);
            GameSceneManager.Instance.AddDistance();
        }
        //public float GetCurSpeed() => (GameSceneManager.Instance.CurGauage / SpeedRatio) * MoveSpeed;
        public float GetCurSpeed()
        {
#if !UNITY_EDITOR
            float testInput = 10;
            if (TokenStateShow.instance != null &&
                TokenStateShow.instance.InputText != "") 
            {
                float.TryParse(TokenStateShow.instance.InputText, out testInput);
            }

            float result = ((float)Managers.Token.CurSpeedMeterPerSec) * MoveSpeed * testInput;
            linearDeceleration.ValueUpdate(result);
#endif

            return linearDeceleration.value;
        }

        public void PlayerInputCheckForStop()
        {
            if(Managers.Token.LastEventTime >= INPUT_ANIMATE_STOP_TIME && !animateController.GetBoolNoInput())
            {
#if !UNITY_EDITOR
                animateController.PlayAnimateNoInput(true);
#endif
            }
            else if(Managers.Token.LastEventTime < INPUT_ANIMATE_STOP_TIME && animateController.GetBoolNoInput())
            {
                animateController.PlayAnimateNoInput(false);
            }
        }
        public void PlayerAnimateMove(bool trigger)
        {
            animateController.PlayAnimateMove(trigger);
        }
     
#region SetState
        
        public void SetStateNone() 
        {
            stateController.ChangeState(PlayerNone.Instance);
        }
        public void SetStateIdle()
        {
            stateController.ChangeState(PlayerIdle.Instance);
        }
#endregion SetState

        private void AddLeftToken()
        {
            Managers.Token.Save_left_speed += 50;
        }
        private void AddRightToken()
        {
            Managers.Token.Save_right_speed += 50;
        }
    }
}