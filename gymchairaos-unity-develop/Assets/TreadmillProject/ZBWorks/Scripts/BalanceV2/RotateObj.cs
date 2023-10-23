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

        private void FixedUpdate()
        {
            //roll ��ǲ�� �ֳľ��Ŀ� ���� ���� �ൿ�� ��������
            //������ rollȸ��, ������ pitchȸ���� �ϵ����Ѵ�

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
            //x������ input ���� ���� / input�� +���� -�������� ��ȯ�ص�, �ٷ� -�� �ȉ� / ���� +���� ��Ӵ�������, ��¦ �ش�������� ġ������
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