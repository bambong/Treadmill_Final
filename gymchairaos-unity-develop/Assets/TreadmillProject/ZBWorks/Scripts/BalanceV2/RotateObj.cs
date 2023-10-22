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

            //이동준비가 끝난 경우, 다음 입력에 따라 해당방향이동으로 전환하고, 이동을 시작한다.
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

            //* 기존방향 입력받으면 해당방향으로 이동한다.
            //roll 방향 이동
            if (dirSelect == DirSelect.roll && Mathf.Abs(rotateVec_Pitch) < 0.5f) 
            {
                RotateRoll(rotateVec_Roll);
                return;
            }
            //pitch 방향 이동
            if (dirSelect == DirSelect.pitch && Mathf.Abs(rotateVec_Roll) < 0.5f)
            {
                RotatePitch(rotateVec_Pitch);
                return;
            }

            //* 한쪽방향 이동 도중, 다른방향 이동 입력을 받으면, 이동준비를 시작한다.
            //roll 방향 이동 중 다른방향 입력
            float rotDir;
            if (dirSelect == DirSelect.roll)
            {
                //원위치 이동...
                //이동해야할 방향을 구한다
                rotDir = rotationTarget.eulerAngles.x >= 330 && rotationTarget.eulerAngles.x <= 360 ? 1 : -1;
                RotateRoll(Mathf.Abs(rotateVec_Pitch) * rotDir);

                //원위치 이동완료
                if (Mathf.Abs(rotationTarget.eulerAngles.x) < 0.5f)
                {
                    rotationTarget.rotation = Quaternion.Euler(0, 0, rotationTarget.eulerAngles.z);
                    dirSelect = DirSelect.none;
                }
                return;
            }
            //pitch 방향 이동 중 다른방향 입력
            if (dirSelect == DirSelect.pitch)
            {
                //원위치 이동...
                rotDir = rotationTarget.eulerAngles.z >= 330 && rotationTarget.eulerAngles.z <= 360 ? 1 : -1;
                RotatePitch(Mathf.Abs(rotateVec_Roll) * rotDir);

                //원위치 이동완료
                if (Mathf.Abs(rotationTarget.eulerAngles.z) < 0.5f)
                {
                    rotationTarget.rotation = Quaternion.Euler(rotationTarget.eulerAngles.x, 0, 0);
                    dirSelect = DirSelect.none;
                }
                return;
            }

            //// Roll 회전
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

            //// Yaw 회전
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