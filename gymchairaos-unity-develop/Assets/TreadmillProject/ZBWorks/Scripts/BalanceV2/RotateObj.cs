using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Balance2
{
    public class RotateObj : MonoBehaviour
    {
        [SerializeField] Transform rotationTarget;
        [SerializeField] Rigidbody rigidbody;
        [SerializeField] float rotateVec_Roll;
        [SerializeField] float rotateVec_Yaw;
        [SerializeField] float rotatePow;
        [SerializeField] float rotateMax;


        /// <summary>
        /// 회전정보 업데이트
        /// </summary>
        /// <param name="x축회전"></param>
        /// <param name="z축회전"></param>
        public void RotateInfoUpdate(float rotateVec_Roll, float rotateVec_Yaw)
        {
            this.rotateVec_Roll = rotateVec_Roll;
            this.rotateVec_Yaw = rotateVec_Yaw;
        }

        public void ResetState()
        {
            rotationTarget.eulerAngles = Vector3.zero;
            rotateVec_Roll = 0;
            rotateVec_Yaw = 0;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (CanRotate_Yaw())
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.AngleAxis(rotateVec_Yaw * rotatePow * Time.fixedDeltaTime, Vector3.forward) * Quaternion.AngleAxis(rotateVec_Roll * rotatePow * Time.fixedDeltaTime, Vector3.right));
        }
        private bool CanRotate_Roll()
        {
            return true;
        }
        private bool CanRotate_Yaw()
        {
            return true;
        }
    }
}