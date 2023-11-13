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

                if(left < -1990 || right < -1990)
                {
                    return;
                }  

                if (left * right < 0 &&
                    MIN_INPUT < Mathf.Abs(left - right)) 
                {
                    if(left < right)
                    {
                        if (roll > -maxPow)
                        {
                            roll = -Mathf.Abs((float)Managers.Token.CurSpeedMeterPerSec) * pow;
                        }
                    }
                    else 
                    {
                        if (roll < maxPow)
                        {
                            roll = Mathf.Abs((float)Managers.Token.CurSpeedMeterPerSec) * pow;
                        }
                    }
                }
                else 
                {
                    roll = 0;
                }
                var curDir = left + right;
                if (left * right > 0 &&
                    MIN_INPUT < Mathf.Abs(curDir)) 
                {

                    if (curDir < 0)
                    {
                        if (pitch < maxPow)
                        {
                            pitch = Mathf.Abs((float)Managers.Token.CurSpeedMeterPerSec) * pow;
                        }
                    }
                    else
                    {
                        if (pitch > -maxPow)
                        {
                            pitch = -Mathf.Abs((float)Managers.Token.CurSpeedMeterPerSec) * pow;
                        }

                    }
                }
                else
                {
                    pitch = 0;
                }

#else
                bool rollInput = false;
                bool pitchInput = false;
                
                if (Input.GetKey(KeyCode.D))
                {

                    if (roll < 0) roll = 0;
                    roll += Time.deltaTime * pow;
                    rollInput = true;
                }        
                else if (Input.GetKey(KeyCode.A))
                {
                    if (roll > 0) roll = 0;
                    roll -= Time.deltaTime * pow;
                    rollInput = true;
                }

                if (!rollInput)
                {
                    roll = Mathf.Lerp(roll, 0, Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.K))
                {
                    if (pitch < 0) pitch = 0;
                    pitch += Time.deltaTime * pow;
                    pitchInput = true;
                }
                else if (Input.GetKey(KeyCode.M))
                {
                    if (pitch > 0) pitch = 0;
                    pitch -= Time.deltaTime * pow;
                    pitchInput = true;
                }

                if (!pitchInput)
                {
                    pitch = Mathf.Lerp(pitch, 0, Time.deltaTime);
                }


                pitch = Mathf.Clamp(pitch, -maxPow, maxPow);
                roll = Mathf.Clamp(roll, -maxPow, maxPow);

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