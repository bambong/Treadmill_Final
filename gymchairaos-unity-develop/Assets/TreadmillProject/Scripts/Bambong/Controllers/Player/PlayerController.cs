using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bambong
{

    public class PlayerController : MonoBehaviour
    {
        #region SerializeField
        [SerializeField]
        private Animator playerAnimator;
        
        [SerializeField]
        private float moveSpeed = 1f;
     
        [SerializeField]
        private float speedRatio = 1f;

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

        private void Awake()
        {
            stateController = new PlayerStateController(this);
            animateController = new WheelChairAnimateController(playerAnimator);
            arrowStartY = arrowTrs.transform.localPosition.y;

        }
        public void ArrowAnimStart()
        {
            arrowTrs.DOLocalMoveY(arrowStartY + 0.5f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);
            arrowTrs.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        void Update()
        {
            stateController.Update();
        }

        public void PlayerMoveUpdate()
        {

            var speed = GetCurSpeed() * Time.deltaTime;
            //var speed = TokenInputManager.Instance.CurRpm * 0.02f * Time.deltaTime;
            var pos = transform.position;
            pos += transform.forward * speed; 
            transform.position = pos;
            animateController.SetMoveAnimSpeed(GameSceneManager.Instance.GetGaugeRatio());
            GameSceneManager.Instance.AddDistance();
        }
        public float GetCurSpeed() => (GameSceneManager.Instance.CurGauage / SpeedRatio) * MoveSpeed;


        public void PlayerInputCheckForStop()
        {
            if(TokenInputManager.Instance.LastEventTime >= INPUT_ANIMATE_STOP_TIME && !animateController.GetBoolNoInput())
            {
                animateController.PlayAnimateNoInput(true);
            }
            else if(TokenInputManager.Instance.LastEventTime < INPUT_ANIMATE_STOP_TIME && animateController.GetBoolNoInput())
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


    }
}