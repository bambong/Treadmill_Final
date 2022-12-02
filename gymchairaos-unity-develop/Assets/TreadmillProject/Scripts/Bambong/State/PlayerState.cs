using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class PlayerNone : Singleton<PlayerNone>,IState<PlayerController> 
    {

    }
    public class PlayerIdle : Singleton<PlayerIdle>, IState<PlayerController>
    {

        public void UpdateActive(PlayerController playerController)
        {
            if(!GameSceneManager.Instance.CheckSpeedGageOring()) 
            {
                playerController.PlayerStateController.ChangeState(PlayerRun.Instance);
            }
        }
    }

    public class PlayerRun : Singleton<PlayerRun>, IState<PlayerController>
    {

        public void Enter(PlayerController playerController)
        {
          
            playerController.PlayerAnimateMove(true);

        }
        public void UpdateActive(PlayerController playerController)
        {
        
            playerController.PlayerMoveUpdate();
            playerController.PlayerInputCheckForStop();
            if(GameSceneManager.Instance.CheckSpeedGageOring())
            {
                playerController.PlayerStateController.ChangeState(PlayerIdle.Instance);
            }
        }
        public  void Exit(PlayerController playerController)
        {
            playerController.PlayerAnimateMove(false);
        }
    }

    public class PlayerDead : Singleton<PlayerDead>, IState<PlayerController>
    {

    }
}
