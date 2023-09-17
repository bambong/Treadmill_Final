using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class GameNone : Singleton<GameNone>, IState<GameSceneManager>
    {
        public void UpdateActive(GameSceneManager gameSceneManager)
        {
            if (Managers.Token.IsConnect) 
            {
                Debug.Log("Check IsConnect");
                gameSceneManager.SetStateGameStart();
            }
        }
    }
    public class GameInit : Singleton<GameInit>, IState<GameSceneManager>
    {
    }
    public class GameWait : Singleton<GameWait>, IState<GameSceneManager>
    {
    }
    public class GameStart : Singleton<GameStart>, IState<GameSceneManager>
    {
    }
    public class GamePlay : Singleton<GamePlay>, IState<GameSceneManager>
    {
        public void UpdateActive(GameSceneManager gameSceneManager)
        {
            gameSceneManager.UpdateOnStateGamePlay();
        }
    }
    public class GamePause : Singleton<GamePause>, IState<GameSceneManager>
    {
    }
    public class GameOver : Singleton<GameOver>, IState<GameSceneManager>
    {
    }
    public class GameClear : Singleton<GameClear>, IState<GameSceneManager>
    {
    }
}
