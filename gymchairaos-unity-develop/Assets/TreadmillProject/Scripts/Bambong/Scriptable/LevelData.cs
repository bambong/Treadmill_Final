using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace bambong 
{
   [Serializable]
   public class LevelInfo 
   {
        [Header("생성할 AI")]
        public List<SpeedRevisionData> AI_Dtatas;
        [Header("맵 길이")]
        public float distance;
   }

    [CreateAssetMenu(fileName = "LevelData",menuName = "Scriptable/LevelData",order = 0)] 
    public class LevelData : ScriptableObject 
    {
        public List<LevelInfo> levelInfos;
    }

}