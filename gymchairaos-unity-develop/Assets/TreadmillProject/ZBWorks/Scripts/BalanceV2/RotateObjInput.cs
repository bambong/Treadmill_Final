using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace ZB.Balance2
{
    public class RotateObjInput : MonoBehaviour
    {
        public enum RotDir { front, back, left, right}
        [SerializeField] RotateObj rotateObj;
        [SerializeField] float yaw;
        [SerializeField] float roll;

        [SerializeField] float pow;
        [SerializeField] float maxPow;

        [SerializeField] bool active;

        private readonly float MIN_INPUT= 0.1f; 
        public void ResetState()
        {
            yaw = 0;
            roll = 0;
        }
        public void Active(bool active)
        {
            this.active = active;
        }

        public bool RotDirCheck_Front(RotDir rotDir)
        {
            bool result = false;
            float max = 29;
            float min = 5;
            switch (rotDir)
            {
                case RotDir.front:
                    if (roll > max &&
                        Mathf.Abs(yaw) < min)
                        result = true;
                    break;
                case RotDir.back:
                    if (roll < -max &&
                        Mathf.Abs(yaw) < min)
                        result = true;
                    break;
                case RotDir.left:
                    if (yaw < -max &&
                        Mathf.Abs(roll) < min)
                        result = true;
                    break;
                case RotDir.right:
                    if (yaw > max &&
                        Mathf.Abs(roll) < min)
                        result = true;
                    break;
            }
            return result;
        }

        private void Update()
        {
            if (active)
            {
#if !UNITY_EDITOR
                 var left = TokenInputManager.Instance.Save_left_rpm;
                var right = TokenInputManager.Instance.Save_right_rpm;
                if (MIN_INPUT < Mathf.Abs(left - right)) 
                {
                    if(left < right)
                    {
                        if (yaw > -maxPow)
                        {
                            yaw -= Time.deltaTime * pow;
                        }
                    }
                    else 
                    {
                        if (yaw < maxPow)
                        {
                            yaw += Time.deltaTime * pow;
                        }
                    }
                }
                else 
                {
                    yaw = Mathf.Lerp(0, yaw, Time.deltaTime);
                }
                var curDir = left + right;
                if (MIN_INPUT < Mathf.Abs(curDir)) 
                {

                    if (curDir > 0)
                    {
                        if (roll < maxPow)
                        {
                            roll += Time.deltaTime * pow;
                        }
                    }
                    else
                    {
                        if (roll > -maxPow)
                        {
                            roll -= Time.deltaTime * pow;
                        }

                    }
                }
                else
                {
                    roll = Mathf.Lerp(0, roll, Time.deltaTime);
                }

#else
                if (Input.GetKey(KeyCode.D) && yaw < maxPow)
                {
                    yaw += Time.deltaTime * pow;
                }
                else if (yaw > 0)
                {
                    yaw -= Time.deltaTime * pow;
                }
                if (Input.GetKey(KeyCode.A) && yaw > -maxPow)
                {
                    yaw -= Time.deltaTime * pow;
                }
                else if (yaw < 0)
                {
                    yaw += Time.deltaTime * pow;
                }

                if (Input.GetKey(KeyCode.K) && roll < maxPow)
                {
                    roll += Time.deltaTime * pow;
                }
                else if (roll > 0)
                {
                    roll -= Time.deltaTime * pow;
                }
                if (Input.GetKey(KeyCode.M) && roll > -maxPow)
                {
                    roll -= Time.deltaTime * pow;
                }
                else if (roll < 0)
                {
                    roll += Time.deltaTime * pow;
                }
#endif
                rotateObj.RotateInfoUpdate(yaw, roll);
            }
            else
            {
                roll *= Time.deltaTime;
                yaw *= Time.deltaTime;

                rotateObj.RotateInfoUpdate(yaw, roll);
            }
        }
    }

}