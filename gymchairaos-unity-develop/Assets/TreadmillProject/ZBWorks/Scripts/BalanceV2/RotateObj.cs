using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Balance2
{
    public class RotateObj : MonoBehaviour
    {
        [SerializeField] Transform rotationTarget;
        [SerializeField] float rotateVec_Roll;
        [SerializeField] float rotateVec_Pitch;
        [SerializeField] float rotatePow;
        [SerializeField] float rotateMax;


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

        // Update is called once per frame
        private void FixedUpdate()
        {
            // Roll 회전
            if (CanRotate_Roll())
            {
                Vector3 currentRotation = rotationTarget.localEulerAngles;
                currentRotation.x += rotateVec_Roll * rotatePow * Time.fixedDeltaTime;
                //currentRotation.x = Mathf.Clamp(currentRotation.x, -rotateMax, rotateMax); // 원하는 회전 범위로 제한
                rotationTarget.localEulerAngles = currentRotation;
            }

            // Yaw 회전
            if (CanRotate_Pitch())
            {
                Vector3 currentRotation = rotationTarget.localEulerAngles;
                currentRotation.z += rotateVec_Pitch * rotatePow * Time.fixedDeltaTime;
                //currentRotation.x = Mathf.Clamp(currentRotation.x, -rotateMax, rotateMax); // 원하는 회전 범위로 제한
                rotationTarget.localEulerAngles = currentRotation;
            }
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