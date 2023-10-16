using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using TMPro;

namespace ZB.Balance2
{
    public class RotateObjInput : MonoBehaviour
    {
        public enum RotDir { front, back, left, right}
        [SerializeField] RotateObj rotateObj;
        [SerializeField] float yaw;
        [SerializeField] float pitch;

        [Space]
        [SerializeField] float pow;
        [SerializeField] float maxPow;
        [SerializeField] private float MIN_INPUT= 0.1f; 

        [SerializeField] bool active;

        public void ResetState()
        {
            yaw = 0;
            pitch = 0;
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
                    if (pitch > max &&
                        Mathf.Abs(yaw) < min)
                        result = true;
                    break;
                case RotDir.back:
                    if (pitch < -max &&
                        Mathf.Abs(yaw) < min)
                        result = true;
                    break;
                case RotDir.left:
                    if (yaw < -max &&
                        Mathf.Abs(pitch) < min)
                        result = true;
                    break;
                case RotDir.right:
                    if (yaw > max &&
                        Mathf.Abs(pitch) < min)
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
                 var left = Managers.Token.Save_left_speed;
                var right = Managers.Token.Save_right_speed;
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
                        if (pitch < maxPow)
                        {
                            pitch += Time.deltaTime * pow;
                        }
                    }
                    else
                    {
                        if (pitch > -maxPow)
                        {
                            pitch -= Time.deltaTime * pow;
                        }

                    }
                }
                else
                {
                    pitch = Mathf.Lerp(0, pitch, Time.deltaTime);
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

                if (Input.GetKey(KeyCode.K) && pitch < maxPow)
                {
                    pitch += Time.deltaTime * pow;
                }
                else if (pitch > 0)
                {
                    pitch -= Time.deltaTime * pow;
                }
                if (Input.GetKey(KeyCode.M) && pitch > -maxPow)
                {
                    pitch -= Time.deltaTime * pow;
                }
                else if (pitch < 0)
                {
                    pitch += Time.deltaTime * pow;
                }
#endif
                rotateObj.RotateInfoUpdate(yaw, pitch);
            }
            else
            {
                pitch *= Time.deltaTime;
                yaw *= Time.deltaTime;

                rotateObj.RotateInfoUpdate(yaw, pitch);
            }
        }

        [Space(30)]
        [SerializeField] TMP_InputField input_MININPUT;
        [SerializeField] TMP_InputField input_power;
        [SerializeField] TMP_InputField input_maxPower;
        public void TestInputField()
        {
            float result = 0;
            if (float.TryParse(input_MININPUT.text, out result))
                MIN_INPUT = result;
            if (float.TryParse(input_power.text, out result))
                pow = result;
            if (float.TryParse(input_maxPower.text, out result))
                maxPow = result;
        }
    }

}