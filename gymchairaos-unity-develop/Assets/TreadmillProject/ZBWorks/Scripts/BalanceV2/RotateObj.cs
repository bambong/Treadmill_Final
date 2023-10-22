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

            //�̵��غ� ���� ���, ���� �Է¿� ���� �ش�����̵����� ��ȯ�ϰ�, �̵��� �����Ѵ�.
            if (dirSelect == DirSelect.none &&
                (Mathf.Abs(rotateVec_Roll) > 0.5f || Mathf.Abs(rotateVec_Pitch) > 0.5f)) 
            {
                if (Mathf.Abs(rotateVec_Roll) > Mathf.Abs(rotateVec_Pitch))
                {
                    dirSelect = DirSelect.roll;
                    return;
                }
                dirSelect = DirSelect.pitch;
                return;
            }

            //* �������� �Է¹����� �ش�������� �̵��Ѵ�.
            //roll ���� �̵�
            if (dirSelect == DirSelect.roll && Mathf.Abs(rotateVec_Pitch) < 0.5f) 
            {
                RotateRoll(rotateVec_Roll);
                return;
            }
            //pitch ���� �̵�
            if (dirSelect == DirSelect.pitch && Mathf.Abs(rotateVec_Roll) < 0.5f)
            {
                RotatePitch(rotateVec_Pitch);
                return;
            }

            //* ���ʹ��� �̵� ����, �ٸ����� �̵� �Է��� ������, �̵��غ� �����Ѵ�.
            //roll ���� �̵� �� �ٸ����� �Է�
            float rotDir;
            if (dirSelect == DirSelect.roll)
            {
                //����ġ �̵�...
                //�̵��ؾ��� ������ ���Ѵ�
                rotDir = rotationTarget.eulerAngles.x >= 330 && rotationTarget.eulerAngles.x <= 360 ? 1 : -1;
                RotateRoll(Mathf.Abs(rotateVec_Pitch) * rotDir);

                //����ġ �̵��Ϸ�
                if (Mathf.Abs(rotationTarget.eulerAngles.x) < 0.5f)
                {
                    rotationTarget.rotation = Quaternion.Euler(0, 0, rotationTarget.eulerAngles.z);
                    dirSelect = DirSelect.none;
                }
                return;
            }
            //pitch ���� �̵� �� �ٸ����� �Է�
            if (dirSelect == DirSelect.pitch)
            {
                //����ġ �̵�...
                rotDir = rotationTarget.eulerAngles.z >= 330 && rotationTarget.eulerAngles.z <= 360 ? 1 : -1;
                RotatePitch(Mathf.Abs(rotateVec_Roll) * rotDir);

                //����ġ �̵��Ϸ�
                if (Mathf.Abs(rotationTarget.eulerAngles.z) < 0.5f)
                {
                    rotationTarget.rotation = Quaternion.Euler(rotationTarget.eulerAngles.x, 0, 0);
                    dirSelect = DirSelect.none;
                }
                return;
            }

            //// Roll ȸ��
            //if (CanRotate_Roll())
            //{
            //    float x = rotationTarget.eulerAngles.x + rotateVec_Roll * rotatePow * Time.fixedDeltaTime;
            //    if (x > 30 && x < 330)
            //        x = Mathf.Abs(x - 30) < Mathf.Abs(x - 330) ? 30 : 330;

            //    rotationTarget.rotation =
            //        Quaternion.Euler(
            //            x,
            //            0,
            //            rotationTarget.eulerAngles.z);
            //}

            //// Yaw ȸ��
            //if (CanRotate_Pitch())
            //{
            //    float z = rotationTarget.eulerAngles.z + rotateVec_Pitch * rotatePow * Time.fixedDeltaTime;
            //    if (z > 30 && z < 330)
            //        z = Mathf.Abs(z - 30) < Mathf.Abs(z - 330) ? 30 : 330;

            //    rotationTarget.rotation =
            //        Quaternion.Euler(
            //            rotationTarget.eulerAngles.x,
            //            0,
            //            z);
            //}
        }
        private void RotateRoll(float value)
        {
            float x = rotationTarget.eulerAngles.x + value * rotatePow * Time.fixedDeltaTime;
            if (x > 30 && x < 330)
                x = Mathf.Abs(x - 30) < Mathf.Abs(x - 330) ? 30 : 330;

            rotationTarget.rotation =
                Quaternion.Euler(
                    x,
                    0,
                    rotationTarget.eulerAngles.z);
        }
        private void RotatePitch(float value)
        {
            float z = rotationTarget.eulerAngles.z + value * rotatePow * Time.fixedDeltaTime;
            if (z > 30 && z < 330)
                z = Mathf.Abs(z - 30) < Mathf.Abs(z - 330) ? 30 : 330;

            rotationTarget.rotation =
                Quaternion.Euler(
                    rotationTarget.eulerAngles.x,
                    0,
                    z);
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