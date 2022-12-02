using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class PlayerStateController : StateController<PlayerController>
    {
        public PlayerStateController(PlayerController player ):base(player)
        {
            Init();
        }
        public void Init()
        {
         
            curState = PlayerNone.Instance;
        }
    }

}
