using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace bambong 
{
    public class AI_Run : Singleton<AI_Run> , IState<AI_CharController> 
    {
        public void Enter(AI_CharController aiController) 
        {
            aiController.AnimateMove(true);
        }
        public void UpdateActive(AI_CharController aiController) 
        {
            aiController.MoveUpdate();
        }
        public void Exit(AI_CharController aiController)
        {
            aiController.AnimateMove(false);
        }
    }
    public class AI_Stop : Singleton<AI_Stop>, IState<AI_CharController>
    {
       
    }
    public class AI_Idle : Singleton<AI_Idle>, IState<AI_CharController>
    {

    }

}
