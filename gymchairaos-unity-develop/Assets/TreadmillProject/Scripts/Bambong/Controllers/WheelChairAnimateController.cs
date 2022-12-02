using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace bambong 
{
    public class WheelChairAnimateController
    {
       
        private Animator animator;

        private readonly string MOVE_CHECK_BOOL = "IsRun";
        private readonly string NO_INPUT_CHECK_BOOL = "IsNoInput";
        private readonly string MOVE_ANIM_SPEED_FLOAT = "MoveSpeed";

        private readonly float MINMUM_ANIM_SPEED = 0.5f;
        private readonly float MAXMUM_ANIM_SPEED = 1.5f;

        public WheelChairAnimateController(Animator animator) 
        {
            this.animator = animator;
        }
        public void SetMoveAnimSpeed(float ratio)
        {
            var revision = MAXMUM_ANIM_SPEED - MINMUM_ANIM_SPEED;
            var speed = MINMUM_ANIM_SPEED + ratio * revision;
            animator.SetFloat(MOVE_ANIM_SPEED_FLOAT, speed);
        }

        public void PlayAnimateNoInput(bool trigger)
        {
            animator.SetBool(NO_INPUT_CHECK_BOOL, trigger);
        }
        public void PlayAnimateMove(bool trigger)
        {
            animator.SetBool(MOVE_CHECK_BOOL, trigger);
        }
        public bool GetBoolNoInput() => animator.GetBool(NO_INPUT_CHECK_BOOL);
    }

}
