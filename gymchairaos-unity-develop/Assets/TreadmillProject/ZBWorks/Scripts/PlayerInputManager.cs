using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;

namespace ZB
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] PlayerMoveController move;
        [SerializeField] PlayerAnimationController ani;
        [SerializeField] ObjectsScrolling scroll;

        [SerializeField] float h;
        [SerializeField] float scrollSpeedMultiple;
        [SerializeField] float scrollMinSpeed;
        [SerializeField] float sideMoveMultiple;

        void Awake()
        {
#if UNITY_EDITOR
            Managers.Token.AddLeftTokenEvent(AddLeftToken);
            Managers.Token.AddRightTokenEvent(AddRightToken);
            StartCoroutine(DecreaseToken());
#endif
            Managers.Token.ReceivedEvent += SideMove;
        }
        public void AddLeftToken()
        {
            Managers.Token.Save_left_speed += 10;
        }
        public void AddRightToken()
        {
            Managers.Token.Save_right_speed += 10;
        }
        public IEnumerator DecreaseToken()
        {
            while (gameObject != null)
            { 
                Managers.Token.Save_left_speed = Mathf.Max(0, Managers.Token.Save_left_speed - 1);
                Managers.Token.Save_right_speed = Mathf.Max(0, Managers.Token.Save_right_speed - 1);
                yield return new WaitForSeconds(0.2f);
            }
        }

        void Update()
        {
            //h = Input.GetAxis("Horizontal");
            //move.MoveInput(h);
            Debug.Log("update");

            //회전속도에 따른 스크롤속도 조정

            Debug.Log("token Is On");
            scroll.ScrollSpeedChange(Managers.Token.CurSpeed);
        
        }

        void SideMove()
        {
            Debug.Log("!@#!@#!@$!@$$#@ " + Managers.Token.Save_left_speed + " " +Managers.Token.Save_right_speed);



            var speed = 240f;
            var dis = Managers.Token.Save_right_speed - Managers.Token.Save_left_speed;

            if (Mathf.Abs(dis) < 100)
            {
                speed = 0;
            }
            else
            {
                speed *= dis > 0 ? 1 : -1;
            }

            move.MoveInput(
                (speed) * sideMoveMultiple);
        }
    }
}