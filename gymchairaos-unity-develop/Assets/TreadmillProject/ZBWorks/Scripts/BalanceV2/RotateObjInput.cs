using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Balance2
{
    public class RotateObjInput : MonoBehaviour
    {
        [SerializeField] RotateObj rotateObj;
        [SerializeField] float yaw;
        [SerializeField] float roll;

        [SerializeField] float pow;
        [SerializeField] float maxPow;

        [SerializeField] bool active;

        public void ResetState()
        {
            yaw = 0;
            roll = 0;
        }
        public void Active(bool active)
        {
            this.active = active;
        }
        private void Update()
        {
            if (active)
            {
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

                rotateObj.RotateInfoUpdate(yaw, roll);
            }
        }
    }

}