using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace bambong 
{
    public class GameStateController : StateController<GameSceneManager>
    {
        public GameStateController(GameSceneManager gameSceneManager):base(gameSceneManager)
        {
            Init();
        }
        public bool IsStateGamePlaying() => curState == GamePlay.Instance;
        public void Init()
        {
        
            curState = GameNone.Instance;
        }
    }
}
