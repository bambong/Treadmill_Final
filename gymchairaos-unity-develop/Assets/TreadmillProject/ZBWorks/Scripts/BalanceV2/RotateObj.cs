using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        private void FixedUpdate()
        {
            // Roll 회전
            if (CanRotate_Roll())
            {
                float x = rotationTarget.eulerAngles.x + rotateVec_Roll * rotatePow * Time.fixedDeltaTime;
                if (x > 89 && x < 271)
                    x = Mathf.Abs(x - 89) < Mathf.Abs(x - 271) ? 89 : 271;

                rotationTarget.rotation =
                    Quaternion.Euler(
                        x,
                        0,
                        rotationTarget.eulerAngles.z);
            }

            // Yaw 회전
            if (CanRotate_Pitch())
            {
                float z = rotationTarget.eulerAngles.z + rotateVec_Pitch * rotatePow * Time.fixedDeltaTime;
                if (z > 89 && z < 271)
                    z = Mathf.Abs(z - 89) < Mathf.Abs(z - 271) ? 89 : 271;

                rotationTarget.rotation =
                    Quaternion.Euler(
                        rotationTarget.eulerAngles.x,
                        0,
                        z);
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