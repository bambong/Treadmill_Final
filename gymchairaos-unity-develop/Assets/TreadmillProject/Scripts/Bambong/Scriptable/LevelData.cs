using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace bambong 
{
   [Serializable]
   public class LevelInfo 
   {
        [Header("������ AI")]
        public List<SpeedRevisionData> AI_Dtatas;
        [Header("�� ����")]
        public float distance;
   }

    [CreateAssetMenu(fileName = "LevelData",menuName = "Scriptable/LevelData",order = 0)] 
    public class LevelData : ScriptableObject 
    {
        public List<LevelInfo> levelInfos;
    }

}