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
        /// ȸ������ ������Ʈ
        /// </summary>
        /// <param name="x��ȸ��"></param>
        /// <param name="z��ȸ��"></param>
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
            // Roll ȸ��
            if (CanRotate_Roll())
            {
                Vector3 currentRotation = rotationTarget.localEulerAngles;
                currentRotation.x += rotateVec_Roll * rotatePow * Time.fixedDeltaTime;
                //currentRotation.x = Mathf.Clamp(currentRotation.x, -rotateMax, rotateMax); // ���ϴ� ȸ�� ������ ����
                rotationTarget.localEulerAngles = currentRotation;
            }

            // Yaw ȸ��
            if (CanRotate_Pitch())
            {
                Vector3 currentRotation = rotationTarget.localEulerAngles;
                currentRotation.z += rotateVec_Pitch * rotatePow * Time.fixedDeltaTime;
                //currentRotation.x = Mathf.Clamp(currentRotation.x, -rotateMax, rotateMax); // ���ϴ� ȸ�� ������ ����
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