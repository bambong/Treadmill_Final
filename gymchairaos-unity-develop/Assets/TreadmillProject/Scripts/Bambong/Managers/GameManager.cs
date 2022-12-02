using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace bambong 
{
    
    public class GameManager : Singleton<GameManager>
    {
       

        private int currentLevel = 0;
        public int CurrentLevel { get { return currentLevel; } }
        private readonly string LEVEL_CLEARED_PLAYER_PREFAB = "SPEEDGAME_LV_CLEARED";
        private readonly int Clear = 1;
        private readonly int NotClear = 0;
        
        public void InitLevel(int level) 
        {
            if(!PlayerPrefs.HasKey(LEVEL_CLEARED_PLAYER_PREFAB + level)) 
            {
                PlayerPrefs.SetInt(LEVEL_CLEARED_PLAYER_PREFAB + level,NotClear);           
            }
        
        }
        public bool IsOpenLevel(int level) 
        {
            if(level <= 0) 
            {
                return true;
            }

            return PlayerPrefs.GetInt(LEVEL_CLEARED_PLAYER_PREFAB + (level - 1)) == Clear;

        }
        public void ClearLevel(int level) 
        {
            PlayerPrefs.SetInt(LEVEL_CLEARED_PLAYER_PREFAB + level,Clear);
        }
        public void SetLevel(int level ) 
        {
            currentLevel = level;
        }
        
    }


}

