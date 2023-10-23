using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZB.Balance2
{
    public class RotateObj : MonoBehaviour
    {
        public enum DirSelect { none, roll, pitch}

        [SerializeField] DirSelect dirSelect;
        //[SerializeField] DirSelect wantDir;
        [SerializeField] Transform rotationTarget;
        [SerializeField] float rotateVec_Roll;
        [SerializeField] float rotateVec_Pitch;
        [SerializeField] float rotatePow;
        [SerializeField] float rotateMax;
        [SerializeField] float lerpPow = 1;
        private readonly float MIN_POW = 0.5f;
       
        /// <summary>
        /// 회전정보 업데이트
        /// </summary>
        /// <param name="x축회전"></param>
        /// <param name="z축회전"></param>
        public void RotateInfoUpdate(float rotateVec_Roll, float rotateVec_Pitch)
        {
            this.rotateVec_Roll = rotateVec_Roll;
            this.rotateVec_Pitch = rotateVec_Pitch;
        }

        public void ResetState()
        {
            rotationTarget.eulerAngles = Vector3.zero;
            rotateVec_Roll = 0;
            rotateVec_Pitch = 0;
        }

        private void FixedUpdate()
        {
            //roll 인풋이 있냐없냐에 따라 다음 행동이 정해진다
            //있으면 roll회전, 없으면 pitch회전을 하도록한다

            float rollPow = Mathf.Abs(rotateVec_Roll);
            float pitchPow = Mathf.Abs(rotateVec_Pitch);

            if(rollPow < MIN_POW && pitchPow < MIN_POW)
            {
                //rotationTarget.rotation = Quaternion.Lerp(rotationTarget.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * lerpPow);
                return;
            }
#if UNITY_EDITOR
            if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D)) 
            {
                RotateRoll(rotateVec_Roll);
            }
            else if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.M))
            {
                RotatePitch(rotateVec_Pitch);
            }

#else
            if (Managers.Token.Save_left_speed < -1990 || Managers.Token.Save_right_speed < -1990)
            {
                return;
            }  
            
            var left = Managers.Token.Save_left_speed;
            var right = Managers.Token.Save_right_speed;
 
            if(left * right < 0)
            {
                RotateRoll(rotateVec_Roll);
            }
            else 
            {
                RotatePitch(rotateVec_Pitch);
            }
#endif

        }
        private void RotateRoll(float value)
        {
            //x값에서 input 값을 더함 / input을 +에서 -방향으로 전환해도, 바로 -가 안됌 / 남은 +값이 계속더해져서, 살짝 해당방향으로 치우쳐짐
            float x = rotationTarget.eulerAngles.x + value * rotatePow ;
            if (x > 30 && x < 330)
                x = Mathf.Abs(x - 30) < Mathf.Abs(x - 330) ? 30 : 330;

            rotationTarget.rotation =Quaternion.Slerp( rotationTarget.rotation, Quaternion.Euler(x, 0, 0), Time.fixedDeltaTime * lerpPow);
 
        }
        private void RotatePitch(float value)
        {
            float z = rotationTarget.eulerAngles.z + value * rotatePow;
            if (z > 30 && z < 330)
            {
                z = Mathf.Abs(z - 30) < Mathf.Abs(z - 330) ? 30 : 330;
            }

            rotationTarget.rotation = Quaternion.Slerp(rotationTarget.rotation, Quaternion.Euler(0, 0, z), Time.fixedDeltaTime* lerpPow);
        }
        private bool CanRotate_Roll()
        {
            return true;
        }
        private bool CanRotate_Pitch()
        {
            return true;
        }
    }
}