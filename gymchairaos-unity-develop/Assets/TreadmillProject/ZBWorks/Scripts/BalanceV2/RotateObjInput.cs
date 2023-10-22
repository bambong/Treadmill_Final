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
        [SerializeField] float roll;
        [SerializeField] float pitch;

        [Space]
        [SerializeField] float pow;
        [SerializeField] float maxPow;
        [SerializeField] private float MIN_INPUT= 0.1f; 

        [SerializeField] bool active;

        public void ResetState()
        {
            roll = 0;
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
                        Mathf.Abs(roll) < min)
                        result = true;
                    break;
                case RotDir.back:
                    if (pitch < -max &&
                        Mathf.Abs(roll) < min)
                        result = true;
                    break;
                case RotDir.left:
                    if (roll < -max &&
                        Mathf.Abs(pitch) < min)
                        result = true;
                    break;
                case RotDir.right:
                    if (roll > max &&
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
                //if (MIN_INPUT < Mathf.Abs(left - right)) 
                if (left * right < 0 &&
                    MIN_INPUT < Mathf.Abs(left - right)) 
                {
                    if(left < right)
                    {
                        if (roll > -maxPow)
                        {
                            roll = -Mathf.Abs(Managers.Token.CurSpeedMeterPerSec) * pow;
                        }
                    }
                    else 
                    {
                        if (roll < maxPow)
                        {
                            roll = Mathf.Abs(Managers.Token.CurSpeedMeterPerSec) * pow;
                        }
                    }
                }
                else 
                {
                    roll = Mathf.Lerp(0, roll, Time.deltaTime);
                }
                var curDir = left + right;
                if (left * right > 0 &&
                    MIN_INPUT < Mathf.Abs(curDir)) 
                {

                    if (curDir < 0)
                    {
                        if (pitch < maxPow)
                        {
                            pitch = Mathf.Abs(Managers.Token.CurSpeedMeterPerSec) * pow;
                        }
                    }
                    else
                    {
                        if (pitch > -maxPow)
                        {
                            pitch = -Mathf.Abs(Managers.Token.CurSpeedMeterPerSec) * pow;
                        }

                    }
                }
                else
                {
                    pitch = Mathf.Lerp(0, pitch, Time.deltaTime);
                }

#else
                if (Input.GetKey(KeyCode.D) && roll < maxPow)
                {
                    roll += Time.deltaTime * pow;
                }
                else if (roll > 0)
                {
                    roll -= Time.deltaTime * pow;
                }
                if (Input.GetKey(KeyCode.A) && roll > -maxPow)
                {
                    roll -= Time.deltaTime * pow;
                }
                else if (roll < 0)
                {
                    roll += Time.deltaTime * pow;
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
                rotateObj.RotateInfoUpdate(roll, pitch);
            }
            else
            {
                pitch *= Time.deltaTime;
                roll *= Time.deltaTime;

                rotateObj.RotateInfoUpdate(roll, pitch);
            }

            tmp_roll.text = $"roll : {roll}";
            tmp_pitch.text = $"pitch : {pitch}";
        }

        [Space(30)]
        [SerializeField] TMP_InputField input_MININPUT;
        [SerializeField] TMP_InputField input_power;
        [SerializeField] TMP_InputField input_maxPower;
        [SerializeField] TextMeshProUGUI tmp_roll;
        [SerializeField] TextMeshProUGUI tmp_pitch;
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