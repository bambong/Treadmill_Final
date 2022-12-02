using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class AI_StateController : StateController<AI_CharController>
    {
        public AI_StateController(AI_CharController owner) : base(owner)
        {
            curState = AI_Idle.Instance;
        }

    }

}
